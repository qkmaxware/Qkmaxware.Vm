using System.Text;

namespace Qkmaxware.Vm.Test;

[TestClass]
public class ModuleBuilderTester {

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
            Assert.AreEqual(module.ConstantPoolCount, decoded.ConstantPoolCount);

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
            Assert.AreEqual(module.ConstantPoolCount, decoded.ConstantPoolCount);

            Assert.IsInstanceOfType(decoded.ConstantPool[0], typeof(Int32Constant));
            Assert.AreEqual(5, ((Int32Constant)decoded.ConstantPool[0]).Value);

            Assert.IsInstanceOfType(decoded.ConstantPool[1], typeof(UInt32Constant));
            Assert.AreEqual(8U, ((UInt32Constant)decoded.ConstantPool[1]).Value);

            Assert.IsInstanceOfType(decoded.ConstantPool[2], typeof(Float32Constant));
            Assert.AreEqual(3.14f, ((Float32Constant)decoded.ConstantPool[2]).Value);

            Assert.IsInstanceOfType(decoded.ConstantPool[3], typeof(StringConstant));
            Assert.AreEqual("Hello World", ((StringConstant)decoded.ConstantPool[3]).Value);
        }
    }

    [TestMethod]
    public void TestHelloWorld() {
        using var builder = new ModuleBuilder();
        var str = "Hello World";
        builder.AddConstant(new StringConstant(ConstantInfo.Utf8, str));

        builder.PushConstant(0);
        for (var i = 0; i < 11; i++) {
            builder.AddInstruction("dup");                            // Address to array
            builder.PushInt32(i);                           // Index to character
            builder.AddInstruction("get_element");                    // Fetch Address[Index]
            builder.AddInstruction("putchar");                        // Print the character
        }

        var module = builder.ToModule();

        using (var writer = new BinaryWriter(File.OpenWrite("ModuleBuilderTester.TestHelloWorld.qkbc"))) {
            module.EncodeFile(writer);
        }

        var loader = new ModuleLoader();
        using (var reader = new BinaryReader(File.OpenRead("ModuleBuilderTester.TestHelloWorld.qkbc"))) {
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

    [TestMethod]
    public void TestHelloWorldMacro() {
        using var builder = new ModuleBuilder();
        var str = "Hello World";
        builder.AddConstant(new StringConstant(ConstantInfo.Utf8, str));

        builder.PushConstant(0);
        builder.PrintString();

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