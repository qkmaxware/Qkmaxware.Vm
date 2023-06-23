using System;
using System.IO;
using System.Reflection;
using CommandLine;

namespace Qkmaxware.Vm.Terminal.Commands;

public class MacroListingDocument : GeneratedDocument {
    public override string Name() => "Macro Listing";
    public override void WriteOut() {
        var tab = "  ";
        Console.WriteLine("# Assembly Macro Listing");
        var macros = typeof(ModuleBuilder)
        .GetMethods()
        .Where(method => Attribute.IsDefined(method, typeof(MacroAttribute)));
        foreach (var method in macros) {
            var macro = method.GetCustomAttribute<MacroAttribute>();
            Console.WriteLine(tab + "--------------------------------");
            Console.Write(tab); Console.WriteLine(macro?.Name);
            Console.WriteLine(tab + "--------------------------------");
            Console.Write(tab); Console.WriteLine("description:");
            Console.Write(tab); Console.Write(tab); Console.WriteLine(macro?.Description);
            Console.Write(tab); Console.WriteLine("format:");
            Console.Write(tab); Console.Write(tab);
            Console.Write('!');
            Console.Write(macro?.Name);
            foreach (var arg in method.GetParameters()) {
                Console.Write(' ');
                Console.Write(arg.ParameterType);
                Console.Write('(');
                Console.Write(arg.Name);
                Console.Write(")");
            }
            Console.WriteLine();
            Console.WriteLine();
        }
    }
}