using Qkmaxware.Vm.Instructions;

namespace Qkmaxware.Vm.Test;

[TestClass]
public class PopTester {
    [TestMethod]
    public void TestAction() {
        var lhs = new Int32Operand(3);
        var rhs = new Int32Operand(4);
        var env = new RuntimeEnvironment();
        var args = new VmValue[] { };

        new ImmediateI32().Action(new VmValue[]{ lhs }, env);
        new ImmediateI32().Action(new VmValue[]{ rhs }, env);

        Assert.AreEqual(2, env.Stack.SP);
        Assert.AreEqual(rhs, env.Stack.PeekTop());

        var instr = new Pop();
        instr.Action(args, env);

        Assert.AreEqual(1, env.Stack.SP);
        Assert.AreEqual(lhs, env.Stack.PeekTop());
    }
}