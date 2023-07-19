using System.Collections;

namespace Qkmaxware.Vm;

/// <summary>
/// An iterator for allocated blocks in any heap implementation
/// </summary>
public class HeapBlockIterator : IEnumerable<AllocatedMemoryBlock> {
    public IRandomAccessMemory Heap {get; private set;}
    public HeapBlockIterator(IRandomAccessMemory memory) {
        this.Heap = memory;
    }

    public IEnumerator<AllocatedMemoryBlock> GetEnumerator() {
        for (int byte_index = 0; byte_index < Heap.Size.ByteCount; ) {
            AllocatedMemoryBlock info = Heap.BlockInfo(byte_index);

            yield return info;

            byte_index = byte_index + Heap.BlockHeaderSize.ByteCount + info.Size.ByteCount;
        }
    }

    IEnumerator IEnumerable.GetEnumerator() {
        return this.GetEnumerator();
    }
}