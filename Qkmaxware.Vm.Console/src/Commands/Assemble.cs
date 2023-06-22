using System;
using System.IO;
using CommandLine;

namespace Qkmaxware.Vm.Terminal.Commands;

[Verb("assemble", HelpText = "Assemble a textual assembly file into bytecode")]
public class Assemble : BaseCommand {
    [Option('f', "file", HelpText = "Path to bytecode file", Required = true)]
    public string? FileName {get; set;}

    [Option('o', "out", HelpText = "Path to bytecode file")]
    public string? OutputFileName {get; set;}

    public override void Execute() {
        this.FileName = VerifyFile(FileName);
        AssertAsmFile(this.FileName);

        // Read it
        var asm = new Assembly.Assembler();
        using var reader = new StreamReader(FileName);
        var module = asm.FromStream(reader);

        // Write it
        var stream = 
            string.IsNullOrEmpty(this.OutputFileName) 
            ? CreateFileInSameDirectoryAs(this.FileName, NameOf(this.FileName) + $"-{Module.MajorVersion}.{Module.MinorVersion}.qkbc") 
            : (Stream)File.Open(this.OutputFileName, FileMode.Open);
        using (var writer = new BinaryWriter(stream)) {
            module.EncodeFile(writer);
        }

        var name = string.IsNullOrEmpty(this.OutputFileName) ? NameOf(FileName) + $"-{Module.MajorVersion}.{Module.MinorVersion}.qkbc": this.OutputFileName;
        Console.WriteLine("File created '" + name);
    }
}