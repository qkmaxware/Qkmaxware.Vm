namespace Qkmaxware.Vm;

public class ByteValue : VmValue {
    public Byte Value {get; private set;}

    public ByteValue(Byte value) {
        this.Value = value;
    }

    public override void WriteValue(BinaryWriter writer) {
        writer.Write(this.Value);
    }

    public override string ToString() {
        return this.Value.ToString();
    }

    /// <summary>
    /// String representation of the stored value
    /// </summary>
    public override string ValueToString() => this.Value.ToString();
}