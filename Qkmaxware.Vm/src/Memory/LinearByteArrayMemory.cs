using System.Text;

namespace Qkmaxware.Vm;

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
        this.buffer = new byte[Math.Max(this.Size.ByteCount, 5)];// At least 5 bytes for the header
        var stream = new MemoryStream(buffer);
        this.reader = new BinaryReader(stream);
        this.writer = new BinaryWriter(stream);

        writer.Write((byte)0x0); // Free Tag
        writer.Write(buffer.Length - 5);
    }

    public int Reserve(int byteCount) {
        for (int i = 0; i < buffer.Length; i++) {
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

    public IEnumerable<AllocatedMemoryBlock> EnumerateBlocks() {
        for (int i = 0; i < buffer.Length; i++) {
            reader.BaseStream.Position = i;
            var tag = reader.ReadByte();
            var size = reader.ReadInt32();

            yield return BlockInfo(i);

            i = (int)reader.BaseStream.Position + size - 1;
        }
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
        this.reader.BaseStream.Position = address + 5; // 5 bytes skip block header
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
        this.writer.BaseStream.Position = address + 5;  // 5 bytes skip block header
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