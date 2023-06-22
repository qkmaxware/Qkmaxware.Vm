namespace Qkmaxware.Vm.Test;

[TestClass]
public class OperandStackTester {

    [TestMethod]
    public void TestPushPop() {
        var stack = new OperandStack();
        var value = 4;
        // Default state assertions
        Assert.AreEqual(true, stack.IsEmpty);
        Assert.AreEqual(0, stack.SP);
        Assert.AreEqual(0, stack.FP);
        Assert.IsNull(stack.PeekTop());
        // Manipulation assertions
        stack.PushTop(Operand.From(value));
        Assert.AreEqual(false, stack.IsEmpty);
        Assert.AreEqual(1, stack.SP);
        Assert.AreEqual(0, stack.FP);
        var peeked = stack.PeekTop();
        Assert.IsNotNull(peeked);
        //.IsInstanceOfType(peeked, typeof(Int32Operand));
        var popped = stack.PopTop();
        Assert.AreEqual(peeked, popped);
        Assert.AreEqual(value, (peeked).Int32);
        Assert.AreEqual(value, (popped).Int32);
        Assert.AreEqual(true, stack.IsEmpty);
        Assert.AreEqual(0, stack.SP);
        Assert.AreEqual(0, stack.FP);
        Assert.IsNull(stack.PeekTop());
    }

    [TestMethod]
    public void TestFrame() {
        var stack = new OperandStack();
        stack.PushTop(Operand.From(1));
        stack.PushTop(Operand.From(2));
        stack.PushTop(Operand.From(3)); // FP here
        stack.PushTop(Operand.From(4));
        // Default state assertions
        Assert.AreEqual(false, stack.IsEmpty);
        Assert.AreEqual(4, stack.SP);
        Assert.AreEqual(0, stack.FP);
        Assert.IsNotNull(stack.PeekTop());
        // Stack frame manipulations
        stack.FP = -1;
        Assert.AreEqual(4, stack.SP);
        Assert.AreEqual(0, stack.FP);
        stack.FP = 100;
        Assert.AreEqual(4, stack.SP);
        Assert.AreEqual(4, stack.FP);
        stack.FP = 2;
        Assert.AreEqual(4, stack.SP);
        Assert.AreEqual(2, stack.FP);
        // Test local loading
        var local = stack.GetFrameRelative(0);
        Assert.IsNotNull(local);
        //Assert.IsInstanceOfType(local, typeof(Int32Operand));
        Assert.AreEqual(3, (local).Int32);
        local = stack.GetFrameRelative(1);
        Assert.IsNotNull(local);
        //Assert.IsInstanceOfType(local, typeof(Int32Operand));
        Assert.AreEqual(4, (local).Int32);
        local = stack.GetFrameRelative(-1);
        Assert.IsNotNull(local);
        //Assert.IsInstanceOfType(local, typeof(Int32Operand));
        Assert.AreEqual(2, (local).Int32);
        Assert.ThrowsException<IndexOutOfRangeException>(() => stack.GetFrameRelative(100));
        Assert.ThrowsException<IndexOutOfRangeException>(() => stack.GetFrameRelative(-100));
        // Test local saving
        var float_val = 911f;
        stack.SetFrameRelative(0, Operand.From(float_val));
        local = stack.GetFrameRelative(0);
        //Assert.IsInstanceOfType(local, typeof(Float32Operand));
        Assert.AreEqual(float_val, (local).Float32);
        Assert.ThrowsException<IndexOutOfRangeException>(() => stack.SetFrameRelative(100, Operand.From(float_val)));
        Assert.ThrowsException<IndexOutOfRangeException>(() => stack.SetFrameRelative(-100, Operand.From(float_val)));
    }

}