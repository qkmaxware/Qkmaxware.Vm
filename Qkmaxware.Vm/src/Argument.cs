namespace Qkmaxware.Vm;

/// <summary>
/// Base class for instruction arguments
/// </summary>
public abstract class Argument {
    /// <summary>
    /// "Human-readable" Argument name for printing
    /// </summary>
    /// <value>name</value>
    public string Name {get; private set;}

    public Argument(string name) {
        this.Name = name;
    }

    /// <summary>
    /// Read the value of this argument from the given binary stream
    /// </summary>
    /// <param name="reader">reader</param>
    /// <returns>argument value</returns>
    public abstract VmValue ReadValue(BinaryReader reader);
}

/// <summary>
/// Argument composed of a single byte
/// </summary>
public class ByteArgument : Argument {

    public ByteArgument(string name) : base(name) {}

    public override VmValue ReadValue(BinaryReader reader) {
        return new ByteValue(reader.ReadByte());
    }
}

/// <summary>
/// Argument composed of a 4 bytes as a signed integer
/// </summary>
public class Int32Argument : Argument {

    public Int32Argument(string name) : base(name) {}

    public override VmValue ReadValue(BinaryReader reader) {
        return new Int32Operand(reader.ReadInt32());
    }
}

/// <summary>
/// Argument composed of a 4 bytes as an unsigned integer
/// </summary>
public class UInt32Argument : Argument {

    public UInt32Argument(string name) : base(name) {}

    public override VmValue ReadValue(BinaryReader reader) {
        return new UInt32Operand(reader.ReadUInt32());
    }
}

/// <summary>
/// Argument composed of a 4 bytes as a floating-point number
/// </summary>
public class Float32Argument : Argument {

    public Float32Argument(string name) : base(name) {}

    public override VmValue ReadValue(BinaryReader reader) {
        return new Float32Operand(reader.ReadSingle());
    }
}