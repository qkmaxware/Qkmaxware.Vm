using Qkmaxware.Vm.Instructions;

namespace Qkmaxware.Vm.Test;

[TestClass]
public class ModI32Tester {
    [TestMethod]
    public void TestPositiveAction() {
        var lhs = 3;
        var rhs = 4;
        var computed = lhs % rhs;
        var env = new RuntimeEnvironment();
        var args = new VmValue[] { };

        new ImmediateI32().Action(new VmValue[]{ new Int32Operand(lhs) }, env);
        new ImmediateI32().Action(new VmValue[]{ new Int32Operand(rhs) }, env);

        var instr = new ModI32();
        instr.Action(args, env);

        Assert.AreEqual(1, env.Stack.SP);
        Assert.AreEqual(0, env.Stack.FP);
        Assert.AreEqual(false, env.Stack.IsEmpty);
        Assert.IsNotNull(env.Stack.PeekTop());
        var result = env.Stack.PopTop();
        Assert.IsInstanceOfType(result, typeof(Int32Operand));
        Assert.AreEqual(computed, ((Int32Operand)result).Value);
    }

    [TestMethod]
    public void TestNegativeAction() {
        var lhs = 3;
        var rhs = -4;
        var computed = -1;
        var env = new RuntimeEnvironment();
        var args = new VmValue[] { };

        new ImmediateI32().Action(new VmValue[]{ new Int32Operand(lhs) }, env);
        new ImmediateI32().Action(new VmValue[]{ new Int32Operand(rhs) }, env);

        var instr = new ModI32();
        instr.Action(args, env);

        Assert.AreEqual(1, env.Stack.SP);
        Assert.AreEqual(0, env.Stack.FP);
        Assert.AreEqual(false, env.Stack.IsEmpty);
        Assert.IsNotNull(env.Stack.PeekTop());
        var result = env.Stack.PopTop();
        Assert.IsInstanceOfType(result, typeof(Int32Operand));
        Assert.AreEqual(computed, ((Int32Operand)result).Value);
    }
}