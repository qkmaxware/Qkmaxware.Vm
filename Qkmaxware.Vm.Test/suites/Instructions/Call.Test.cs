using Qkmaxware.Vm.Instructions;

namespace Qkmaxware.Vm.Test;

[TestClass]
public class CallTester {

    [TestMethod]
    public void TestCallProcedure() {
        using var builder = new ModuleBuilder();

        // Before the call
        builder.PushInt32(3);       // Procedure argument
        builder.PushInt32(4);       // Procedure argument
        builder.Call(5, argc: 2);   // Invocation
        builder.Exit(0);            // End program

        // After the call
        builder.PushArgument(0);    // Load 3
        builder.PushArgument(1);    // Load 4
        builder.AddInt32();         // Add 3 + 4
        builder.Return();           // Return with nothing

        var module = builder.ToModule();
        var vm = new Machine();
        var thread = vm.LoadProgram(module);

        thread.RunNext();
        thread.RunNext();
        Assert.AreEqual(2, thread.Environment.Stack.SP);
        Assert.AreEqual(0, thread.Environment.Stack.FP);

        thread.RunNext();
        Assert.AreEqual(4+2, thread.Environment.Stack.SP);
        Assert.AreEqual(2, thread.Environment.Stack.FP);

        #nullable disable
        thread.RunNext();
        Assert.AreEqual(4+2+1, thread.Environment.Stack.SP);
        Assert.IsInstanceOfType(thread.Environment.Stack.PeekTop(), typeof(Int32Operand));
        var arg_1 = (Int32Operand)thread.Environment.Stack.PeekTop();
        Assert.AreEqual(3, arg_1.Value);

        thread.RunNext();
        Assert.AreEqual(4+2+2, thread.Environment.Stack.SP);
        Assert.IsInstanceOfType(thread.Environment.Stack.PeekTop(), typeof(Int32Operand));
        var arg_2 = (Int32Operand)thread.Environment.Stack.PeekTop();
        Assert.AreEqual(4, arg_2.Value);

        thread.RunNext();
        Assert.AreEqual(4+2+1, thread.Environment.Stack.SP);
        Assert.IsInstanceOfType(thread.Environment.Stack.PeekTop(), typeof(Int32Operand));
        var res = (Int32Operand)thread.Environment.Stack.PeekTop();
        Assert.AreEqual(3 + 4, res.Value);

        thread.RunNext();
        Assert.AreEqual(2, thread.Environment.Stack.SP);
        Assert.AreEqual(0, thread.Environment.Stack.FP);
        #nullable restore
    }

    /*
    function main() {
        add(3, 4);
    }
    function add(x, y) {
        var z = x + y
        return z;
    }
    */

    [TestMethod]
    public void TestCallFunction() {
        using var builder = new ModuleBuilder();

        // Before the call
        builder.PushInt32(3);       // Procedure argument
        builder.PushInt32(4);       // Procedure argument
        builder.Call(5, argc: 2);   // Invocation
        builder.Exit(0);            // End program

        // After the call
        builder.PushArgument(0);    // Load 3
        builder.PushArgument(1);    // Load 4
        builder.AddInt32();         // Add 3 + 4
        builder.ReturnResult();     // Return result of 3 + 4

        var module = builder.ToModule();
        var vm = new Machine();
        var thread = vm.LoadProgram(module);

        thread.RunNext();
        thread.RunNext();
        Assert.AreEqual(2, thread.Environment.Stack.SP);
        Assert.AreEqual(0, thread.Environment.Stack.FP);

        thread.RunNext();
        Assert.AreEqual(4+2, thread.Environment.Stack.SP);
        Assert.AreEqual(2, thread.Environment.Stack.FP);

        #nullable disable
        thread.RunNext();
        Assert.AreEqual(4+2+1, thread.Environment.Stack.SP);
        Assert.IsInstanceOfType(thread.Environment.Stack.PeekTop(), typeof(Int32Operand));
        var arg_1 = (Int32Operand)thread.Environment.Stack.PeekTop();
        Assert.AreEqual(3, arg_1.Value);

        thread.RunNext();
        Assert.AreEqual(4+2+2, thread.Environment.Stack.SP);
        Assert.IsInstanceOfType(thread.Environment.Stack.PeekTop(), typeof(Int32Operand));
        var arg_2 = (Int32Operand)thread.Environment.Stack.PeekTop();
        Assert.AreEqual(4, arg_2.Value);

        thread.RunNext();
        Assert.AreEqual(4+2+1, thread.Environment.Stack.SP);
        Assert.IsInstanceOfType(thread.Environment.Stack.PeekTop(), typeof(Int32Operand));
        var res = (Int32Operand)thread.Environment.Stack.PeekTop();
        Assert.AreEqual(3 + 4, res.Value);

        thread.RunNext();
        Assert.AreEqual(3, thread.Environment.Stack.SP);
        Assert.AreEqual(0, thread.Environment.Stack.FP);
        Assert.IsInstanceOfType(thread.Environment.Stack.PeekTop(), typeof(Int32Operand));
        var ret = (Int32Operand)thread.Environment.Stack.PeekTop();
        Assert.AreEqual(3 + 4, ret.Value);
        #nullable restore
    }
}