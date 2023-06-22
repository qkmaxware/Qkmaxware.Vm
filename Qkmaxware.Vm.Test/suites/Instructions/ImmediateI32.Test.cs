using Qkmaxware.Vm.Instructions;

namespace Qkmaxware.Vm.Test;

[TestClass]
public class ImmediateI32Tester {
    [TestMethod]
    public void TestAction() {
        var value = 4;
        var env = new RuntimeEnvironment();
        var args = new VmValue[] { Operand.From(value) };

        var instr = new ImmediateI32();
        instr.Action(args, env);

        Assert.AreEqual(1, env.Stack.SP);
        Assert.AreEqual(0, env.Stack.FP);
        Assert.AreEqual(false, env.Stack.IsEmpty);
        Assert.IsNotNull(env.Stack.PeekTop());
        var result = env.Stack.PopTop();
        //Assert.IsInstanceOfType(result, typeof(Int32Operand));
        Assert.AreEqual(value, (result).Int32);
    }
}