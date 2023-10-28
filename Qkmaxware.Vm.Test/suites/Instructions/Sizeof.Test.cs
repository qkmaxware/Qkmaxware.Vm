using Qkmaxware.Vm.Instructions;

namespace Qkmaxware.Vm.Test;

[TestClass]
public class SizeofTester {
    [TestMethod]
    public void TestAction() {
        var env = new RuntimeEnvironment();
        env.Memories.Add(new Memory(Mutability.ReadWrite, DataSize.Bytes(128), DataSize.Bytes(128), new LinearByteArrayMemory( DataSize.Bytes(128))));
        var args = new VmValue[] { Operand.From(0) };

        var str = "Hello";
        int offset = 5;
        env.Memories[0].Write8(0, 0x01);
        env.Memories[0].Write32(1, (uint)str.Length);
        foreach (var b in System.Text.Encoding.ASCII.GetBytes(str)) {
            env.Memories[0].Write8(offset++, b);
        }
        env.Stack.PushTop(Operand.From(0));

        var block = env.Memories[0].GetBlockInfo(0);
        Assert.AreEqual(false, block.IsFree);
        Assert.AreEqual(0, block.BlockAddress);
        Assert.AreEqual(5, block.DataAddress);
        Assert.AreEqual(5, block.Size.ByteCount);

        var instr = new Sizeof();
        instr.Action(args, env);
        
        Assert.AreEqual(1, env.Stack.SP);
        //Assert.IsInstanceOfType(env.Stack.PeekTop(), typeof(Int32Operand));
        var top = env.Stack.PopTop();
        Assert.AreEqual(str.Length, top.Int32);
    }
}