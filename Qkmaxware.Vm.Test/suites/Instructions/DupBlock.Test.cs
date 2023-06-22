using Qkmaxware.Vm.Instructions;

namespace Qkmaxware.Vm.Test;

[TestClass]
public class DupNTester {
    [TestMethod]
    public void TestAction() {
        var lhs = Operand.From(3);
        var rhs = Operand.From(4);
        var env = new RuntimeEnvironment();
        var args = new VmValue[] { Operand.From(2) };

        new ImmediateI32().Action(new VmValue[]{ lhs }, env);
        new ImmediateI32().Action(new VmValue[]{ rhs }, env);

        Assert.AreEqual(2, env.Stack.SP);
        Assert.AreEqual(rhs, env.Stack.PeekTop());

        var instr = new DupBlock();
        instr.Action(args, env);

        Assert.AreEqual(4, env.Stack.SP);
        Assert.AreEqual(rhs, env.Stack.PopTop());
        Assert.AreEqual(lhs, env.Stack.PopTop());
        Assert.AreEqual(rhs, env.Stack.PopTop());
        Assert.AreEqual(lhs, env.Stack.PopTop());
    }
}