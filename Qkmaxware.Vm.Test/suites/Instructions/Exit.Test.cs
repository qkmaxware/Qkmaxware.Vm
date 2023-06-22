using Qkmaxware.Vm.Instructions;

namespace Qkmaxware.Vm.Test;

[TestClass]
public class ExitTester {
    [TestMethod]
    public void TestAction() {
        var env = new RuntimeEnvironment();
        var args = new VmValue[] { Operand.From(255) };

        var instr = new Exit();
        Assert.ThrowsException<VmExitRequestException>(() => instr.Action(args, env));
    }
}