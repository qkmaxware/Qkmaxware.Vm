using Qkmaxware.Vm.Instructions;

namespace Qkmaxware.Vm.Test;

[TestClass]
public class LenTester {
    [TestMethod]
    public void TestAction() {
        var env = new RuntimeEnvironment();
        var args = new VmValue[] {  };

        var str = "Hello";
        var constant = new StringConstant(ConstantInfo.Utf8, str);
        env.ConstantPool.Add(constant);
        env.Stack.PushTop(Operand.From(new Pointer(PointerType.ConstantPoolIndex, constant.PoolIndex)));

        var instr = new Len();
        instr.Action(args, env);
        
        Assert.AreEqual(1, env.Stack.SP);
        //Assert.IsInstanceOfType(env.Stack.PeekTop(), typeof(Int32Operand));
        var top = env.Stack.PopTop();
        Assert.AreEqual(str.Length, top.Int32);
    }
}