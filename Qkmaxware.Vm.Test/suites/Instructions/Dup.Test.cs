using Qkmaxware.Vm.Instructions;

namespace Qkmaxware.Vm.Test;

[TestClass]
public class DupTester {
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

        var instr = new Dup();
        instr.Action(args, env);

        Assert.AreEqual(3, env.Stack.SP);
        Assert.AreEqual(rhs, env.Stack.PopTop());
        Assert.AreEqual(rhs, env.Stack.PopTop());
        Assert.AreEqual(lhs, env.Stack.PopTop());
    }
}