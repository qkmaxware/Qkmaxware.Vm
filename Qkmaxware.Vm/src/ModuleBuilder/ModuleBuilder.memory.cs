using System.Reflection.Metadata;
using Qkmaxware.Vm.Instructions;

namespace Qkmaxware.Vm;

/// <summary>
/// Builder to simplify the creation of bytecode modules programmatically
/// </summary>
public partial class ModuleBuilder {
    /// <summary>
    /// Add a constant to the constant pool
    /// </summary>
    /// <param name="data">constant to add</param>
    public MemoryRef AddConstant(byte[] bytes) {
        var memoryIdx = ConstantPoolIndex;
        var offset = nextConstantIndex;
        nextConstantIndex += Memory.BlockHeaderSize.ByteCount + bytes.Length;

        this._constantPool.Add(new HeapObject(bytes));
        return new MemoryRef(memoryIdx, offset);
    }

    /// <summary>
    /// Add a constant to the static pool
    /// </summary>
    /// <param name="data">static to add</param>
    public MemoryRef AddStatic(byte[] bytes) {
        var memoryIdx = StaticPoolIndex;
        var offset = nextStaticIndex;
        nextStaticIndex += Memory.BlockHeaderSize.ByteCount + bytes.Length;

        this._staticPool.Add(new HeapObject(bytes));
        return new MemoryRef(memoryIdx, offset);
    }

    /// <summary>
    /// Create a new arbitrary storage memory
    /// </summary>
    /// <param name="limits">limits of the memory</param>
    /// <param name="mutability">optional mutability (default readonly)</param>
    /// <returns>memory index</returns>
    public int MakeMemory(Limits limits, Mutability mutability = Mutability.ReadOnly) {
        var offset = this._additionalMems.Count;
        this._additionalMems.Add(new MemorySpec(mutability: mutability, limits: limits, initializer: null));

        return this.AdditionalMemoryOffsetIndex + offset;
    }

    /// <summary>
    /// Add an 32bit signed integer constant to the constant pool
    /// </summary>
    /// <param name="value">value to add</param>
    public MemoryRef AddConstantInt(Int32 value) {
        byte[] bytes = BitConverter.GetBytes(value);
        if (!BitConverter.IsLittleEndian)
            Array.Reverse(bytes);
        return AddConstant(bytes);
    }

    /// <summary>
    /// Add an 32bit unsigned integer constant to the constant pool
    /// </summary>
    /// <param name="value">value to add</param>
    public MemoryRef AddConstantUInt(UInt32 value) {
        byte[] bytes = BitConverter.GetBytes(value);
        if (!BitConverter.IsLittleEndian)
            Array.Reverse(bytes);
        return AddConstant(bytes);
    }

    /// <summary>
    /// Add an 32bit floating point constant to the constant pool
    /// </summary>
    /// <param name="value">value to add</param>
    public MemoryRef AddConstantFloat(Single value) {
        byte[] bytes = BitConverter.GetBytes(value);
        if (!BitConverter.IsLittleEndian)
            Array.Reverse(bytes);
        return AddConstant(bytes);
    }   

    /// <summary>
    /// Add a string constant to the constant pool
    /// </summary>
    /// <param name="value">value to add</param>
    public MemoryRef AddConstantAsciiString(string str) {
        byte[] bytes = System.Text.Encoding.ASCII.GetBytes(str);
        return AddConstant(bytes);
    }

    /// <summary>
    /// Add a string constant to the constant pool terminated by the null character
    /// </summary>
    /// <param name="value">value to add</param>
    public MemoryRef AddConstantAsciizString(string str) {
        byte[] bytes = System.Text.Encoding.ASCII.GetBytes(str+ '\0');
        return AddConstant(bytes);
    }

    /// <summary>
    /// Add a string constant to the constant pool
    /// </summary>
    /// <param name="value">value to add</param>
    public MemoryRef AddConstantUtf8String(string str) {
        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(str);
        return AddConstant(bytes);
    }

    /// <summary>
    /// Add a string constant to the constant pool
    /// </summary>
    /// <param name="value">value to add</param>
    public MemoryRef AddConstantUtf32String(string str) {
        byte[] bytes = System.Text.Encoding.UTF32.GetBytes(str);
        return AddConstant(bytes);
    }

    /// <summary>
    /// Add a value to the static pool
    /// </summary>
    /// <param name="value">value to add</param>
    public MemoryRef AddStatic(Operand value) {
        byte[] bytes = BitConverter.GetBytes(value.UInt32);
        if (!BitConverter.IsLittleEndian)
            Array.Reverse(bytes);
        return AddStatic(bytes);
    }

    /// <summary>
    /// Add an 32bit signed integer constant to the static pool
    /// </summary>
    /// <param name="value">value to add</param>
    public MemoryRef AddStaticInt(Int32 value) {
        byte[] bytes = BitConverter.GetBytes(value);
        if (!BitConverter.IsLittleEndian)
            Array.Reverse(bytes);
        return AddStatic(bytes);
    }

    /// <summary>
    /// Add an 32bit unsigned integer constant to the static pool
    /// </summary>
    /// <param name="value">value to add</param>
    public MemoryRef AddStaticUInt(UInt32 value) {
        byte[] bytes = BitConverter.GetBytes(value);
        if (!BitConverter.IsLittleEndian)
            Array.Reverse(bytes);
        return AddStatic(bytes);
    }

    /// <summary>
    /// Add an 32bit floating point constant to the static pool
    /// </summary>
    /// <param name="value">value to add</param>
    public MemoryRef AddStaticFloat(Single value) {
        byte[] bytes = BitConverter.GetBytes(value);
        if (!BitConverter.IsLittleEndian)
            Array.Reverse(bytes);
        return AddStatic(bytes);
    }   

    /// <summary>
    /// Add a string to the static pool
    /// </summary>
    /// <param name="value">value to add</param>
    public MemoryRef AddStaticAsciiString(string str) {
        byte[] bytes = System.Text.Encoding.ASCII.GetBytes(str);
        return AddStatic(bytes);
    }

    /// <summary>
    /// Add a string to the static pool terminated by the null character
    /// </summary>
    /// <param name="value">value to add</param>
    public MemoryRef AddStaticAsciizString(string str) {
        byte[] bytes = System.Text.Encoding.ASCII.GetBytes(str+ '\0');
        return AddStatic(bytes);
    }

    /// <summary>
    /// Add a string constant to the static pool
    /// </summary>
    /// <param name="value">value to add</param>
    public MemoryRef AddStaticUtf8String(string str) {
        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(str);
        return AddStatic(bytes);
    }

    /// <summary>
    /// Add a string constant to the static pool
    /// </summary>
    /// <param name="value">value to add</param>
    public MemoryRef AddStaticUtf32String(string str) {
        byte[] bytes = System.Text.Encoding.UTF32.GetBytes(str);
        return AddStatic(bytes);
    }

    
}