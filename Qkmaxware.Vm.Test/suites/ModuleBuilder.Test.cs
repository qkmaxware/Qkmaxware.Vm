using System.Text;

namespace Qkmaxware.Vm.Test;

[TestClass]
public class ModuleBuilderTester {

    [TestMethod]
    public void CreateModule() {
        using var builder = new ModuleBuilder();

        var module = builder.ToModule();
        using (var writer = new BinaryWriter(File.OpenWrite("ModuleBuilderTester.CreateModule.qkbc"))) {
            module.EncodeFile(writer);
        }
    }

    [TestMethod]
    public void TestJumplessProgam() {
        using var builder = new ModuleBuilder();

        var main = builder.Label("main");
        builder.PushInt32(3);
        builder.PushInt32(4);
        builder.AddInstruction("add_i32");

        var module = builder.ToModule();

        using (var writer = new BinaryWriter(File.OpenWrite("ModuleBuilderTester.TestJumplessProgam.qkbc"))) {
            module.EncodeFile(writer);
        }

        var loader = new ModuleLoader();
        using (var reader = new BinaryReader(File.OpenRead("ModuleBuilderTester.TestJumplessProgam.qkbc"))) {
            var decoded = loader.FromStream(reader);

            Assert.AreEqual(module.CodeLength, decoded.CodeLength);
            Assert.AreEqual(module.MemoryCount, decoded.MemoryCount);

            foreach (var instr in new Disassembler().DisassembleCode(module)) {
                Console.WriteLine(instr);
            }

            // TODO run the module and make sure it produces the expected result
        }
    }

    [TestMethod]
    public void TestConstantPool() {
        using var builder = new ModuleBuilder();

        builder.AddConstantInt(5);
        builder.AddConstantUInt(8U);
        builder.AddConstantFloat(3.14f);

        builder.AddConstantAsciiString("Hello World");

        var module = builder.ToModule();

        using (var writer = new BinaryWriter(File.OpenWrite("ModuleBuilderTester.TestConstantPool.qkbc"))) {
            module.EncodeFile(writer);
        }

        var loader = new ModuleLoader();
        using (var reader = new BinaryReader(File.OpenRead("ModuleBuilderTester.TestConstantPool.qkbc"))) {
            var decoded = loader.FromStream(reader);

            Assert.AreEqual(module.CodeLength, decoded.CodeLength);
            Assert.AreEqual(module.MemoryCount, decoded.MemoryCount);

            for (var i = 0; i < module.MemoryCount; i++) {
                Assert.AreEqual(module.Memories[i].Initializer.Count, decoded.Memories[i].Initializer.Count);
            }
        }
    }

    [TestMethod]
    public void TestHelloWorldMacro() { 
        using var builder = new ModuleBuilder();
        var str = "Hello World";
        var ptr = builder.AddConstantUtf8String(str);

        builder.PushAddressOf(ptr);
        builder.PrintStringOnStack(0);

        var module = builder.ToModule();
        using (var writer = new BinaryWriter(File.OpenWrite("ModuleBuilderTester.TestHelloWorldMacro.qkbc"))) {
            module.EncodeFile(writer);
        }

        var loader = new ModuleLoader();
        using (var reader = new BinaryReader(File.OpenRead("ModuleBuilderTester.TestHelloWorldMacro.qkbc"))) {
            var decoded = loader.FromStream(reader);
            foreach (var instr in new Disassembler().DisassembleCode(module)) {
                Console.WriteLine(instr);
            }

            var sb = new StringBuilder();
            var host = new HostInterface(
                stdin: Console.In,
                stdout: new StringWriter(sb)
            );
            var vm = new Machine(host);
            var thread = vm.LoadProgram(decoded);
            thread.RunUntilComplete();
            var stdout = (sb).ToString();
            Assert.AreEqual(str, stdout);
        }
    }
}