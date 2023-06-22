using System;
using System.IO;
using CommandLine;

namespace Qkmaxware.Vm.Terminal.Commands;

[Verb("docs", HelpText = "View bytecode documentation")]
public class Docs : BaseCommand {
    [Option('f', "file", HelpText = "Path to bytecode file")]
    public string? FileName {get; set;}

    public override void Execute() {
        var tab = "    ";
        Console.WriteLine("# Bytecode Instruction Set");
        foreach (var instr in InstructionMap.Instance) {
            Console.WriteLine("    --------------------------------");
            Console.Write(tab); Console.WriteLine(instr.Name);
            Console.WriteLine("    --------------------------------");
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