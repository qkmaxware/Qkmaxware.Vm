namespace Qkmaxware.Vm;

/// <summary>
/// A constant in the constant pool
/// </summary>
public abstract class ConstantData {
    /// <summary>
    /// Constant position within the constant pool
    /// </summary>
    /// <value></value>
    public int PoolIndex {get; internal set;}
    
    /// <summary>
    /// Type information for this constant
    /// </summary>
    /// <value>constant type info</value>
    public ConstantInfo TypeInfo {get; private set;}

    public ConstantData(ConstantInfo type) {
        this.TypeInfo = type;
    }

    /// <summary>
    /// Operand to add to the stack when this constant is loaded from memory
    /// </summary>
    /// <returns>stack operand</returns>
    public abstract Operand LoadOperand();

    /// <summary>
    /// Encode this constant to the given writer
    /// </summary>
    /// <param name="writer">writer to encode data to</param>
    public abstract void Encode(BinaryWriter writer);    

    /// <summary>
    /// Print the value of this constant as a string
    /// </summary>
    /// <returns>string representation of the stored value</returns>
    public abstract string ValueToString();
}

/// <summary>
/// Constant composed of primitive data
/// </summary>
public abstract class PrimitiveConstant : ConstantData {
    public PrimitiveConstant(PrimitiveConstantType type) : base(type) {}
}

/// <summary>
/// 32bit signed integer constant
/// </summary>
public class Int32Constant : PrimitiveConstant {
    /// <summary>
    /// Stored constant value
    /// </summary>
    /// <value></value>
    public Int32 Value {get; private set;}
    
    public Int32Constant(Int32 value) : base(ConstantInfo.Int32) {
        this.Value = value;
    }

    /// <summary>
    /// Operand to add to the stack when this constant is loaded from memory
    /// </summary>
    /// <returns>stack operand</returns>
    public override Operand LoadOperand() => new Int32Operand(this.Value);

    /// <summary>
    /// Encode this constant to the given writer
    /// </summary>
    /// <param name="writer">writer to encode data to</param>
    public override void Encode(BinaryWriter writer) {
        writer.Write(this.Value);
    }

    /// <summary>
    /// Print the value of this constant as a string
    /// </summary>
    /// <returns>string representation of the stored value</returns>
    public override string ValueToString() => this.Value.ToString();
}

/// <summary>
/// 32bit unsigned integer constant
/// </summary>
public class UInt32Constant : PrimitiveConstant {
    /// <summary>
    /// Stored constant value
    /// </summary>
    /// <value></value>
    public UInt32 Value {get; private set;}
    
    public UInt32Constant(UInt32 value) : base(ConstantInfo.UInt32) {
        this.Value = value;
    }

    /// <summary>
    /// Operand to add to the stack when this constant is loaded from memory
    /// </summary>
    /// <returns>stack operand</returns>
    public override Operand LoadOperand() => new UInt32Operand(this.Value);

    /// <summary>
    /// Encode this constant to the given writer
    /// </summary>
    /// <param name="writer">writer to encode data to</param>
    public override void Encode(BinaryWriter writer) {
        writer.Write(this.Value);
    }

    /// <summary>
    /// Print the value of this constant as a string
    /// </summary>
    /// <returns>string representation of the stored value</returns>
    public override string ValueToString() => this.Value.ToString();
}

/// <summary>
/// 32bit floating point number constant
/// </summary>
public class Float32Constant : PrimitiveConstant {
    /// <summary>
    /// Stored constant value
    /// </summary>
    /// <value></value>
    public Single Value {get; private set;}
    
    public Float32Constant(Single value) : base(ConstantInfo.Float32) {
        this.Value = value;
    }

    /// <summary>
    /// Operand to add to the stack when this constant is loaded from memory
    /// </summary>
    /// <returns>stack operand</returns>
    public override Operand LoadOperand() => new Float32Operand(this.Value);

    /// <summary>
    /// Encode this constant to the given writer
    /// </summary>
    /// <param name="writer">writer to encode data to</param>
    public override void Encode(BinaryWriter writer) {
        writer.Write(this.Value);
    }

    /// <summary>
    /// Print the value of this constant as a string
    /// </summary>
    /// <returns>string representation of the stored value</returns>
    public override string ValueToString() => this.Value.ToString();
}

/// <summary>
/// Constant array
/// </summary>
public abstract class ArrayConstant : ConstantData {
    public ArrayConstant(ArrayConstantType type) : base(type) {}

    public abstract int CountBytes();
    public abstract int CountElements();
    public abstract Operand GetElementAt(int index);
}

/// <summary>
/// Constant array of primitives
/// </summary>
/// <typeparam name="T"></typeparam>
public class PrimitiveArrayConstant : ArrayConstant  {

    private PrimitiveConstant[] operands;

    public PrimitiveArrayConstant(ArrayConstantType type, PrimitiveConstant[] operands) : base(type) {
        this.operands = operands;
    }

    public override void Encode(BinaryWriter writer) {
        // First write the type of the stored elements
        writer.Write(((ArrayConstantType)this.TypeInfo).ElementType.TypeTag);
        // Then indicate how many things there are
        writer.Write(this.operands.Length);
        // Then write the things themselves
        foreach (var operand in operands) {
            operand.Encode(writer);
        }
    }

    public override Operand LoadOperand() => new ConstantPoolArrayPointerOperand(this);
    public override int CountBytes() {
        return this.CountElements() * ((ArrayConstantType)this.TypeInfo).ElementType.SizeBytes();
    }

    public override int CountElements() => this.operands.Length;

    public override Operand GetElementAt(int index) => this.operands[index].LoadOperand();

    /// <summary>
    /// Print the value of this constant as a string
    /// </summary>
    /// <returns>string representation of the stored value</returns>
    public override string ValueToString() => "[" + string.Join(',', this.operands.Select(x => x.ValueToString())) + "]";
}

/// <summary>
/// 32bit floating point number constant
/// </summary>
public class StringConstant : ArrayConstant {
    /// <summary>
    /// Stored constant value
    /// </summary>
    /// <value></value>
    public string Value {get; private set;}
    
    public StringConstant(StringConstantType stringType, string value) : base(stringType) {
        this.Value = value;
        this.bytes = stringType.Encoding.GetByteCount(value);
    }

    /// <summary>
    /// Operand to add to the stack when this constant is loaded from memory
    /// </summary>
    /// <returns>stack operand</returns>
    public override Operand LoadOperand() => new ConstantPoolArrayPointerOperand(this);

    /// <summary>
    /// Encode this constant to the given writer
    /// </summary>
    /// <param name="writer">writer to encode data to</param>
    public override void Encode(BinaryWriter writer) {
        var bytes = ((StringConstantType)this.TypeInfo).Encoding.GetBytes(this.Value);
        var length = bytes.Length;
        writer.Write(length);
        writer.Write(bytes);
    }


    private int bytes;
    public override int CountBytes() => bytes;

    public override int CountElements() => this.Value.Length;

    public override Operand GetElementAt(int index) => new Int32Operand(this.Value[index]);

    /// <summary>
    /// Print the value of this constant as a string
    /// </summary>
    /// <returns>string representation of the stored value</returns>
    public override string ValueToString() => System.Text.Json.JsonSerializer.Serialize(this.Value);
}