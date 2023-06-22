using System;
using System.IO;
using CommandLine;

namespace Qkmaxware.Vm.Terminal.Commands;

// qkvm describe -f /path/to/file.qkbc

[Verb("describe", HelpText = "Describe a bytecode module")]
public class Describe : BaseCommand {
    [Option('f', "file", HelpText = "Path to bytecode file")]
    public string? FileName {get; set;}

    public override void Execute() {
        this.FileName = VerifyFile(this.FileName);

        int major;
        int minor;
        AssertBytecodeFile(this.FileName, out major, out minor);
        Console.WriteLine("Qkmaxware Bytecode Module");
        Console.WriteLine($"version {major}.{minor}");
        Console.WriteLine();

        ModuleLoader loader = new ModuleLoader();
        using var reader = new BinaryReader(File.OpenRead(this.FileName));
        var module = loader.FromStream(reader);  

        var dis = new Disassembler();
        Console.WriteLine($"Code ({module.CodeLength} bytes):");
        foreach (var line in dis.DisassembleCode(module)) {
            Console.Write("    0x");
            Console.Write($"{line.MemoryOffset:X}".PadRight(6));
            Console.Write(' ');
            Console.Write(line.Instruction.Name);
            foreach (var arg in line.Arguments) {
                Console.Write(' ');
                Console.Write(arg);
            }
            Console.WriteLine();
        }

        // TODO also show constants
        Console.WriteLine();
        Console.WriteLine($"Constant Pool ({module.ConstantPoolCount} value):");
        Console.Write("    ");
        Console.Write($"Index".PadRight(6));
        Console.Write(' ');
        Console.Write("| Type".PadRight(32));
        Console.WriteLine("| Value");
        Console.WriteLine(new string('-', 64));
        foreach (var cons in module.ConstantPool) {
            Console.Write("      ");
            Console.Write($"{cons.PoolIndex}".PadRight(6));
            Console.Write(' ');
            Console.Write(cons.GetType().Name.PadRight(32));
            Console.WriteLine(cons.ValueToString());
        }
    }
}