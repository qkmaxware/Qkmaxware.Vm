namespace Qkmaxware.Vm;

/// <summary>
/// Base class for types within the virtual machine
/// </summary>
public abstract class VmType {

    public static readonly Int32Type Int32 = new Int32Type();
    public static readonly UInt32Type UInt32 = new UInt32Type();
    public static readonly Float32Type Float32 = new Float32Type();

    /// <summary>
    /// Size, in bytes, for values of this type
    /// </summary>
    /// <value>size</value>
    public abstract int SizeBytes {get;}
}

/// <summary>
/// Base class for types within the virtual machine representable by C# primitives
/// </summary>
public abstract class VmType<TUnderlying> : VmType {
    public abstract TUnderlying MinValue {get;}
    public abstract TUnderlying MaxValue {get;}
    public abstract TUnderlying DefaultValue {get;}
}

/// <summary>
/// 32 bit signed integer type
/// </summary>
public class Int32Type : VmType<Int32> {

    internal Int32Type() {}

    public override Int32 MinValue => Int32.MinValue;
    public override Int32 MaxValue => Int32.MaxValue;
    public override Int32 DefaultValue => default(Int32);
    public override Int32 SizeBytes => 4;
}

/// <summary>
/// 32 bit unsigned integer type
/// </summary>
public class UInt32Type : VmType<UInt32> {

    internal UInt32Type() {}

    public override UInt32 MinValue => UInt32.MinValue;
    public override UInt32 MaxValue => UInt32.MaxValue;
    public override UInt32 DefaultValue => default(UInt32);
    public override Int32 SizeBytes => 4;
}

/// <summary>
/// 32 bit floating-point number type
/// </summary>
public class Float32Type : VmType<Single> {

    internal Float32Type() {}

    public override Single MinValue => Single.MinValue;
    public override Single MaxValue => Single.MaxValue;
    public override Single DefaultValue => default(Single);
    public override Int32 SizeBytes => 4;
}