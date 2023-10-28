using System;
using System.IO;
using CommandLine;
using CommandLine.Text;

namespace Qkmaxware.Vm.Terminal.Commands;

[Verb("docs", HelpText = "View bytecode documentation")]
class DocHelper {
    [Option("topic", HelpText = "Specific topic to view documentation about. Leave blank to list all topics.")]
    public string? Topic {get; set;} 
}

[Verb("docs", HelpText = "View bytecode documentation")]
public class Docs : BaseCommand {

    [Option("topic", HelpText = "Specific topic to view documentation about. Leave blank to list all topics.")]
    public string? Topic {get; private set;} 

    [Usage(ApplicationAlias = "qkvm")]
    public static IEnumerable<Example> Examples {
        get {
            foreach (var doc in docs) {
                yield return new Example("topic '" + doc.Name() + "'", new DocHelper { Topic = doc.Name() });
            }
        }
    }

    private static IEnumerable<Document> docs {get; set;}

    static Docs() {
        docs = getEmbeddedDocFiles()
        .Append(new MacroListingDocument())
        .Append(new InstructionSetDocument())
        .ToList();
    }

    private static IEnumerable<Document> getEmbeddedDocFiles() {
        var assembly = typeof(Docs).Assembly;
        var prefix = "Qkmaxware.Vm.Console.docs.";
        foreach (var name in assembly.GetManifestResourceNames()) {
            if (name.StartsWith(prefix)) {
                yield return new EmbeddedDocument(assembly, name, Path.GetFileNameWithoutExtension(name.Remove(0, prefix.Length)));
            }
        }
    }

    public override void Execute() {
        if (string.IsNullOrEmpty(Topic)) {
            printHelpText();
            Console.WriteLine($"Missing topic, use --help for a list of topics.");
        } else {
            var small_topic = Topic.ToLower();
            var selected = docs.Where(doc => doc.Name().ToLower().Contains(small_topic)).FirstOrDefault();
            if (selected == null) {
                printHelpText();
                Console.WriteLine($"Unknown topic '{Topic}'.");
            } else {
                Console.WriteLine(CommandLine.Text.HeadingInfo.Default);
                Console.WriteLine(CommandLine.Text.CopyrightInfo.Default);
                Console.WriteLine();
                selected.WriteOut(Console.Out);
            }
        }
    }

    private void printHelpText() {
        Console.WriteLine(CommandLine.Text.HeadingInfo.Default);
        Console.WriteLine(CommandLine.Text.CopyrightInfo.Default);
        Console.WriteLine();
        //var results = Parser.Default.ParseArguments<Docs>(Parser.Default.FormatCommandLineArgs(this));
        //Console.WriteLine(HelpText.AutoBuild<Docs>(results));
    }
}