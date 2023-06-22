using System.Text;

namespace Qkmaxware.Vm;

/// <summary>
/// Reference to a block of allocated memory
/// </summary>
public class LinearByteArrayBlock {
    public bool IsFree {get; private set;}
    public uint Address {get; private set;}
    public DataSize Size {get; private set;}

    public LinearByteArrayBlock(bool free, uint addr, int size) {
        this.IsFree = free;
        this.Address = addr;
        this.Size = DataSize.Bytes(size);
    }

    public override string ToString() {
        var status = IsFree ? "FREE" : "RESERVED";
        return $"{status} 0x{this.Address:X}({Size})";
    }
}

/// <summary>
/// Memory organized as a linear array of bytes
/// </summary>
public class LinearByteArrayMemory : IRandomAccessMemory {

    public DataSize Size {get; private set;}
    private byte[] buffer;
    private BinaryReader reader;
    private BinaryWriter writer;

    public static readonly LinearByteArrayMemory Zero = new LinearByteArrayMemory(DataSize.Bytes(0));

    public LinearByteArrayMemory(DataSize heapSize) {
        this.Size = heapSize;
        this.buffer = new byte[this.Size.ByteCount];
        var stream = new MemoryStream(buffer);
        this.reader = new BinaryReader(stream);
        this.writer = new BinaryWriter(stream);

        writer.Write((byte)0x0); // Free Tag
        writer.Write(buffer.Length - 5);
    }

    public uint Reserve(int byteCount) {
        for (long i = 0; i < buffer.Length; i++) {
            reader.BaseStream.Position = i;
            var tag = reader.ReadByte();
            var size = reader.ReadInt32();

            //Console.WriteLine("At 0x" + i.ToString("X") + " " + size + "bytes are " + (tag == 0x0 ? "free" : "reserved"));

            // Load tag
            if (tag == 0x0) {
                // Free space
                if (size > (byteCount + 5)) {
                    // Consume this space
                    writer.BaseStream.Position = i;
                    writer.Write((byte)0x1);
                    writer.Write(byteCount);

                    // Create the empty block
                    writer.BaseStream.Position = i + byteCount + 5;
                    writer.Write((byte)0x0);
                    writer.Write(size - byteCount);

                    return (uint)i;
                } 
                else if (size >= byteCount) {
                    // Consume this space
                    writer.BaseStream.Position = i;
                    writer.Write((byte)0x1);
                    writer.Write(size);

                    return (uint)i;
                }
                else {
                    // Skip this space
                    i = reader.BaseStream.Position + size - 1;
                }
            } else  {
                // Consumed space
                i = reader.BaseStream.Position + size - 1;
            }
        }

        throw new OutOfMemoryException();
    }

    public void Free(LinearByteArrayBlock block) {
        Free(block.Address);
    }

    public void Free(uint address) {
        writer.BaseStream.Position = address;
        // Clear the tag (free memory)
        writer.Write((byte)0x0);
        var size = reader.ReadInt32();

        // Check the next block if it exists
        reader.BaseStream.Position += size;
        if (reader.BaseStream.Position < this.buffer.Length) {
            var next_block_tag = reader.ReadByte();
            if (next_block_tag == 0x0) {
                // Next block is also free, merge blocks
                const int next_block_header_size = 5;
                var next_block_size = reader.ReadInt32();
                writer.BaseStream.Position = address;
                writer.Write((byte)0x0);
                writer.Write(size + next_block_size + next_block_header_size);
            }
        }
    }

    public IEnumerable<LinearByteArrayBlock> EnumerateBlocks() {
        for (long i = 0; i < buffer.Length; i++) {
            reader.BaseStream.Position = i;
            var tag = reader.ReadByte();
            var size = reader.ReadInt32();

            //Console.WriteLine("At 0x" + i.ToString("X") + " " + size + "bytes are " + (tag == 0x0 ? "free" : "reserved"));
            //Console.WriteLine("POSITION " + reader.BaseStream.Position);
            yield return new LinearByteArrayBlock(
                free: tag == 0x0,
                addr: (uint)i,
                size: size
            );

            i = reader.BaseStream.Position + size - 1;
        }
    }

    public byte ReadByte(uint address) {
        return buffer[address];
    }

    public void WriteByte(uint address, byte value) {
        buffer[address] = value;
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