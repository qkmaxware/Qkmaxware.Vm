using Qkmaxware.Vm.Instructions;

namespace Qkmaxware.Vm.Test;

[TestClass]
public class LoadConstTester {
    [TestMethod]
    public void TestAction() {
        using var builder = new ModuleBuilder();

        builder.AddConstantInt(3);
        builder.AddConstantInt(4);
        var module = builder.ToModule();
        var env = new RuntimeEnvironment(module, HostInterface.Default(), LinearByteArrayMemory.Zero);
        var args = new VmValue[] { new Int32Operand(1) };


        var instr = new LoadConst();
        instr.Action(args, env);

        Assert.AreEqual(1, env.Stack.SP);
        Assert.IsInstanceOfType(env.Stack.PeekTop(), typeof(Int32Operand));
        Assert.AreEqual(4, ((Int32Operand)env.Stack.PopTop()).Value);
    }
}