using Qkmaxware.Vm.Instructions;

namespace Qkmaxware.Vm.Test;

[TestClass]
public class LoadLocalTester {
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

        var instr = new LoadLocal();
        instr.Action(args, env);

        Assert.AreEqual(4 + 3, env.Stack.SP);
        Assert.AreEqual(lhs, env.Stack.PeekTop());
        
        args = new VmValue[] { Operand.From(1) };
        instr = new LoadLocal();
        instr.Action(args, env);

        Assert.AreEqual(4 + 4, env.Stack.SP);
        Assert.AreEqual(rhs, env.Stack.PeekTop());
    }
}