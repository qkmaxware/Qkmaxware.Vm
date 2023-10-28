using System.Reflection;
using System.Text.RegularExpressions;

namespace Qkmaxware.Vm.Assembly;

// Examples
/*
// File Main.qkasm


// File Library.qkasm


// Assemble and link the 2
// qkvm assemble -f Main.qkasm
// qkvm assemble -f Library.qkasm
// qkvm link -f Main.qkbc -l Library.qkbc -o out.qkbc
// qkvm run -f out.qkbc
*/

/// <summary>
/// Parser for "asm 1.x" assembly files
/// </summary>
public class Asm1xParser : IAssemblyParser {
    public bool SupportsVersion(string dialect, int Major, int Minor) => dialect == "asm" && Major == 1;

    private static Regex commentPattern = new Regex(@"\/\/(.*)"); //

    public Module Parse(TextReader reader) {
        using var builder = new ModuleBuilder();
        Dictionary<string, Label> labels = new Dictionary<string, Label>();
        List<LabelThunk> label_thunks = new List<LabelThunk>();
        Dictionary<string, MemoryRef> constants = new Dictionary<string, MemoryRef>();

        string? line = null;
        long lineIndex = 0;
        while ((line = reader.ReadLine()) != null) {
            try {
                // Ignore empty lines
                line = commentPattern.Replace(line, string.Empty).Trim();
                if (string.IsNullOrEmpty(line))
                    continue;

                // Handle different line types
                if (line.StartsWith("import ")) {
                    handleImport(builder, line);
                } 
                else if (line.StartsWith("export ")) {
                    handleExport(builder, line);
                }
                else if (line.StartsWith('@')) {
                    handleConstant(builder, labels, constants, line);
                }
                else if (line.StartsWith('.')) {
                    handleLabel(builder, labels, label_thunks, constants, line);
                }
                else if (line.StartsWith("!")) {
                    handleMacro(builder, labels, constants, line);
                }
                else {
                    handleInstruction(builder, labels, label_thunks, constants, line);
                }

                // Increment line index
                lineIndex ++;
            } catch (Exception e) {
                throw new AssemblyException(lineIndex, e);
            }
        }

        var stillWaiting = label_thunks.SelectMany(x => x.AwaitingLabels).Distinct().ToList();
        if (stillWaiting.Any()) {
            throw new ArgumentException($"Labels {string.Join(',', stillWaiting)} are not defined.");
        }
        return builder.ToModule();
    }

    private static Regex importPattern = new Regex(@"^\s*import\s+(?<value>"".+)");
    private void handleImport(ModuleBuilder builder, string line) {
        var match = importPattern.Match(line);
        if (!match.Success) {
            throw new FormatException("Invalid import format, missing doubly quoted import name");
        }
        var value = System.Text.Json.JsonSerializer.Deserialize<string>(match.Groups["value"].Value);
        if (value == null)  
            throw new FormatException("Invalid string representation for name");
        builder.ImportSubprogram(value);
    }

    private static Regex exportPattern = new Regex(@"^\s*export\s+(?<value>"".+)");
    private void handleExport(ModuleBuilder builder, string line) {
        var match = exportPattern.Match(line);
        if (!match.Success) {
            throw new FormatException("Invalid export format, missing doubly quoted exported name");
        }
        var value = System.Text.Json.JsonSerializer.Deserialize<string>(match.Groups["value"].Value);
        if (value == null)  
            throw new FormatException("Invalid string representation for name");
        builder.ExportSubprogram(value);
    }

