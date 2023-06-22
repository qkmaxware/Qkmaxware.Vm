using System;
using System.IO;
using CommandLine;

namespace Qkmaxware.Vm.Terminal.Commands;

[Verb("link", HelpText = "Link multiple bytecode files together into a single file")]
public class Link : BaseCommand {
    [Option('f', "file", HelpText = "Path to bytecode file", Required = true)]
    public string? FileName {get; set;}

    [Option('l', "link", HelpText = "Path to files to link with", Required = true, Separator = ',')]
    public IEnumerable<string>? LinkFilePaths {get; set;}

    [Option('o', "out", HelpText = "Output path to resulting bytecode file")]
    public string? OutputFileName {get; set;}

    private Module load(string? path) {
        path = VerifyFile(FileName);
        AssertBytecodeFile(path);

        var loader = new ModuleLoader();
        using var reader = new BinaryReader(File.OpenRead(path));
        return loader.FromStream(reader);
    }

    public override void Execute() {
        // Load modules
        this.FileName = VerifyFile(FileName);
        var primary = load(this.FileName);

        var linkers = this.LinkFilePaths?.Select(path => load(path))?.ToArray() ?? new Module[0];
        if (linkers.Length == 0) {
            throw new ArgumentException("No modules provided for linking.");
        }

        // Link modules
        var linker = new Linker();
        for (var i = 0; i < linkers.Length; i++) {
            primary = linker.Link(primary, linkers[i]);
        }

        // Write it
        var stream = 
            string.IsNullOrEmpty(this.OutputFileName) 
            ? CreateFileInSameDirectoryAs(this.FileName, NameOf(this.FileName) + $"-{Module.MajorVersion}.{Module.MinorVersion}.qkbc") 
            : (Stream)File.Open(this.OutputFileName, FileMode.Open);
        using (var writer = new BinaryWriter(stream)) {
            primary.EncodeFile(writer);
        }

        var name = string.IsNullOrEmpty(this.OutputFileName) ? NameOf(FileName) + $"-{Module.MajorVersion}.{Module.MinorVersion}.qkbc": this.OutputFileName;
        Console.WriteLine("File created '" + name);
    }
}