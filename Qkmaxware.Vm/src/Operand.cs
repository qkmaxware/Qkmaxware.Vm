namespace Qkmaxware.Vm;

/// <summary>
/// Base class for instruction operands
/// </summary>
public abstract class Operand : VmValue {
    /// <summary>
    /// Type information of the operand
    /// </summary>
    /// <value>type information</value>
    public abstract VmType Type {get;} 
}

/// <summary>
/// Base class for operands storing a value
/// </summary>
/// <typeparam name="T">stored value type</typeparam>
public abstract class Operand<T> : Operand {
    public T Value {get; private set;}

    public Operand(T value) {
        this.Value = value;
    }

    public override string? ToString() {
        return this.Value?.ToString() ?? base.ToString();
    }
}

/// <summary>
/// 32bit signed integer operand
/// </summary>
public class Int32Operand : Operand<Int32> {
    public override VmType Type => VmType.Int32;
    public Int32Operand(Int32 value) : base(value) {}
    public override void WriteValue(BinaryWriter writer) {
        writer.Write(this.Value);
    }

    /// <summary>
    /// String representation of the stored value
    /// </summary>
    public override string ValueToString() => this.Value.ToString();
}

/// <summary>
/// 32bit unsigned integer operand
/// </summary>
public class UInt32Operand : Operand<UInt32> {
    public override VmType Type => VmType.UInt32;
    public UInt32Operand(UInt32 value) : base(value) {}

    public override void WriteValue(BinaryWriter writer) {
        writer.Write(this.Value);
    }

    /// <summary>
    /// String representation of the stored value
    /// </summary>
    public override string ValueToString() => this.Value.ToString();
}

/// <summary>
/// 32bit floating-point number operand
/// </summary>
public class Float32Operand : Operand<Single> {
    public override VmType Type => VmType.Float32;
    public Float32Operand(Single value) : base(value) {}

    public override void WriteValue(BinaryWriter writer) {
        writer.Write(this.Value);
    }

    /// <summary>
    /// String representation of the stored value
    /// </summary>
    public override string ValueToString() => this.Value.ToString();
}

/// <summary>
/// Pointer to some value in saved data
/// </summary>
public abstract class PointerOperand : Operand {}

/// <summary>
/// Pointer to a compound data object
/// </summary>
public abstract class CompoundDataPointerOperand : PointerOperand {
    /// <summary>
    /// Number of bytes used by this compound data type
    /// </summary>
    /// <returns>byte count</returns>
    public abstract int CountBytes();
}

/// <summary>
/// A pointer to arbitrary memory on the heap
/// </summary>
public class HeapDataPointerOperand : CompoundDataPointerOperand {
    public uint Address {get; private set;}

    public override VmType Type => throw new NotImplementedException();

    private IRandomAccessMemory heap;

    public HeapDataPointerOperand(uint heap_address, IRandomAccessMemory source) {
        this.Address = heap_address;
        this.heap = source;
    }

    /// <summary>
    /// Number of bytes used by this compound data type
    /// </summary>
    /// <returns>byte count</returns>
    public override int CountBytes() {
        var bytes = new byte[] {
            heap.ReadByte(Address + 1),
            heap.ReadByte(Address + 2),
            heap.ReadByte(Address + 3),
            heap.ReadByte(Address + 4)
        };
        using (var reader = new BinaryReader(new MemoryStream(bytes))) {
            return reader.ReadInt32();
        }
    }

    public override void WriteValue(BinaryWriter writer) {
        throw new NotImplementedException();
    }

    public override string ValueToString() => "heap::" + this.Address;
}

/// <summary>
/// Pointer to an array 
/// </summary>
public abstract class ArrayPointerOperand : CompoundDataPointerOperand {
    /// <summary>
    /// Number of elements in this array
    /// </summary>
    /// <returns>count of elements</returns>
    public abstract int CountElements();

    /// <summary>
    /// Element at a given offset in the array
    /// </summary>
    /// <param name="index">offset index</param>
    /// <returns>element at index</returns>
    public abstract Operand GetElementAt(int index);
}

/// <summary>
/// Pointer to some value in the constant pool
/// </summary>
public class ConstantPoolArrayPointerOperand : ArrayPointerOperand {

    private ArrayConstant constant;

    public override VmType Type => throw new NotImplementedException();

    public ConstantPoolArrayPointerOperand(ArrayConstant constant) {
        this.constant = constant;
    }

    public override void WriteValue(BinaryWriter writer) {
        throw new NotImplementedException();
    }

    public override int CountElements() {
        return constant.CountElements();
    }

    public override int CountBytes() {
        return constant.CountBytes();
    }

    public override Operand GetElementAt(int index) {
        return constant.GetElementAt(index);
    }

    /// <summary>
    /// String representation of the stored value
    /// </summary>
    public override string ValueToString() => "constant::" + this.constant.PoolIndex;
}