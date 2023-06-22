using System;
using System.IO;
using CommandLine;

namespace Qkmaxware.Vm.Terminal.Commands;

[Verb("assemble", HelpText = "Assemble a textual assembly file into bytecode")]
public class Assemble : BaseCommand {
    [Option('f', "file", HelpText = "Path to bytecode file")]
    public string? FileName {get; set;}

    public override void Execute() {
        this.FileName = VerifyFile(FileName);
        AssertAsmFile(this.FileName);

        // Read it
        var asm = new Assembly.Assembler();
        using var reader = new StreamReader(FileName);
        var module = asm.FromStream(reader);

        // Write it
        using (var writer = new BinaryWriter(CreateFileInSameDirectoryAs(this.FileName, NameOf(FileName)+".qkbc"))) {
            module.EncodeFile(writer);
        }

        Console.WriteLine("File created '" + NameOf(FileName)+".qkbc'");
    }
}