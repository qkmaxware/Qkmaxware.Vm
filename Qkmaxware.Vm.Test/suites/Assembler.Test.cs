using System.Text;

namespace Qkmaxware.Vm.Test;

[TestClass]
public class AssemblerTester {

public string Program1 = @"use asm 1.0

@hello = ""Hello World""

.main
    load_const @hello
    !printstr
";

    [TestMethod]
    public void TestProgram1() {
        var asm = new Assembly.Assembler();
        using var reader = new StringReader(Program1);
        
        var module = asm.FromStream(reader);

        var sb = new StringBuilder();
        var host = new HostInterface(
            stdin: Console.In,
            stdout: new StringWriter(sb)
        );
        var vm = new Machine(host);
        var thread = vm.LoadProgram(module);
        thread.RunUntilComplete();
        var stdout = (sb).ToString();
        Assert.AreEqual("Hello World", stdout);
    }

public string Program2 = @"use asm 1.0

@pi = 3.1415926f
@r = 12f

.main   
    load_const @pi

    // Compute r^2
    load_const @r
    load_const @r
    mul_f32

    // Compute pi * r^2
    mul_f32
";

    [TestMethod]
    public void TestProgram2() {
        var asm = new Assembly.Assembler();
        using var reader = new StringReader(Program2);
        
        var module = asm.FromStream(reader);

        var sb = new StringBuilder();
        var host = new HostInterface(
            stdin: Console.In,
            stdout: new StringWriter(sb)
        );
        var vm = new Machine(host);
        var thread = vm.LoadProgram(module);
        thread.RunUntilComplete();
        Assert.AreEqual(false, thread.Environment.Stack.IsEmpty);
        Assert.IsInstanceOfType(thread.Environment.Stack.PeekTop(), typeof(Float32Operand));
        Assert.AreEqual(3.1415926f * 12*12, ((Float32Operand)thread.Environment.Stack.PopTop()).Value, 0.001f);
    }


}