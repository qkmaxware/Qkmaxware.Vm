using Qkmaxware.Vm.Instructions;

namespace Qkmaxware.Vm.Test;

[TestClass]
public class StoreLocalTester {
    [TestMethod]
    public void TestAction() {
        var lhs = Operand.From(3);
        var rhs = Operand.From(4);
        var env = new RuntimeEnvironment();
        var args = new VmValue[] { Operand.From(0) };

        // -- Fill in placeholders for frame values
        new ImmediateI32().Action(new VmValue[]{ Operand.From(0) }, env);
        new ImmediateI32().Action(new VmValue[]{ Operand.From(0) }, env);
        new ImmediateI32().Action(new VmValue[]{ Operand.From(0) }, env);
        new ImmediateI32().Action(new VmValue[]{ Operand.From(0) }, env);
        //

        new ImmediateI32().Action(new VmValue[]{ lhs }, env);
        new ImmediateI32().Action(new VmValue[]{ rhs }, env);

        Assert.AreEqual(4 + 2, env.Stack.SP);
        Assert.AreEqual(rhs, env.Stack.PeekTop());

        var instr = new StoreLocal();
        instr.Action(args, env);

        Assert.AreEqual(4 + 1, env.Stack.SP);
        Assert.AreEqual(rhs, env.Stack.PeekTop());
    }
}