using Qkmaxware.Vm.Instructions;

namespace Qkmaxware.Vm;

/// <summary>
/// Link 2 modules together
/// </summary>
public class Linker {
    /// <summary>
    /// Link a primary module to an auxillary one
    /// </summary>
    /// <param name="primary">primary module</param>
    /// <param name="auxillary">auxillary module</param>
    public Module Link(Module primary, Module auxillary) {
        Module combined = new Module();
        var codeOffset = primary.CodeLength;

        // TODO imports and exports
        // See if primary needs anything auxillary provides, or if primary provides things auxillary needs
        var primary_mapping = new Dictionary<Import, Export>();
        var primary_unmapped = new List<Import>();
        foreach (var import in primary.Imports) {
            var export = auxillary.Exports.Where(export => export.Name == import.Name).FirstOrDefault();
            if (export != null) {
                primary_mapping[import] = export;
            } else {
                primary_unmapped.Add(import);
            }
        }
        var aux_mapping = new Dictionary<Import, Export>();
        var aux_unmapped = new List<Import>();
        foreach (var import in auxillary.Imports) {
            var export = primary.Exports.Where(export => export.Name == import.Name).FirstOrDefault();
            if (export != null) {
                aux_mapping[import] = export;
            } else {
                aux_unmapped.Add(import);
            }
        }
        combined.Imports.AddRange(primary_unmapped);
        combined.Imports.AddRange(aux_unmapped);
        combined.Exports.AddRange(primary.Exports);
        combined.Exports.AddRange(auxillary.Exports.Select(export => new Export(export.Name, codeOffset + export.CodePosition)));

        // Join constant pool
        combined.ConstantPool.AddRange(primary.ConstantPool);
        var constantPoolOffset = combined.ConstantPoolCount;
        combined.ConstantPool.AddRange(auxillary.ConstantPool);

        // Join static pool
        combined.StaticPool.AddRange(primary.StaticPool);
        var staticPoolOffset = combined.StaticPool.Count;
        combined.StaticPool.AddRange(auxillary.StaticPool);

        // Join code
        var dis = new Disassembler();
        using var builder = new ModuleBuilder();
        foreach (var instr in dis.DisassembleCode(primary)) {
            if (instr.Instruction is CallExternal) {
                // See if we can convert this to a pure call
                var index = ((Operand)instr.Arguments[0]);
                var args = ((Operand)instr.Arguments[1]);
                var import = primary.Imports[index.Int32];
                if (primary_mapping.ContainsKey(import)) {
                    // Convert to a pure call
                    var export = primary_mapping[import];
                    var abs_position = codeOffset + export.CodePosition;
                    var pc = (builder.Anchor() + 9);
                    var rel_position = abs_position - pc;
                    //Console.WriteLine("Rewriting call from auxillary@" + export.CodePosition + " to linked@" + abs_position + " an offset from 0x" + pc.ToString("X") + " by " + rel_position);
                    builder.AddInstruction(new Call(), new VmValue[] { Operand.From((int)rel_position), args });
                } else {
                    // Leave as an external call, update index to new value
                    builder.AddInstruction(instr.Instruction, new VmValue[]{ Operand.From(primary.Imports.IndexOf(import)), args });
                }
            } else {
                // Encode as is
                builder.AddInstruction(instr.Instruction, instr.Arguments.ToArray());
            }
        }
        foreach (var instr in dis.DisassembleCode(auxillary)) {
            if (instr.Instruction is LoadConst) {
                // Re-write the index
                var new_index = Operand.From(((Operand)instr.Arguments.ElementAt(0)).Int32 + constantPoolOffset);
                builder.AddInstruction(instr.Instruction, new VmValue[]{ new_index });
            } 
            else if (instr.Instruction is LoadStatic || instr.Instruction is StoreStatic) {
                // Re-write the index
                var new_index = Operand.From(((Operand)instr.Arguments.ElementAt(0)).Int32 + staticPoolOffset);
                builder.AddInstruction(instr.Instruction, new VmValue[]{ new_index });
            }
            else if (instr.Instruction is CallExternal) {
                // See if we can convert this to a pure call
                var index = ((Operand)instr.Arguments[0]);
                var args = ((Operand)instr.Arguments[1]);
                var import = auxillary.Imports[index.Int32];
                if (aux_mapping.ContainsKey(import)) {
                    // Convert to a pure call
                    var export = aux_mapping[import];
                    var abs_position = export.CodePosition;
                    var pc = (builder.Anchor() + 9);
                    var rel_position = abs_position - pc;
                    builder.AddInstruction(new Call(), new VmValue[] { Operand.From((int)rel_position), args });
                } else {
                    // Leave as an external call, update index to new value
                    builder.AddInstruction(instr.Instruction, new VmValue[]{ Operand.From(auxillary.Imports.IndexOf(import)), args });
                }
            } else {
                // Encode as is
                builder.AddInstruction(instr.Instruction, instr.Arguments.ToArray());
            }
        }
        combined.Code.AddRange(builder.ToModule().Code);

        return combined;
    }
}