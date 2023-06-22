using Qkmaxware.Vm.Instructions;

namespace Qkmaxware.Vm.Test;

[TestClass]
public class OrI32Tester {
    [TestMethod]
    public void TestAction() {
        var lhs = 3;
        var rhs = 4;
        var computed = lhs | rhs;
        var env = new RuntimeEnvironment();
        var args = new VmValue[] { };

        new ImmediateI32().Action(new VmValue[]{ Operand.From(lhs) }, env);
        new ImmediateI32().Action(new VmValue[]{ Operand.From(rhs) }, env);

        var instr = new OrI32();
        instr.Action(args, env);

        Assert.AreEqual(1, env.Stack.SP);
        Assert.AreEqual(0, env.Stack.FP);
        Assert.AreEqual(false, env.Stack.IsEmpty);
        Assert.IsNotNull(env.Stack.PeekTop());
        var result = env.Stack.PopTop();
        //Assert.IsInstanceOfType(result, typeof(Int32Operand));
        Assert.AreEqual(computed, (result).Int32);
    }
}