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
    public int Address {get; private set;}
    /// <summary>
    /// Allocated size of the block
    /// </summary>
    /// <value>block size</value>
    public DataSize Size {get; private set;}

    public AllocatedMemoryBlock(bool free, int addr, int size) {
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
/// Interface for memory which can be accessed by a random index
/// </summary>
public interface IRandomAccessMemory {
    /// <summary>
    /// Reserve or allocate the given number of bytes
    /// </summary>
    /// <returns>starting byte address</returns>
    /// <param name="byteCount">bytes to reserve</param>
    public int Reserve(int byteCount);
    /// <summary>
    /// Deallocate or free the memory reserved at address 
    /// </summary>
    /// <param name="address">address to the start of a reserved block</param>
    public void Free(int address);
    /// <summary>
    /// Block info for the block at the given address
    /// </summary>
    /// <param name="address">block start address</param>
    /// <returns>block info</returns>
    public AllocatedMemoryBlock BlockInfo(int address);
    /// <summary>
    /// Read a byte from memory at the given address
    /// </summary>
    /// <param name="address">address to read byte from</param>
    /// <returns>read byte</returns>
    public byte ReadByte(int address);
    /// <summary>
    /// Read a 32bit word starting from the given address
    /// </summary>
    /// <param name="address">word start address</param>
    /// <returns>word</returns>
    public uint ReadWord32(int address);
    /// <summary>
    /// Write a byte to memory at the given address
    /// </summary>
    /// <param name="address">address to write byte to</param>
    /// <param name="value">value of byte to write</param>
    public void WriteByte(int address, byte value);
    /// <summary>
    /// Write a 32bit word starting from the given address
    /// </summary>
    /// <param name="address">word start address</param>
    /// <param name="value">value to write</param>
    /// <returns>word</returns>
    public void WriteWord32(int address, uint value);
}