    private static Regex constantPattern = new Regex(@"^\s*\@(?<id>\w+)\s*=\s*(?<value>.+)");
    private void handleConstant(ModuleBuilder builder, Dictionary<string, Label> labels, Dictionary<string, MemoryRef> consts, string line) {
        var match = constantPattern.Match(line);
        if (!match.Success) {
            throw new FormatException("Invalid constant format. Constants should be named using an '@' followed by any number of the following digits [a-zA-Z0-9_] and assigned a value after the '=' sign.");
        }
        var id = match.Groups["id"].Value;
        var value = match.Groups["value"].Value;

        var constantValue = value[0] switch {
            '"' => readString(value),
            char c when (c == '+' || c == '-' || char.IsDigit(c)) => readNumber(value),
            _ => throw new InvalidDataException($"Unknown type of value '{value}'.")
        };

       var constantRef = builder.AddConstant(constantValue);
       consts[id] = constantRef;
    }
    private byte[] readString(string value) {
        var smatch = stringRegex.Match(value);
        var str = string.Empty;
        if (smatch.Success) {
            str = smatch.Groups["content"]?.Value ?? string.Empty;
        }
        if (string.IsNullOrEmpty(str))
            throw new FormatException("Invalid string format");
        var bytes = System.Text.Encoding.UTF8.GetBytes(str);
        return bytes;
    }
    private static Regex floatPattern = new Regex(@"(\+|\-)?\d+(?:\.\d+f?|f)");
    private static Regex uintPattern = new Regex(@"\d+(u|U)");
    private static Regex intPattern = new Regex(@"(\+|\-)?\d+");
    private static Regex memoryAnchorPattern = new Regex("\\$\\[a-zA-Z]+");
    private byte[] readNumber(string value) {
        var fmatch = floatPattern.Match(value);
        if (fmatch.Success) {
            var bytes = BitConverter.GetBytes(float.Parse(fmatch.Value.Replace("f", string.Empty)));
            if (!BitConverter.IsLittleEndian)
                Array.Reverse(bytes);
            return bytes;
        }
        var umatch = uintPattern.Match(value);
        if (umatch.Success) {
            var bytes = BitConverter.GetBytes(uint.Parse(umatch.Value.Replace("u", string.Empty).Replace("U", string.Empty)));
            if (!BitConverter.IsLittleEndian)
                Array.Reverse(bytes);
            return bytes;
        }
        var imatch = intPattern.Match(value);
        if (imatch.Success) {
            var bytes = BitConverter.GetBytes(int.Parse(imatch.Value));
            if (!BitConverter.IsLittleEndian)
                Array.Reverse(bytes);
            return bytes;
        }

        throw new FormatException("Invalid numeric format");
    }

    private static Regex labelPattern = new Regex(@"^\s*\.(?<id>\w+)\s*$");
    private void handleLabel(ModuleBuilder builder, Dictionary<string, Label> labels, List<LabelThunk> label_thunks, Dictionary<string, MemoryRef> consts, string line) {
        var match = labelPattern.Match(line);
        if (!match.Success) {
            throw new FormatException("Invalid label format. Labels should be a '.' followed by any number of the following digits [a-zA-Z0-9_].");
        }
        var id = match.Groups["id"].Value;
        var label = builder.Label(id);
        labels[id] = label;

        // See if any instructions are awaiting having this label determined
        label_thunks.RemoveAll((thunk) => thunk.ComputeIfPossible(labels));
    }

    private static Regex macroPattern = new Regex(@"^\s*\!(?<id>\w+)\s*(?<args>.*)");
    private void handleMacro(ModuleBuilder builder, Dictionary<string, Label> labels, Dictionary<string, MemoryRef> consts, string line) { 
        var match = macroPattern.Match(line);
        if (!match.Success) {
            throw new FormatException("Invalid macro format. Macro names should be a '!' followed by any number of the following digits [a-zA-Z0-9_].");
        }
        var id = match.Groups["id"].Value;
        var args = match.Groups["args"].Value.Trim();

        // Find macro
        var macro = typeof(ModuleBuilder)
        .GetMethods()
        .Where(method => Attribute.IsDefined(method, typeof(MacroAttribute)) && method.GetCustomAttribute<MacroAttribute>()?.Name == id)
        .FirstOrDefault();
        if (macro == null)
            throw new MissingMethodException($"Macro '{id}' is not defined.");
        
        // Parse arguments
        object[] argValues = readMacroArgs(labels, consts, args);

        // Invoke
        macro.Invoke(builder, argValues);
    }

