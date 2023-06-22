using Qkmaxware.Vm.Instructions;

namespace Qkmaxware.Vm.Test;

[TestClass]
public class SizeofTester {
    [TestMethod]
    public void TestAction() {
        var env = new RuntimeEnvironment();
        var args = new VmValue[] {  };

        var str = "Hello";
        var constant = new StringConstant(ConstantInfo.Utf32, str);
        env.ConstantPool.Add(constant);
        env.Stack.PushTop(new ConstantPoolArrayPointerOperand(constant));

        var instr = new Sizeof();
        instr.Action(args, env);
        
        Assert.AreEqual(1, env.Stack.SP);
        Assert.IsInstanceOfType(env.Stack.PeekTop(), typeof(Int32Operand));
        var top = (Int32Operand)env.Stack.PopTop();
        Assert.AreEqual(str.Length * 4, top.Value);
    }
}