using System;
using System.IO;
using CommandLine;

namespace Qkmaxware.Vm.Terminal.Commands;

// qkvm describe -f /path/to/file.qkbc

[Verb("describe", HelpText = "Describe a bytecode module")]
public class Describe : BaseCommand {
    [Option('f', "file", HelpText = "Path to bytecode file", Required = true)]
    public string? FileName {get; set;}

    public override void Execute() {
        this.FileName = VerifyFile(this.FileName);

        int major;
        int minor;
        AssertBytecodeFile(this.FileName, out major, out minor);

        ModuleLoader loader = new ModuleLoader();
        var file = new FileInfo(this.FileName);
        using var reader = new BinaryReader(file.OpenRead());
        var module = loader.FromStream(reader);  

        Console.WriteLine("Qkmaxware Bytecode Module");
        Console.WriteLine($"version: {major}.{minor}");
        Console.WriteLine($"size: {file.Length} bytes");
        Console.WriteLine($"last modified: {file.LastWriteTime}");
        Console.WriteLine();
        Console.WriteLine();

        Console.WriteLine($"Exports ({module.ExportCount}):");
        foreach (var export in module.Exports) {
            Console.Write("    - ");
            Console.Write(export.Name);
            Console.Write(" @ 0x");
            Console.WriteLine(export.CodePosition.ToString("X"));
        }
        Console.WriteLine();

        Console.WriteLine($"Imports ({module.ImportCount}):");
        foreach (var import in module.Imports) {
            Console.Write("    - ");
            Console.WriteLine(import.Name);
        }
        Console.WriteLine();

        var dis = new Disassembler();
        Console.WriteLine($"Code ({module.CodeLength} bytes):");
        foreach (var line in dis.DisassembleCode(module)) {
            foreach (var export in module.Exports) {
                if (export.CodePosition == line.MemoryOffset) {
                    Console.WriteLine($"  {export.Name}:");
                }
            }
            Console.Write("    0x");
            Console.Write($"{line.MemoryOffset:X}".PadRight(6));
            Console.Write(' ');
            Console.Write(line.Instruction.Name);
            foreach (var arg in line.Arguments) {
                Console.Write(' ');
                Console.Write(arg.ValueToString());
            }
            Console.WriteLine();
        }

        Console.WriteLine();
        Console.WriteLine($"Memories ({module.MemoryCount}):");
        Console.Write("    ");
        Console.Write($"Index".PadRight(6));
        Console.Write(' ');
        Console.WriteLine("| Permissions | Size      | Data");
        var static_index = 0;
        foreach (var value in module.Memories) {
            Console.Write("      ");
            Console.Write($"{static_index++}".PadRight(6));
            Console.Write(' ');
            Console.Write(value.Mutability.ToString().PadRight(13));
            Console.Write(' ');
            Console.Write(value.Limits.ToString().PadRight(11));
            Console.Write(' ');
            Console.Write(System.Text.Encoding.ASCII.GetString(value.Initializer.ToArray()));
            Console.WriteLine();
        }
    }
}