using System;
using System.IO;
using CommandLine;

namespace Qkmaxware.Vm.Terminal.Commands;

public class InstructionSetDocument : GeneratedDocument {
    public override string Name() => "Bytecode Instruction Set";
    public override void WriteOut(TextWriter writer) {
        var tab = "  ";
        writer.WriteLine("# Bytecode Instruction Set");
        foreach (var instr in InstructionMap.Instance) {
            writer.WriteLine(tab + "--------------------------------");
            writer.Write(tab); writer.Write($"0x{instr.Opcode:X2} "); writer.WriteLine(instr.Name);
            writer.WriteLine(tab + "--------------------------------");
            writer.Write(tab); writer.WriteLine("description:");
            writer.Write(tab); writer.Write(tab); writer.WriteLine(instr.Description);
            writer.Write(tab); writer.WriteLine("format:");
            writer.Write(tab); writer.Write(tab);
            writer.Write(instr.Name);
            foreach (var arg in instr.Arguments) {
                writer.Write(' ');
                writer.Write(arg.GetType().Name);
                writer.Write('(');
                writer.Write(arg.Name);
                writer.Write(")");
            }
            writer.WriteLine();
            writer.Write(tab); writer.WriteLine("stack:");
            writer.Write(tab); writer.Write(tab); 
                writer.Write('['); writer.Write(string.Join(',', instr.StackInputs)); writer.Write("] -> ["); writer.Write(string.Join(',', instr.StackOutputs)); writer.Write(']');
            writer.WriteLine();
            writer.WriteLine();
        }
    }
}