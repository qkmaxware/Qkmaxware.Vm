using System;
using System.IO;
using System.Reflection;
using CommandLine;

namespace Qkmaxware.Vm.Terminal.Commands;

public class MacroListingDocument : GeneratedDocument {
    public override string Name() => "Macro Listing";
    public override void WriteOut(TextWriter writer) {
        var tab = "  ";
        writer.WriteLine("# Assembly Macro Listing");
        var macros = typeof(ModuleBuilder)
        .GetMethods()
        .Where(method => Attribute.IsDefined(method, typeof(MacroAttribute)));
        foreach (var method in macros) {
            var macro = method.GetCustomAttribute<MacroAttribute>();
            writer.WriteLine(tab + "--------------------------------");
            writer.Write(tab); writer.WriteLine(macro?.Name);
            writer.WriteLine(tab + "--------------------------------");
            writer.Write(tab); writer.WriteLine("description:");
            writer.Write(tab); writer.Write(tab); writer.WriteLine(macro?.Description);
            writer.Write(tab); writer.WriteLine("format:");
            writer.Write(tab); writer.Write(tab);
            writer.Write('!');
            writer.Write(macro?.Name);
            foreach (var arg in method.GetParameters()) {
                writer.Write(' ');
                writer.Write(arg.ParameterType);
                writer.Write('(');
                writer.Write(arg.Name);
                writer.Write(")");
            }
            writer.WriteLine();
            writer.WriteLine();
        }
    }
}