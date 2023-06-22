using Qkmaxware.Vm.Instructions;

namespace Qkmaxware.Vm.Test;

[TestClass]
public class NopTester {
    [TestMethod]
    public void TestAction() {
        var env = new RuntimeEnvironment();
        var args = new VmValue[] {  };

        var instr = new Nop();
        instr.Action(args, env);

        Assert.AreEqual(0, env.Stack.SP);
        Assert.AreEqual(0, env.Stack.FP);
        Assert.AreEqual(true, env.Stack.IsEmpty);
    }
}