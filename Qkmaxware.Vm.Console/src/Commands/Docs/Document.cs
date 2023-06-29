using System;
using System.IO;
using CommandLine;

namespace Qkmaxware.Vm.Terminal.Commands;

public abstract class Document {
    public abstract string Name();
    public abstract void WriteOut(TextWriter writer);
}


public class EmbeddedDocument : Document {
    private System.Reflection.Assembly assembly;
    private string resource;
    private string name;

    public override string Name() => name;

    public EmbeddedDocument(System.Reflection.Assembly assembly, string resource, string name) {
        this.assembly = assembly;
        this.resource = resource;
        this.name = name;
    }

    public override void WriteOut(TextWriter writer) {
        Stream? stream = assembly.GetManifestResourceStream(resource);
        if (stream != null) {
            using (var reader = new StreamReader(stream)) {
                writer.WriteLine(reader.ReadToEnd());
            }
        }
    }
}

public abstract class GeneratedDocument : Document {
    
}