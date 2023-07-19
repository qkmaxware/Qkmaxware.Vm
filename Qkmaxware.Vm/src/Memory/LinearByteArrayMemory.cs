using System.Text;

namespace Qkmaxware.Vm;

/// <summary>
/// Memory organized as a linear array of bytes
/// </summary>
public class LinearByteArrayMemory : IRandomAccessMemory {

    public DataSize Size {get; private set;}
    /// <summary>
    /// Size of each data block's header if applicable
    /// </summary>
    public DataSize BlockHeaderSize {get;}
    private byte[] buffer;
    private BinaryReader reader;
    private BinaryWriter writer;

    public static readonly LinearByteArrayMemory Zero = new LinearByteArrayMemory(DataSize.Bytes(0));

    public LinearByteArrayMemory(DataSize heapSize) {
        this.Size = heapSize; 
        this.BlockHeaderSize = DataSize.Bytes(sizeof(byte) + sizeof(int)); // At least 5 bytes for the header
        this.buffer = new byte[Math.Max(this.Size.ByteCount, BlockHeaderSize.ByteCount)];
        var stream = new MemoryStream(buffer);
        this.reader = new BinaryReader(stream);
        this.writer = new BinaryWriter(stream);

        writer.Write((byte)0x0); // Free Tag
        writer.Write(buffer.Length - BlockHeaderSize.ByteCount);
    }

    public int Reserve(int byteCount) {
        for (int i = 0; i < buffer.Length; i++) {
            reader.BaseStream.Position = i;
            var tag = reader.ReadByte();
            var size = reader.ReadInt32();

            // Load tag
            if (tag == 0x0) {
                // Free space
                if (size > (byteCount + BlockHeaderSize.ByteCount)) {
                    // Consume this space
                    writer.BaseStream.Position = i;
                    writer.Write((byte)0x1);
                    writer.Write(byteCount);

                    // Create the empty block at the end of this space
                    writer.BaseStream.Position += byteCount;
                    writer.Write((byte)0x0);
                    writer.Write(size - byteCount - BlockHeaderSize.ByteCount);

                    return i;
                } 
                else if (size >= byteCount) {
                    // Consume this space
                    writer.BaseStream.Position = i;
                    writer.Write((byte)0x1);
                    writer.Write(size);

                    return i;
                }
                else {
                    // Skip this space
                    i = (int)reader.BaseStream.Position + size - 1;
                }
            } else  {
                // Consumed space
                i = (int)reader.BaseStream.Position + size - 1;
            }
        }

        throw new OutOfMemoryException();
    }

    public void Free(AllocatedMemoryBlock block) {
        Free(block.Address);
    }

    public void Free(int address) {
        AllocatedMemoryBlock? prev = null;
        AllocatedMemoryBlock? current = null;
        AllocatedMemoryBlock? next = null;
        foreach (var block in new HeapBlockIterator(this)) {
            if (block.Address == address) {
                current = block;
            } else if (current == null) {
                prev = block;
            } else if (current != null) {
                next = block;
                break;
            }
        }
        if (current == null)
            return;

        // Compute the size of the new free'd area
        AllocatedMemoryBlock startAt = current;
        DataSize freeSpace = current.Size;
        if (prev != null && prev.IsFree) {
            startAt = prev;
            freeSpace += prev.Size + BlockHeaderSize; // Eat the header of the current block;
        }
        if (next != null && next.IsFree) {
            freeSpace += next.Size + BlockHeaderSize; // Eat the next block and it's header
        }

        // Clear the tag (free memory)
        writer.BaseStream.Position = startAt.Address;
        writer.Write((byte)0x0);
        writer.Write(freeSpace.ByteCount); // off by 30 bytes
    }

    /// <summary>
    /// Block info for the block at the given address
    /// </summary>
    /// <param name="address">block start address</param>
    /// <returns>block info</returns>
    public AllocatedMemoryBlock BlockInfo(int address) {
        reader.BaseStream.Position = address;
        var tag = reader.ReadByte();
        var size = reader.ReadInt32();
        return new AllocatedMemoryBlock(
            free: tag == 0x0,
            addr: address,
            size: size
        );
    }

    public byte ReadByte(int address) {
        return buffer[address];
    }

    /// <summary>
    /// Read a 32bit word starting from the given address
    /// </summary>
    /// <param name="address">word start address</param>
    /// <returns>word</returns>
    public uint ReadWord32(int address) {
        this.reader.BaseStream.Position = address + BlockHeaderSize.ByteCount; // 5 bytes skip block header
        return reader.ReadUInt32();
    }

    public void WriteByte(int address, byte value) {
        buffer[address] = value;
    }

    /// <summary>
    /// Write a 32bit word starting from the given address
    /// </summary>
    /// <param name="address">word start address</param>
    /// <param name="value">value to write</param>
    /// <returns>word</returns>
    public void WriteWord32(int address, uint value) {
        this.writer.BaseStream.Position = address + BlockHeaderSize.ByteCount;  // 5 bytes skip block header
        writer.Write(value);
    }

    public override string ToString() {
        var writer = new StringBuilder();
        writer.Append("[");
        for (var i = 0; i < buffer.Length; i++) {
            writer.Append("0x");
            writer.Append(buffer[i].ToString("X"));
            writer.Append(", ");
        }
        writer.Append("]");
        return writer.ToString();
    }
}