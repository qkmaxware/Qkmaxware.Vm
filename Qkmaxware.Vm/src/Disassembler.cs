using System.Collections.ObjectModel;
using System.Text;

namespace Qkmaxware.Vm;

/// <summary>
/// Disassembly information for a given instruction
/// </summary>
public class DisassembledInstruction {
    public long MemoryOffset {get; private set;}
    public IInstruction Instruction {get; private set;}
    public ReadOnlyCollection<VmValue> Arguments {get; private set;}

    public DisassembledInstruction(long offset, IInstruction instr, IEnumerable<VmValue> args) {
        this.MemoryOffset = offset;
        this.Instruction = instr;
        this.Arguments = args.ToList().AsReadOnly();
    }

    public override string ToString() {
        StringBuilder sb = new StringBuilder();
        sb.Append($"0x{this.MemoryOffset:X}".PadRight(6));
        sb.Append(this.Instruction.Name);
        foreach (var arg in Arguments) {
            sb.Append(' ');
            sb.Append(arg);
        }
        return sb.ToString();
    }
}

/// <summary>
/// Disassembler able to convert bytecode back to human readable instructions
/// </summary>
public class Disassembler {

    /// <summary>
    /// Disassemble all instruction in the given module
    /// </summary>
    /// <param name="module">module to disassemble</param>
    /// <returns>list of instruction disassembly information</returns>
    public IEnumerable<DisassembledInstruction> DisassembleCode(Module module) {
        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream);
        foreach (var b in module.Code) {
            writer.Write(b);
        }
        stream.Position = 0;

        using (var reader = new BinaryReader(stream)) {
            // Loop while stream is not empty
            while (reader.BaseStream.Position != reader.BaseStream.Length) {
                // Fetch opcode
                var instr_number = reader.BaseStream.Position;
                var opcode = reader.ReadByte();

                // Fetch args
                var instr = InstructionMap.Instance[opcode];
                if (instr == null) {
                    throw new ArgumentException($"Unknown operation with opcode 'opcode' at position 0x{instr_number:x}");
                }
                List<VmValue> arg_values = new List<VmValue>(instr.Arity);
                foreach (var arg in instr.Arguments) {
                    arg_values.Add(arg.ReadValue(reader));
                }

                yield return new DisassembledInstruction(
                    offset: instr_number,
                    instr: instr,
                    args: arg_values
                );
            }
        }
    }

}