using System.Collections;

namespace Qkmaxware.Vm;

/// <summary>
/// An iterator for allocated blocks in any heap implementation
/// </summary>
public class HeapBlockIterator : IEnumerable<AllocatedMemoryBlock> {
    public Memory Heap {get; private set;}
    public HeapBlockIterator(Memory memory) {
        this.Heap = memory;
    }

    public IEnumerator<AllocatedMemoryBlock> GetEnumerator() {
        for (int byte_index = 0; byte_index < Heap.CurrentSize.ByteCount;) {
            AllocatedMemoryBlock info = Heap.GetBlockInfo(byte_index);

            yield return info;

            byte_index = byte_index + Memory.BlockHeaderSize.ByteCount + info.Size.ByteCount;
        }
    }

    IEnumerator IEnumerable.GetEnumerator() {
        return this.GetEnumerator();
    }
}