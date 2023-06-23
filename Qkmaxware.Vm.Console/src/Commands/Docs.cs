using System;
using System.IO;
using CommandLine;

namespace Qkmaxware.Vm.Terminal.Commands;

[Verb("docs", HelpText = "View bytecode documentation")]
public class Docs : BaseCommand {

    [Option("topic", HelpText = "Specific topic to view documentation about. Leave blank to list all topics.")]
    public string? Topic {get; private set;} 

    private static IEnumerable<Document> docs = 
        getEmbeddedDocFiles()
        .Append(new MacroListingDocument())
        .Append(new InstructionSetDocument());

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
        Console.WriteLine(CommandLine.Text.HeadingInfo.Default);
        Console.WriteLine(CommandLine.Text.CopyrightInfo.Default);
        Console.WriteLine();

        if (string.IsNullOrEmpty(Topic)) {
            PrintTopics();
        } else {
            var small_topic = Topic.ToLower();
            var selected = docs.Where(doc => doc.Name().ToLower().Contains(small_topic)).FirstOrDefault();
            if (selected == null) {
                Console.WriteLine($"Unknown topic '{Topic}'");
                Console.WriteLine();
                PrintTopics();
            } else {
                selected.WriteOut();
            }
        }
    }

    private void PrintTopics() {
        Console.WriteLine("# Topics");
        foreach (var doc in docs) {
            Console.Write("  ");
            Console.WriteLine(doc.Name());
        }
    }
}