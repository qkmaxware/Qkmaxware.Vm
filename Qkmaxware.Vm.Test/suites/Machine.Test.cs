namespace Qkmaxware.Vm.Test;

[TestClass]
public class MachineTester {

    [TestMethod]
    public void TestIntAddProgram() {
        using var builder = new ModuleBuilder();
        builder.PushInt32(4);
        builder.PushInt32(3);
        builder.AddInstruction("add_i32");
        var module = builder.ToModule();

        Machine machine = new Machine();
        var thread = machine.LoadProgram(module);
        thread.RunUntilComplete();

        Assert.AreEqual(1, thread.Environment.Stack.SP);
        Assert.AreEqual(0, thread.Environment.Stack.FP);
        //Assert.IsInstanceOfType(thread.Environment.Stack.PeekTop(), typeof(Int32Operand));
        var top = thread.Environment.Stack.PopTop();
        Assert.AreEqual(7, top.Int32);
    }
}