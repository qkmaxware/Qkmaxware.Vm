namespace Qkmaxware.Vm;

/// <summary>
/// Base class for instruction operands
/// </summary>
public class Operand : VmValue {
    private System.UInt32 value;

    public System.UInt32 UInt32 => value;
    public System.Int32 Int32 => BitConverter.ToInt32(BitConverter.GetBytes(value));
    public System.Single Float32 => BitConverter.ToSingle(BitConverter.GetBytes(value));
    public Pointer Pointer32 => new Pointer(value);

    private Operand(System.UInt32 value) {
        this.value = value;
    }

    public static Operand From(Pointer value) {
        return new Operand(value.UIntValue);
    }
    public static Operand From(System.UInt32 value) {
        return new Operand(value);
    }
    public static implicit operator Operand (System.UInt32 value) => From(value);
    public static Operand From(Int32 value) {
        return From(BitConverter.ToUInt32(BitConverter.GetBytes(value)));
    }
    public static implicit operator Operand (System.Int32 value) => From(value);
    public static Operand From(Single value) {
        return From(BitConverter.ToUInt32(BitConverter.GetBytes(value)));
    }
    public static implicit operator Operand (System.Single value) => From(value);
    

    public override void WriteValue(BinaryWriter writer) {
        writer.Write(this.value);
    }

    public override string ValueToString() {
        return "0";
    }
}
