using System.Reflection;
using System.Text.RegularExpressions;

namespace Qkmaxware.Vm.Assembly;

/// <summary>
/// Parser for "asm 1.x" assembly files
/// </summary>
public class Asm1xParser : IAssemblyParser {
    public bool SupportsVersion(string dialect, int Major, int Minor) => dialect == "asm" && Major == 1;

    private static Regex commentPattern = new Regex(@"\/\/(.*)"); //

    public Module Parse(TextReader reader) {
        using var builder = new ModuleBuilder();
        Dictionary<string, Label> labels = new Dictionary<string, Label>();
        Dictionary<string, ConstantRef> constants = new Dictionary<string, ConstantRef>();

        string? line = null;
        long lineIndex = 0;
        while ((line = reader.ReadLine()) != null) {
            try {
                // Ignore empty lines
                line = commentPattern.Replace(line, string.Empty).Trim();
                if (string.IsNullOrEmpty(line))
                    continue;

                // Handle different line types
                if (line.StartsWith('@')) {
                    handleConstant(builder, labels, constants, line);
                }
                else if (line.StartsWith('.')) {
                    handleLabel(builder, labels, constants, line);
                }
                else if (line.StartsWith("!")) {
                    handleMacro(builder, labels, constants, line);
                }
                else {
                    handleInstruction(builder, labels, constants, line);
                }

                // Increment line index
                lineIndex ++;
            } catch (Exception e) {
                throw new AssemblyException(lineIndex, e);
            }
        }

        return builder.ToModule();
    }

    private static Regex constantPattern = new Regex(@"^\s*\@(?<id>\w+)\s*=\s*(?<value>.+)");
    private void handleConstant(ModuleBuilder builder, Dictionary<string, Label> labels, Dictionary<string, ConstantRef> consts, string line) {
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
    private ConstantData readString(string value) {
        var str = System.Text.Json.JsonSerializer.Deserialize<string>(value);
        if (str == null)
            throw new FormatException("Invalid string format");
        return new StringConstant(ConstantInfo.Utf8, str);
    }
    private static Regex floatPattern = new Regex(@"(\+|\-)?\d+(?:\.\d+f?|f)");
    private static Regex uintPattern = new Regex(@"\d+(u|U)");
    private static Regex intPattern = new Regex(@"(\+|\-)?\d+");
    private ConstantData readNumber(string value) {
        var fmatch = floatPattern.Match(value);
        if (fmatch.Success) {
            return new Float32Constant(float.Parse(fmatch.Value.Replace("f", string.Empty)));
        }
        var umatch = uintPattern.Match(value);
        if (umatch.Success) {
            return new UInt32Constant(uint.Parse(umatch.Value.Replace("u", string.Empty).Replace("U", string.Empty)));
        }
        var imatch = intPattern.Match(value);
        if (imatch.Success) {
            return new Int32Constant(int.Parse(imatch.Value));
        }

        throw new FormatException("Invalid numeric format");
    }

    private static Regex labelPattern = new Regex(@"^\s*\.(?<id>\w+)\s*$");
    private void handleLabel(ModuleBuilder builder, Dictionary<string, Label> labels, Dictionary<string, ConstantRef> consts, string line) {
        var match = labelPattern.Match(line);
        if (!match.Success) {
            throw new FormatException("Invalid label format. Labels should be a '.' followed by any number of the following digits [a-zA-Z0-9_].");
        }
        var id = match.Groups["id"].Value;
        var label = builder.Label(id);
        labels[id] = label;
    }

    private static Regex macroPattern = new Regex(@"^\s*\!(?<id>\w+)\s*(?<args>.*)");
    private void handleMacro(ModuleBuilder builder, Dictionary<string, Label> labels, Dictionary<string, ConstantRef> consts, string line) { 
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
    private object[] readMacroArgs(Dictionary<string, Label> labels, Dictionary<string, ConstantRef> consts, string argList) {
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

            var lmatch = labelNameRegex.Match(arg);
            if (lmatch.Success) {
                var constId = lmatch.Groups["id"].Value;
                if (!labels.ContainsKey(constId))
                    throw new MissingMemberException($"No label defined with name '{constId}'.");
                parsed[i] = labels[constId].CodePosition;
                continue;
            }
            var cmatch = constantNameRegex.Match(arg);
            if (lmatch.Success) {
                var constId = cmatch.Groups["id"].Value;
                if (!consts.ContainsKey(constId))
                    throw new MissingMemberException($"No constant defined with name '{constId}'.");
                parsed[i] = consts[constId].PoolIndex;
                continue;
            }

            throw new FormatException("Unknown argument type");
        }
        return parsed;
    }

    private static Regex instrPattern = new Regex(@"^\s*(?<id>\w+)\s*(?<args>.*)");
    private void handleInstruction(ModuleBuilder builder, Dictionary<string, Label> labels, Dictionary<string, ConstantRef> consts, string line) { 
        var match = instrPattern.Match(line);
        if (!match.Success) {
            throw new FormatException("Invalid instruction format. Instruction names should be any number of the following digits [a-zA-Z0-9_]. Arguments should follow instruction names separated by spaces.");
        }
        var id = match.Groups["id"].Value;
        var args = match.Groups["args"].Value.Trim();

        var values = readInitialInstrArgs(labels, consts, args);

        var before = builder.Anchor();
        builder.AddInstruction(id, values); // Push empty values
        var after = builder.Anchor();
        builder.RewindStream(before);
        values = readFinalInstrArgs(after, labels, consts, args);
        builder.AddInstruction(id, values); // Push actual values
    }

    private VmValue[] readInitialInstrArgs(Dictionary<string, Label> labels, Dictionary<string, ConstantRef> consts, string argList) {
        if (string.IsNullOrEmpty(argList))
            return new VmValue[0];

        var args = argList.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        var parsed = new VmValue[args.Length];
        for (var i = 0; i < args.Length; i++) {
            var arg = args[i];

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

            var lmatch = labelNameRegex.Match(arg);
            if (lmatch.Success) {
                // goto .my_func
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
    private VmValue[] readFinalInstrArgs(long positionAfterInstruction, Dictionary<string, Label> labels, Dictionary<string, ConstantRef> consts, string argList) {
        if (string.IsNullOrEmpty(argList))
            return new VmValue[0];

        var args = argList.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        var parsed = new VmValue[args.Length];
        for (var i = 0; i < args.Length; i++) {
            var arg = args[i];

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
                parsed[i] = Operand.From(consts[constId].PoolIndex);
                continue;
            }

            throw new FormatException("Unknown argument type");
        }
        return parsed;
    }
}