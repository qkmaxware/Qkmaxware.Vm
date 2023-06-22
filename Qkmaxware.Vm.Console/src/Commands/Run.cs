using System;
using System.IO;
using System.Text;
using CommandLine;

namespace Qkmaxware.Vm.Terminal.Commands;

// qkvm run -f /path/to/file.qkbc

[Verb("run", HelpText = "Run a bytecode module")]
public class Run : BaseCommand {
    [Option('f', "file", HelpText = "Path to bytecode file", Required = true)]
    public string? FileName {get; set;}

    [Option("heap", HelpText = "Size of the heap", Default = "1024 bytes")]
    public string? HeapSize {get; set;}

    [Option('i', "interactive", HelpText = "Run in interactive debugging mode", Default = false)]
    public bool Interactive {get; set;}

    public override void Execute() {
        this.FileName = VerifyFile(this.FileName);

        ModuleLoader loader = new ModuleLoader();
        using var reader = new BinaryReader(File.OpenRead(this.FileName));
        var module = loader.FromStream(reader);

        var heap = new LinearByteArrayMemory(DataSize.Parse(this.HeapSize ?? string.Empty, null));

        if (!Interactive) {
            var vm = new Machine(
                host: HostInterface.Default(),
                heap: heap
            );
            var thread = vm.LoadProgram(module);
            thread.RunUntilComplete();
        } else {
            var sb = new StringBuilder();
            var host = new HostInterface(
                stdin: Console.In,
                stdout: new StringWriter(sb)
            );
            var vm = new Machine(
                host: host,
                heap: heap
            );
            var thread = vm.LoadProgram(module);
            var col = Console.CursorLeft;
            var row = Console.CursorTop;

            using (var ireader = new BinaryReader(new BytecodeStream(module))) {
                bool run = true;
                while(run && thread.HasNextInstruction) {
                    ireader.BaseStream.Position = thread.PC;
                    var opcode = ireader.ReadByte();
                    var instr = InstructionMap.Instance[opcode];
                    var args = new VmValue[0];
                    if (instr != null) {
                        args = new VmValue[instr.Arity];
                        var arg = 0;
                        foreach (var argSpec in instr.Arguments) {
                            args[arg++] = argSpec.ReadValue(ireader);
                        }
                    }
                    
                    // Clear console
                    var now = Console.CursorTop;
                    Console.SetCursorPosition(col, row);
                    for (var i = 0; i < now; i++) {
                        Console.WriteLine(new String(' ', Console.BufferWidth));
                    }
                    Console.SetCursorPosition(col, row);

                    Console.WriteLine("Stack: ");
                    Console.WriteLine("    SP: " + thread.Environment.Stack.SP);
                    Console.WriteLine("    FP: " + thread.Environment.Stack.FP);
                    Console.Write("    Top: ");
                    Console.Write(thread.Environment.Stack.PeekTop()?.GetType()?.Name);  
                    Console.Write("(");  
                    Console.Write(thread.Environment.Stack.PeekTop()?.ValueToString());  
                    Console.WriteLine(")");  
                    Console.WriteLine();

                    Console.WriteLine("Next Instruction: ");
                    Console.WriteLine("    PC:");
                    Console.WriteLine("    - Decimal: " + thread.PC.ToString());
                    Console.WriteLine("    - Hex: 0x" + thread.PC.ToString("X"));
                    Console.WriteLine("    - Binary: 0b" + Convert.ToString(thread.PC, 2));
                    Console.WriteLine("    Opcode: 0x" + instr?.Opcode.ToString("X"));
                    Console.WriteLine("    Name: " + instr?.Name);
                    Console.WriteLine("    Args: ");
                    if (instr != null) {
                        for (var i = 0; i < args.Length; i++) {
                            var argv = args[i];
                            var argn = instr.Arguments.ElementAt(i);
                        Console.Write("    - ");  
                        Console.Write(argn.Name);    
                        Console.Write(": ");     
                        Console.Write(argv.GetType().Name);  
                        Console.Write("(");  
                        Console.Write(argv.ValueToString());  
                        Console.Write(")");  
                        Console.WriteLine();
                        }
                    }
                    Console.WriteLine();

                    Console.WriteLine("Standard Out: |-");
                    Console.Write("    ");
                    Console.WriteLine(sb.ToString());
                    Console.WriteLine();

                    Console.WriteLine("Breakpoints: ");
                    foreach (var bp in thread.EnumerateBreakpoints()) {
                        Console.WriteLine("- 0x" + bp.ToString("X"));
                    }
                    Console.WriteLine();

                    Console.WriteLine("Commands:");
                    Console.WriteLine("- next");
                    Console.WriteLine("- run until end");
                    Console.WriteLine("- run until breakpoint");
                    Console.WriteLine("- breakpoint add");
                    Console.WriteLine("- breakpoint remove");
                    Console.WriteLine("- exit");

                    Console.WriteLine();
                    Console.Write("> ");
                    var line = Console.ReadLine()?.ToLower()?.Trim();
                    switch (line) {
                        case "next":
                            thread.RunNext();
                            break;
                        case "run until end":
                            thread.RunUntilComplete();
                            break;
                        case "run until breakpoint":
                            thread.RunUntilBreakpoint();
                            break;
                        case "breakpoint add":
                            Console.Write("Instruction Index? ");
                            long bpa = 0;
                            long.TryParse(Console.ReadLine(), out bpa);
                            thread.AddBreakpoint(bpa);
                            break;
                        case "breakpoint remove":
                            Console.Write("Instruction Index? ");
                            long bpr = 0;
                            long.TryParse(Console.ReadLine(), out bpr);
                            thread.RemoveBreakpoint(bpr);
                            break;
                        case "exit":
                        case "quit":
                        case "close":
                            run = false;
                            break;
                    }
                }
            }
        }
    }
}