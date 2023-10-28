namespace Qkmaxware.Vm.Test;

[TestClass]
public class OperandTester {

    [TestMethod]
    public void TestConstructor() {
        var uvalue = 6u;
        var uop = Operand.From(uvalue);
        Assert.AreEqual(uvalue, uop.UInt32);

        var ivalue = -5;
        var iop = Operand.From(ivalue);
        Assert.AreEqual(ivalue, iop.Int32);

        var fvalue = 4.5f;
        var fop = Operand.From(fvalue);
        Assert.AreEqual(fvalue, fop.Float32);
    }
}