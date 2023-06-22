using Qkmaxware.Vm.Instructions;

namespace Qkmaxware.Vm.Test;

[TestClass]
public class SwapTester {
    [TestMethod]
    public void TestAction() {
        var lhs = Operand.From(3);
        var rhs = Operand.From(4);
        var env = new RuntimeEnvironment();
        var args = new VmValue[] { };

        new ImmediateI32().Action(new VmValue[]{ lhs }, env);
        new ImmediateI32().Action(new VmValue[]{ rhs }, env);

        Assert.AreEqual(2, env.Stack.SP);
        Assert.AreEqual(rhs, env.Stack.PeekTop());

        var instr = new Swap();
        instr.Action(args, env);

        Assert.AreEqual(2, env.Stack.SP);
        Assert.AreEqual(lhs, env.Stack.PeekTop());
    }
}