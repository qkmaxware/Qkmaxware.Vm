using System;
using System.IO;
using CommandLine;

namespace Qkmaxware.Vm.Terminal.Commands;

public class InstructionSetDocument : GeneratedDocument {
    public override string Name() => "Bytecode Instruction Set";
    public override void WriteOut() {
        var tab = "  ";
        Console.WriteLine("# Bytecode Instruction Set");
        foreach (var instr in InstructionMap.Instance) {
            Console.WriteLine(tab + "--------------------------------");
            Console.Write(tab); Console.Write($"0x{instr.Opcode:X2} "); Console.WriteLine(instr.Name);
            Console.WriteLine(tab + "--------------------------------");
            Console.Write(tab); Console.WriteLine("description:");
            Console.Write(tab); Console.Write(tab); Console.WriteLine(instr.Description);
            Console.Write(tab); Console.WriteLine("format:");
            Console.Write(tab); Console.Write(tab);
            Console.Write(instr.Name);
            foreach (var arg in instr.Arguments) {
                Console.Write(' ');
                Console.Write(arg.GetType().Name);
                Console.Write('(');
                Console.Write(arg.Name);
                Console.Write(")");
            }
            Console.WriteLine();
            Console.WriteLine();
        }
    }
}