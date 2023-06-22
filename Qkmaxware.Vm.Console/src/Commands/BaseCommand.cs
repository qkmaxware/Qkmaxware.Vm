using System;
using System.IO;
using CommandLine;

namespace Qkmaxware.Vm.Terminal.Commands;

[Verb("assemble", HelpText = "Assemble a textual assembly file into bytecode")]
public abstract class BaseCommand {
    
    public abstract void Execute();

    protected string VerifyFile(string? FileName) {
        if (string.IsNullOrEmpty(FileName)) {
            throw new FileNotFoundException("Missing file argument.");
        }

        if (!File.Exists(FileName)) {
            throw new FileNotFoundException($"File '{FileName}' does not exist.");
        }

        return FileName;
    }

    protected void AssertBytecodeFile(string FileName) {
        int ma; int mi;
        AssertBytecodeFile(FileName, out ma, out mi);
    }
    protected void AssertBytecodeFile(string FileName, out int major_version, out int minor_version) {
        using (var header_reader = new BinaryReader(File.OpenRead(FileName))) {
            foreach (var magic_byte in Module.MagicNumbers) {
                if (header_reader.ReadByte() != magic_byte) {
                    throw new ArgumentException("File is not a Qkmaxware Bytecode Module.");
                }
            }
            major_version = header_reader.ReadInt32();
            minor_version = header_reader.ReadInt32();
        }
    }

    protected void AssertAsmFile(string FileName) {
        string d; int ma; int mi;
        AssertAsmFile(FileName, out d, out ma, out mi);
    }
    protected void AssertAsmFile(string FileName, out string dialect, out int major_version, out int minor_version) {
        using (var header_reader = new StreamReader(File.OpenRead(FileName))) {
            Assembly.Assembler.IsContentAssembly(header_reader, out dialect, out major_version, out minor_version);
        }
    }

    protected string NameOf(string FileName) {
        return Path.GetFileNameWithoutExtension(FileName);
    }

    protected Stream CreateFileInSameDirectoryAs(string siblingFile, string desiredFilename) {
        var dir = Path.GetDirectoryName(siblingFile);
        var path = Path.Combine(dir ?? string.Empty, desiredFilename);
        return File.OpenWrite(path);
    }

    public StatusCode TryExecute() {
        try {
            Execute();
            return StatusCode.Ok;
        } catch (Exception e) {
            Console.WriteLine(e);
            return StatusCode.Err;
        }
    }

}