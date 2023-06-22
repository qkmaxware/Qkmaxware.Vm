using CommandLine;
using Qkmaxware.Vm.Terminal.Commands;

namespace Qkmaxware.Vm.Terminal;

public class Program {

    public static int Main() {
        return Parser.Default
            .ParseArguments<Run, Describe, ConvertVersion, Assemble, Docs>(
                System.Environment.GetCommandLineArgs().Skip(1)
            )
            .MapResult(
                (Run run) => (int)run.TryExecute(),
                (Describe desc) => (int)desc.TryExecute(),
                (ConvertVersion conv) => (int)conv.TryExecute(),
                (Assemble asm) => (int)asm.TryExecute(),
                (Docs docs) => (int)docs.TryExecute(),
                errs => 1
            );
    }

}