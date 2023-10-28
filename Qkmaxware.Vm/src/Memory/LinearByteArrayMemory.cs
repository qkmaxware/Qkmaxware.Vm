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
    public static readonly LinearByteArrayMemory Zero = new LinearByteArrayMemory(DataSize.Bytes(256));

    public LinearByteArrayMemory(DataSize heapSize) {
        this.Size = heapSize; 
        this.BlockHeaderSize = Memory.BlockHeaderSize;
        this.buffer = new byte[Math.Max(this.Size.ByteCount, BlockHeaderSize.ByteCount)];
    }

    /// <summary>
    /// Get bytes for this memory 
    /// </summary>
    /// <returns>bytes</returns>
    public byte[] GetBytes() => buffer;

    public byte ReadByte(int address) {
        return buffer[address];
    }

    public void WriteByte(int address, byte value) {
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