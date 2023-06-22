using System;
using System.IO;
using CommandLine;

namespace Qkmaxware.Vm.Terminal.Commands;

[Verb("convert", HelpText = "Convert bytecode modules from older formats to the latest format")]
public class ConvertVersion : BaseCommand {
    [Option('f', "file", HelpText = "Path to bytecode file", Required = true)]
    public string? FileName {get; set;}

    [Option('o', "out", HelpText = "Output path to resulting bytecode file")]
    public string? OutputFileName {get; set;}

    public override void Execute() {
        this.FileName = VerifyFile(FileName);

        var major = 0;
        var minor = 0;
        AssertBytecodeFile(this.FileName, out major, out minor);

        if (major > Module.MajorVersion || (major == Module.MajorVersion && minor > Module.MinorVersion)) {
            throw new ArgumentException($"File '{FileName}' is in a newer version than provided by this utility.");
        }
        if (major == Module.MajorVersion && minor == Module.MinorVersion) {
            throw new ArgumentException($"File '{FileName}' is already in the latest format.");
        }

        ModuleLoader loader = new ModuleLoader();
        using var reader = new BinaryReader(File.OpenRead(this.FileName));
        var module = loader.FromStream(reader);

        // Create new file
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