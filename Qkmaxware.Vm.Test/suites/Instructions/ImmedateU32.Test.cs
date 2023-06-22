using Qkmaxware.Vm.Instructions;

namespace Qkmaxware.Vm.Test;

[TestClass]
public class ImmediateU32Tester {
    [TestMethod]
    public void TestAction() {
        var value = 4u;
        var env = new RuntimeEnvironment();
        var args = new VmValue[] { new UInt32Operand(value) };

        var instr = new ImmediateU32();
        instr.Action(args, env);

        Assert.AreEqual(1, env.Stack.SP);
        Assert.AreEqual(0, env.Stack.FP);
        Assert.AreEqual(false, env.Stack.IsEmpty);
        Assert.IsNotNull(env.Stack.PeekTop());
        var result = env.Stack.PopTop();
        Assert.IsInstanceOfType(result, typeof(UInt32Operand));
        Assert.AreEqual(value, ((UInt32Operand)result).Value);
    }
}