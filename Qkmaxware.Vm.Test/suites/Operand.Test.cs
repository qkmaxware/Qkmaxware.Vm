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

        var ptr1value = new Pointer(PointerType.ConstantPoolIndex, 0);
        var ptr1op = Operand.From(ptr1value);
        Assert.AreEqual(true , ptr1value.IsConstantPoolIndex());
        Assert.AreEqual(true, ptr1op.Pointer32.IsConstantPoolIndex());
        Assert.AreEqual(false , ptr1value.IsHeapAddress());
        Assert.AreEqual(false, ptr1op.Pointer32.IsHeapAddress());
        Assert.AreEqual(0 , ptr1value.IntValue);
        Assert.AreEqual(0, ptr1op.Pointer32.IntValue);

        var ptr2value = new Pointer(PointerType.HeapAddress, 1);
        var ptr2op = Operand.From(ptr2value);
        Assert.AreEqual(false , ptr2value.IsConstantPoolIndex());
        Assert.AreEqual(false, ptr2op.Pointer32.IsConstantPoolIndex());
        Assert.AreEqual(true , ptr2value.IsHeapAddress());
        Assert.AreEqual(true, ptr2op.Pointer32.IsHeapAddress());
        Assert.AreEqual(1 , ptr2value.IntValue);
        Assert.AreEqual(1, ptr2op.Pointer32.IntValue);
    }
}