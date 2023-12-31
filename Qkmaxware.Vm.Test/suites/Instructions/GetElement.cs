using Qkmaxware.Vm.Instructions;

namespace Qkmaxware.Vm.Test;

[TestClass]
public class GetElementTester {
    [TestMethod]
    public void TestAction() {
        var env = new RuntimeEnvironment();
        var args = new VmValue[] {  };

        var str = "Hello";
        var constant = new StringConstant(ConstantInfo.Utf8, str);
        env.ConstantPool.Add(constant);
        env.Stack.PushTop(Operand.From(new Pointer(constant)));
        env.Stack.PushTop(Operand.From(2));

        var instr = new GetElement();
        instr.Action(args, env);
        
        Assert.AreEqual(1, env.Stack.SP);
        //Assert.IsInstanceOfType(env.Stack.PeekTop(), typeof(Int32Operand));
        var top = env.Stack.PopTop();
        Assert.AreEqual('l', top.Int32);
    }

    
    [TestMethod]
    public void TestSimpleArray() {
        var array = new PrimitiveArrayConstant(ConstantInfo.Int32Array, new Int32Constant[]{
            new Int32Constant(1),
            new Int32Constant(1),
            new Int32Constant(2),
            new Int32Constant(3),
            new Int32Constant(5),
            new Int32Constant(8)
        });

        var env = new RuntimeEnvironment();

        env.ConstantPool.Add(array);

        // Load the array
        var load = new LoadConst();
        load.Action(new VmValue[] { Operand.From(0) }, env);

        for (var i = 0; i < array.ElementCount(); i++) {
            var dup = new Dup();
            dup.Action(new VmValue[0], env);
            var index = new ImmediateI32();
            index.Action(new VmValue[] { Operand.From(i) }, env);
            var element = new GetElement();
            element.Action(new VmValue[0], env);

            //Assert.IsInstanceOfType(env.Stack.PeekTop(), typeof(Int32Operand));
            var top = env.Stack.PopTop();
            Assert.AreEqual(((Operand)array.GetElementAt(i)).Int32, top.Int32);
        }
    }
}