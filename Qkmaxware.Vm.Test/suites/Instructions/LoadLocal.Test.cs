using Qkmaxware.Vm.Instructions;

namespace Qkmaxware.Vm.Test;

[TestClass]
public class LoadLocalTester {
    [TestMethod]
    public void TestAction() {
        var lhs = new Int32Operand(3);
        var rhs = new Int32Operand(4);
        var env = new RuntimeEnvironment();
        var args = new VmValue[] { new Int32Operand(0) };

        // -- Fill in placeholders
        new ImmediateI32().Action(new VmValue[]{ new Int32Operand(0) }, env);
        new ImmediateI32().Action(new VmValue[]{ new Int32Operand(0) }, env);
        new ImmediateI32().Action(new VmValue[]{ new Int32Operand(0) }, env);
        new ImmediateI32().Action(new VmValue[]{ new Int32Operand(0) }, env);
        //

        new ImmediateI32().Action(new VmValue[]{ lhs }, env);
        new ImmediateI32().Action(new VmValue[]{ rhs }, env);

        Assert.AreEqual(4 + 2, env.Stack.SP);
        Assert.AreEqual(rhs, env.Stack.PeekTop());

        var instr = new LoadLocal();
        instr.Action(args, env);

        Assert.AreEqual(4 + 3, env.Stack.SP);
        Assert.AreEqual(lhs, env.Stack.PeekTop());
        
        args = new VmValue[] { new Int32Operand(1) };
        instr = new LoadLocal();
        instr.Action(args, env);

        Assert.AreEqual(4 + 4, env.Stack.SP);
        Assert.AreEqual(rhs, env.Stack.PeekTop());
    }
}