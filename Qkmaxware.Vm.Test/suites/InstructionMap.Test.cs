namespace Qkmaxware.Vm.Test;

[TestClass]
public class InstructionMapTester {

    [TestMethod]
    public void TestInstructionCount() {
        var map = InstructionMap.Instance;
        Assert.AreEqual(false, map.IsEmpty);
    }

}