    private static Regex labelNameRegex = new Regex(@"^\.(?<id>\w+)");
    private static Regex constantNameRegex = new Regex(@"^\@(?<id>\w+)");
    private static Regex stringRegex = new Regex(@"^((?<![\\])[""])(?<content>(?:.(?!(?<![\\])\1))*.?)\1");
    private object[] readMacroArgs(Dictionary<string, Label> labels, Dictionary<string, MemoryRef> consts, string argList) {
        if (string.IsNullOrEmpty(argList))
            return new object[0];

        var args = argList.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        var parsed = new object[args.Length];
        for (var i = 0; i < args.Length; i++) {
            var arg = args[i];

            var fmatch = floatPattern.Match(arg);
            if (fmatch.Success) {
                parsed[i] = (float.Parse(fmatch.Value));
                continue;
            }
            var umatch = uintPattern.Match(arg);
            if (umatch.Success) {
                parsed[i] = (uint.Parse(umatch.Value));
                continue;
            }
            var imatch = intPattern.Match(arg);
            if (imatch.Success) {
                parsed[i] = (int.Parse(imatch.Value));
                continue;
            }
            
            var smatch = stringRegex.Match(arg);
            if (smatch.Success) {
                parsed[i] = smatch.Groups["content"]?.Value ?? string.Empty;
                continue;
            }

            var lmatch = labelNameRegex.Match(arg);
            if (lmatch.Success) {
                var constId = lmatch.Groups["id"].Value;
                if (!labels.ContainsKey(constId))
                    throw new MissingMemberException($"No label defined with name '{constId}'.");
                parsed[i] = labels[constId].CodePosition;
                continue;
            }
            
            var cmatch = constantNameRegex.Match(arg);
            if (cmatch.Success) {
                var constId = cmatch.Groups["id"].Value;
                if (!consts.ContainsKey(constId))
                    throw new MissingMemberException($"No constant defined with name '{constId}'.");
                parsed[i] = consts[constId];
                continue;
            }

            throw new FormatException($"Unknown argument type '{arg}'.");
        }
        return parsed;
    }

    private static Regex instrPattern = new Regex(@"^\s*(?<id>\w+)\s*(?<args>.*)");
    private void handleInstruction(ModuleBuilder builder, Dictionary<string, Label> labels, List<LabelThunk> label_thunks, Dictionary<string, MemoryRef> consts, string line) { 
        var match = instrPattern.Match(line);
        if (!match.Success) {
            throw new FormatException("Invalid instruction format. Instruction names should be any number of the following digits [a-zA-Z0-9_]. Arguments should follow instruction names separated by spaces.");
        }
        var id = match.Groups["id"].Value;
        var args = match.Groups["args"].Value.Trim();

        List<string> labels_requiring_thunk;
        var values = readInitialInstrArgs(labels, consts, args, out labels_requiring_thunk);

        var before = builder.Anchor();
        builder.AddInstruction(id, values); // Push empty values
        var after = builder.Anchor();
        
        if (labels_requiring_thunk.Any()) {
            label_thunks.Add(new LabelThunk(labels_requiring_thunk, (labels) => {
                var now = builder.Anchor();
                builder.RewindStream(before);
                values = readFinalInstrArgs(after, builder, labels, consts, args);
                builder.AddInstruction(id, values); // Push actual values
                builder.RewindStream(now);
            }));
        } else {
            builder.RewindStream(before);
            values = readFinalInstrArgs(after, builder, labels, consts, args);
            builder.AddInstruction(id, values); // Push actual values
        }
    }

