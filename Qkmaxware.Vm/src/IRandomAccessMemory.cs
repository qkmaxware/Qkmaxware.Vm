namespace Qkmaxware.Vm;

/// <summary>
/// Reference to a block of allocated memory
/// </summary>
public class AllocatedMemoryBlock {
    /// <summary>
    /// Test if this block of memory is reserved or not
    /// </summary>
    /// <value>true if block is not reserved</value>
    public bool IsFree {get; private set;}
    /// <summary>
    /// Block start address
    /// </summary>
    /// <value>address</value>
    public int BlockAddress {get; private set;}
    public int DataAddress {get; private set;}
    /// <summary>
    /// Allocated size of the block
    /// </summary>
    /// <value>block size</value>
    public DataSize Size {get; private set;}

    public AllocatedMemoryBlock(bool free, int startAt, int dataStartAt, int size) {
        this.IsFree = free;
        this.BlockAddress = startAt;
        this.DataAddress = dataStartAt;
        this.Size = DataSize.Bytes(size);
    }

    public override string ToString() {
        var status = IsFree ? "FREE" : "RESERVED";
        return $"{status} 0x{this.BlockAddress:X}({Size})";
    }
}

/// <summary>
/// Interface for memory which can be accessed by a random index
/// </summary>
public interface IRandomAccessMemory {
    /// <summary>
    /// Total size of the memory
    /// </summary>
    /// <value>data size</value>
    public DataSize Size {get;}
    /// <summary>
    /// Get bytes for this memory 
    /// </summary>
    /// <returns>bytes</returns>
    public byte[] GetBytes();
    /// <summary>
    /// Read a byte from memory at the given address
    /// </summary>
    /// <param name="address">address to read byte from</param>
    /// <returns>read byte</returns>
    public byte ReadByte(int address);
    /// <summary>
    /// Write a byte to memory at the given address
    /// </summary>
    /// <param name="address">address to write byte to</param>
    /// <param name="value">value of byte to write</param>
    public void WriteByte(int address, byte value);
}