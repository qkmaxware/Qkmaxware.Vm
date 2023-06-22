namespace Qkmaxware.Vm;

/// <summary>
/// Constant pool constant storage information
/// </summary>
public abstract class ConstantInfo {
    
    public static readonly Int32ConstantType Int32 = new Int32ConstantType();
    public static readonly UInt32ConstantType UInt32 = new UInt32ConstantType();
    public static readonly Float32ConstantType Float32 = new Float32ConstantType();

    public static readonly ArrayConstantType Int32Array = new ArrayConstantType(Int32);
    public static readonly ArrayConstantType UInt32Array = new ArrayConstantType(UInt32);
    public static readonly ArrayConstantType Float32Array = new ArrayConstantType(Float32);

    public static readonly AsciiConstantType Ascii = new AsciiConstantType();
    public static readonly Utf8ConstantType Utf8 = new Utf8ConstantType();
    public static readonly Utf32ConstantType Utf32 = new Utf32ConstantType();
    
    /// <summary>
    /// Unique integer value used to indicate constants of this type
    /// </summary>
    /// <value>tag</value>
    public byte TypeTag {get; private set;}

    public ConstantInfo(byte tag) {
        this.TypeTag = tag;
    }

    /// <summary>
    /// Decode a value of this constant type from the given reader
    /// </summary>
    /// <param name="reader">reader to decode data from</param>
    /// <returns>constant data</returns>
    public abstract ConstantData Decode(BinaryReader reader);
}

/// <summary>
/// Constant type for primitives
/// </summary>
public abstract class PrimitiveConstantType : ConstantInfo {
    public abstract int SizeBytes();

    public PrimitiveConstantType(byte tag) : base(tag) {}
}

/// <summary>
/// Constant type info for 32bit signed integers
/// </summary>
public class Int32ConstantType : PrimitiveConstantType {

    internal Int32ConstantType() : base(0x01) {}

    public override int SizeBytes() => 4;

    public override ConstantData Decode(BinaryReader reader) {
        return new Int32Constant(reader.ReadInt32());
    }
}

/// <summary>
/// Constant type info for 32bit unsigned integers
/// </summary>
public class UInt32ConstantType : PrimitiveConstantType {

    internal UInt32ConstantType() : base(0x02) {}

    public override int SizeBytes() => 4;

    public override ConstantData Decode(BinaryReader reader) {
        return new UInt32Constant(reader.ReadUInt32());
    }
}

/// <summary>
/// Constant type info for 32bit floating point numbers
/// </summary>
public class Float32ConstantType : PrimitiveConstantType {

    internal Float32ConstantType() : base(0x03) {}

    public override int SizeBytes() => 4;

    public override ConstantData Decode(BinaryReader reader) {
        return new Float32Constant(reader.ReadSingle());
    }
}

/// <summary>
/// Type info for an array of elements
/// </summary>
public class ArrayConstantType : ConstantInfo {
    /// <summary>
    /// Type of elements stored in the array
    /// </summary>
    /// <value>type info</value>
    public PrimitiveConstantType ElementType {get; private set;}

    public ArrayConstantType(PrimitiveConstantType elementType) : base(0x09){
        this.ElementType = elementType;
    }

    internal ArrayConstantType(byte tag, PrimitiveConstantType elementType) : base(tag){
        this.ElementType = elementType;
    }

    public override ConstantData Decode(BinaryReader reader) {
        // Get the nested "type" of the elements
        var elementType = reader.ReadByte();
        // Get the number of elements
        var count = reader.ReadInt32();

        // Get the element decoder
        var primitiveTypes = new PrimitiveConstantType[] {
            ConstantInfo.Int32,
            ConstantInfo.UInt32,
            ConstantInfo.Float32,
        };
        var type = primitiveTypes.Where(type => type.TypeTag == elementType).FirstOrDefault();
        if (type == null)   
            throw new ArgumentException("Unable to decode array elements of type " + elementType);
        
        // Decode each element
        var array = new List<PrimitiveConstant>();
        for (var i = 0; i < count; i++) {
            var data = type.Decode(reader);
            array.Add((PrimitiveConstant)data);
        }

        // Create the appropriate constant to return
        return new PrimitiveArrayConstant(new ArrayConstantType(this.TypeTag, type), array.ToArray());
    }
}

/// <summary>
/// Constant type info for encoded strings
/// </summary>
public abstract class StringConstantType : ArrayConstantType {

    public System.Text.Encoding Encoding {get; private set;}

    public StringConstantType(byte tag, System.Text.Encoding encoding) : base(tag, ConstantInfo.Int32) {
        this.Encoding = encoding;
    }

    public override ConstantData Decode(BinaryReader reader) {
        var length = reader.ReadInt32();
        var bytes = reader.ReadBytes(length);
        return new StringConstant(this, this.Encoding.GetString(bytes));
    }
}

/// <summary>
/// Constant type info for ASCII encoded strings
/// </summary>
public class AsciiConstantType : StringConstantType {
    internal AsciiConstantType() : base(0x10, System.Text.Encoding.ASCII) {}
}

/// <summary>
/// Constant type info for UTF8 encoded strings
/// </summary>
public class Utf8ConstantType : StringConstantType {
    internal Utf8ConstantType() : base(0x11, System.Text.Encoding.UTF8) {}
}

/// <summary>
/// Constant type info for UTF32 encoded strings
/// </summary>
public class Utf32ConstantType : StringConstantType {
    internal Utf32ConstantType() : base(0x12, System.Text.Encoding.UTF32) {}
}