    private VmValue[] readInitialInstrArgs(Dictionary<string, Label> labels, Dictionary<string, MemoryRef> consts, string argList, out List<string> requires_thunk) {
        requires_thunk = new List<string>();
        if (string.IsNullOrEmpty(argList))
            return new VmValue[0];

        var args = argList.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        var parsed = new VmValue[args.Length];
        for (var i = 0; i < args.Length; i++) {
            var arg = args[i];

            var memoryMatch = memoryAnchorPattern.Match(arg);
            if (memoryMatch.Success) {
                parsed[i] = Operand.From(0);
                continue;
            }

            var fmatch = floatPattern.Match(arg);
            if (fmatch.Success) {
                parsed[i] = Operand.From(0.0f);
                continue;
            }
            var umatch = uintPattern.Match(arg);
            if (umatch.Success) {
                parsed[i] = Operand.From(0U);
                continue;
            }
            var imatch = intPattern.Match(arg);
            if (imatch.Success) {
                parsed[i] = Operand.From(0);
                continue;
            }

            var smatch = stringRegex.Match(arg);
            if (smatch.Success) {
                parsed[i] = Operand.From(0);
                continue;
            }

            var lmatch = labelNameRegex.Match(arg);
            if (lmatch.Success) {
                // goto .my_func
                var constId = lmatch.Groups["id"].Value;
                if (!labels.ContainsKey(constId))
                    requires_thunk.Add(constId);
                parsed[i] = Operand.From(0);
                continue;
            }
            var cmatch = constantNameRegex.Match(arg);
            if (cmatch.Success) {
                parsed[i] = Operand.From(0);
                continue;
            }

            throw new FormatException("Unknown argument type");
        }
        return parsed;
    }
    private VmValue[] readFinalInstrArgs(long positionAfterInstruction, ModuleBuilder builder, Dictionary<string, Label> labels, Dictionary<string, MemoryRef> consts, string argList) {
        if (string.IsNullOrEmpty(argList))
            return new VmValue[0];

        var args = argList.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        var parsed = new VmValue[args.Length];
        for (var i = 0; i < args.Length; i++) {
            var arg = args[i];

            var memoryMatch = memoryAnchorPattern.Match(arg);
            if (memoryMatch.Success) {
                var anchor = memoryMatch.Value.Substring(1);
                var index = anchor switch {
                    "const" => builder.ConstantPoolIndex,
                    "static" => builder.StaticPoolIndex,
                    _ => 0
                };
                parsed[i] =  Operand.From(index);
                continue;
            }

            var fmatch = floatPattern.Match(arg);
            if (fmatch.Success) {
                parsed[i] =  Operand.From(float.Parse(fmatch.Value.Replace("f", string.Empty)));
                continue;
            }
            var umatch = uintPattern.Match(arg);
            if (umatch.Success) {
                parsed[i] =  Operand.From(uint.Parse(umatch.Value.Replace("u", string.Empty).Replace("U", string.Empty)));
                continue;
            }
            var imatch = intPattern.Match(arg);
            if (imatch.Success) {
                parsed[i] = Operand.From(int.Parse(imatch.Value));
                continue;
            }

            var smatch = stringRegex.Match(arg);
            if (smatch.Success) {
                var name = smatch.Groups["content"]?.Value ?? string.Empty;
                int index;
                if (builder.HasImportedSubprogram(name, out index)) {
                    parsed[i] = Operand.From(index);
                } else {
                    throw new MissingMemberException($"No import defined with name '{name}'.");
                }
                continue;
            }

            var lmatch = labelNameRegex.Match(arg);
            if (lmatch.Success) {
                // goto .my_func
                var constId = lmatch.Groups["id"].Value;
                if (!labels.ContainsKey(constId))
                    throw new MissingMemberException($"No label defined with name '{constId}'.");
                parsed[i] = Operand.From((int)(labels[constId].CodePosition - positionAfterInstruction));
                continue;
            }
            var cmatch = constantNameRegex.Match(arg);
            if (cmatch.Success) {
                var constId = cmatch.Groups["id"].Value;
                if (!consts.ContainsKey(constId))
                    throw new MissingMemberException($"No constant defined with name '{constId}'.");
                parsed[i] = Operand.From(consts[constId].Offset);
                continue;
            }

            throw new FormatException("Unknown argument type");
        }
        return parsed;
    }
}


class LabelThunk {
    public IEnumerable<string> AwaitingLabels {get; private set;}
    private Action<Dictionary<string, Label>> computation;

    public LabelThunk(IEnumerable<string> awaiting, Action<Dictionary<string, Label>> computation) {
        this.AwaitingLabels = awaiting;
        this.computation = computation;
    }

    public bool ComputeIfPossible(Dictionary<string, Label> labels) {
        if (!AwaitingLabels.Except(labels.Keys).Any()) {
            computation(labels);
            return true;
        } else {
            return false;
        }
    }
}