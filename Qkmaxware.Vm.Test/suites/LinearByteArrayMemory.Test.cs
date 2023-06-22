namespace Qkmaxware.Vm.Test;

[TestClass]
public class LinearByteArrayMemoryTester {

    [TestMethod]
    public void TestReserve() {
        var memory = new LinearByteArrayMemory(DataSize.Bytes(128));
        var blocks = memory.EnumerateBlocks().ToList();
        Assert.AreEqual(1, blocks.Count);
        Assert.AreEqual(true, blocks[0].IsFree);
        Assert.AreEqual(128 - 5, blocks[0].Size.ByteCount);

        Console.WriteLine("Before: ");
        Console.WriteLine(memory);
        memory.Reserve(byteCount: 64);
        Console.WriteLine("After 64 Reserved: ");
        Console.WriteLine(memory);

        blocks = memory.EnumerateBlocks().ToList();
        Assert.AreEqual(2, blocks.Count);
        Assert.AreEqual(false, blocks[0].IsFree);
        Assert.AreEqual(64, blocks[0].Size.ByteCount);
        Assert.AreEqual(true, blocks[1].IsFree);

        memory.Reserve(byteCount: 32);
        blocks = memory.EnumerateBlocks().ToList();
        Console.WriteLine("After 32 Reserved: ");
        Console.WriteLine(memory);
        Assert.AreEqual(3, blocks.Count);
        Assert.AreEqual(false, blocks[0].IsFree);
        Assert.AreEqual(false, blocks[1].IsFree);
        Assert.AreEqual(64, blocks[0].Size.ByteCount);
        Assert.AreEqual(32, blocks[1].Size.ByteCount);
        Assert.AreEqual(true, blocks[2].IsFree);
    }

    [TestMethod]
    public void TestFree() {
        var memory = new LinearByteArrayMemory(DataSize.Bytes(128));
        var blocks = memory.EnumerateBlocks().ToList();
        Assert.AreEqual(1, blocks.Count);
        Assert.AreEqual(true, blocks[0].IsFree);
        Assert.AreEqual(128 - 5, blocks[0].Size.ByteCount);

        var one = memory.Reserve(byteCount: 16);
        var two = memory.Reserve(byteCount: 16);
        var three = memory.Reserve(byteCount: 16);
        var four = memory.Reserve(byteCount: 16);

        blocks = memory.EnumerateBlocks().ToList();
        Console.WriteLine(string.Join(System.Environment.NewLine, blocks));
        Console.WriteLine("1----------------------");
        Assert.AreEqual(5, blocks.Count);
        for (var i = 0; i < 4; i++) {
            Assert.AreEqual(false, blocks[i].IsFree);
            Assert.AreEqual(16, blocks[i].Size.ByteCount);
        }
        Assert.AreEqual(true, blocks[4].IsFree);
        var sizeBefore = blocks[4].Size.ByteCount;

        memory.Free(blocks[3]);
        blocks = memory.EnumerateBlocks().ToList();
        Console.WriteLine(string.Join(System.Environment.NewLine, blocks));
        Console.WriteLine("2----------------------");
        Assert.AreEqual(4, blocks.Count);
        for (var i = 0; i < 3; i++) {
            Assert.AreEqual(false, blocks[i].IsFree);
            Assert.AreEqual(16, blocks[i].Size.ByteCount);
        }
        Assert.AreEqual(true, blocks[3].IsFree);
        var sizeAfter = blocks[3].Size.ByteCount;
        Assert.AreEqual(true, sizeAfter > sizeBefore);

        memory.Free(blocks[1]);
        blocks = memory.EnumerateBlocks().ToList();
        Console.WriteLine(string.Join(System.Environment.NewLine, blocks));
        Console.WriteLine("3----------------------");
        Assert.AreEqual(4, blocks.Count);
        Assert.AreEqual(false, blocks[0].IsFree);
        Assert.AreEqual(true, blocks[1].IsFree);
        Assert.AreEqual(false, blocks[2].IsFree);
        Assert.AreEqual(true, blocks[3].IsFree);

        memory.Free(blocks[0]);
        blocks = memory.EnumerateBlocks().ToList();
        Console.WriteLine(string.Join(System.Environment.NewLine, blocks));
        Console.WriteLine("4----------------------");
        Assert.AreEqual(3, blocks.Count);
        Assert.AreEqual(true, blocks[0].IsFree);
        Assert.AreEqual(false, blocks[1].IsFree);
        Assert.AreEqual(true, blocks[2].IsFree);
    }

}