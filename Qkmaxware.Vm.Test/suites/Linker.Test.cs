using System.Text;

namespace Qkmaxware.Vm.Test;

[TestClass]
public class LinkerTester {

    private Module source() {
        using var builder = new ModuleBuilder();
        var import = builder.ImportSubprogram("System.Console.PrintString");
        var str = builder.AddConstantUtf8String("Hello World");

        builder.PushAddressOf(str);
        builder.CallExternal(import, 1);
        builder.Exit(0);
        
        return builder.ToModule();
    }
    private Module library() {
        using var builder = new ModuleBuilder();
        builder.Exit(1);
        var import = builder.ExportSubprogram("System.Console.PrintString");
        builder.PushArgument(0);
        builder.PrintStringOnStack(-2); // Linker will change this index
        builder.Return();
        
        return builder.ToModule();
    }

    [TestMethod]
    public void TestCounts() {
        var primary = source();
        Assert.AreEqual(0, primary.ExportCount);
        Assert.AreEqual(1, primary.ImportCount);

        var lib = library();
        Assert.AreEqual(1, lib.ExportCount);
        Assert.AreEqual(0, lib.ImportCount);

        var linked = new Linker().Link(primary, lib);

        Assert.AreEqual(1, linked.ExportCount);
        Assert.AreEqual(0, linked.ImportCount);
        Assert.AreEqual(primary.MemoryCount + lib.MemoryCount, linked.MemoryCount);
        Assert.AreEqual(primary.CodeLength + lib.CodeLength, linked.CodeLength);
    }

    [TestMethod]
    public void TestExternalCall() {
        var primary = source();
        var lib = library();

        var linked = new Linker().Link(primary, lib);

        var sb = new StringBuilder();
        var host = new HostInterface(
            stdin: Console.In,
            stdout: new StringWriter(sb)
        );
        var vm = new Machine(host);
        var thread = vm.LoadProgram(linked);

        using (var writer = new BinaryWriter(File.OpenWrite("LinkerTester.TestExternalCall.qkbc"))) {
            linked.EncodeFile(writer);
        }

        thread.RunUntilComplete();
        foreach (var instr in new Disassembler().DisassembleCode(linked)) {
            Console.WriteLine(instr);
        }

        Assert.AreEqual("Hello World", sb.ToString());
    }
}