namespace Qkmaxware.Vm;

/// <summary>
/// Interface for memory which can be accessed by a random index
/// </summary>
public interface IRandomAccessMemory {
    /// <summary>
    /// Reserve or allocate the given number of bytes
    /// </summary>
    /// <returns>starting byte address</returns>
    /// <param name="byteCount">bytes to reserve</param>
    public uint Reserve(int byteCount);
    /// <summary>
    /// Deallocate or free the memory reserved at address 
    /// </summary>
    /// <param name="address">address to the start of a reserved block</param>
    public void Free(uint address);
    /// <summary>
    /// Read a byte from memory at the given address
    /// </summary>
    /// <param name="address">address to read byte from</param>
    /// <returns>read byte</returns>
    public byte ReadByte(uint address);
    /// <summary>
    /// Write a byte to memory at the given address
    /// </summary>
    /// <param name="address">address to write byte to</param>
    /// <param name="value">value of byte to write</param>
    public void WriteByte(uint address, byte value);
}