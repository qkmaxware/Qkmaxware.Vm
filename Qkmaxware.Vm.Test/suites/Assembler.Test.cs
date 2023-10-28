using System.Text;

namespace Qkmaxware.Vm.Test;

[TestClass]
public class AssemblerTester {

public string Program1 = @"use asm 1.0

@hello = ""Hello World""

.main
    immediate_i32 @hello
    !printstr 0
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

        using (var writer = new BinaryWriter(File.OpenWrite("AssemblerTester.TestProgram1.qkbc"))) {
            module.EncodeFile(writer);
        }
    }

public string Program2 = @"use asm 1.0

@pi = 3.1415926f
@r = 12f

.main   
    !load_const32 @pi

    // Compute r^2
    !load_const32 @r
    !load_const32 @r
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
        //Assert.IsInstanceOfType(thread.Environment.Stack.PeekTop(), typeof(Float32Operand));
        Assert.AreEqual(3.1415926f * 12*12, (thread.Environment.Stack.PopTop()).Float32, 0.001f);
    }


    [TestMethod]
    public void TestAssembleAndLink() {
var main = @"use asm 1.0

import ""System.Console.PrintString""

@hello = ""Hello World""

export ""Main""
.main
    immediate_i32 @hello
    call_external ""System.Console.PrintString"" 1
    exit 0";
var lib = @"use asm 1.0

exit 1 // We should never get here, if we do something is wrong
export ""System.Console.PrintString""
    load_arg 0
    !printstr 0
    return_procedure";

        var assembler = new Assembly.Assembler();
        using var main_reader = new StringReader(main);
        var main_mod = assembler.FromStream(main_reader);
        using var lib_reader = new StringReader(lib);
        var lib_mod = assembler.FromStream(lib_reader);

        var linker = new Linker();
        var linked = linker.Link(main_mod, lib_mod);

        var sb = new StringBuilder();
        var host = new HostInterface(
            stdin: Console.In,
            stdout: new StringWriter(sb)
        );
        var vm = new Machine(host);
        var thread = vm.LoadProgram(linked);
        thread.RunUntilComplete();

        Assert.AreEqual("Hello World", sb.ToString());
    }

}