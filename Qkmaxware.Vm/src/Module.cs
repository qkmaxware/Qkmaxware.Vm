namespace Qkmaxware.Vm;

/// <summary>
/// Single bytecode module
/// </summary>
public class Module {
    #region Module Header Properties
    private static readonly byte[] Magic = new byte[]{(byte)'q', (byte)'k', (byte)'b', (byte)'c'};
    public static IEnumerable<byte> MagicNumbers => Array.AsReadOnly(Magic);
    public static int MajorVersion {get; private set;} = 1;
    public static int MinorVersion {get; private set;} = 0;
    #endregion

    #region Module Data
    public int CodeLength => Code.Count;
    public List<byte> Code {get; private set;} = new List<byte>();

    public int ConstantPoolCount => ConstantPool.Count;
    public ConstantPool ConstantPool {get; private set;} = new ConstantPool();
    #endregion

    public void EncodeFile(BinaryWriter writer) {
        // -----------------------------------------------------------
        // Write Header
        // -----------------------------------------------------------
        foreach (var b in Magic) {
            writer.Write(b);
        }
        writer.Write(MajorVersion);
        writer.Write(MinorVersion);

        // -----------------------------------------------------------
        // Write Data
        // -----------------------------------------------------------
        writer.Write(this.CodeLength);
        foreach (var b in this.Code) {
            writer.Write(b);
        }

        writer.Write(this.ConstantPoolCount);
        foreach (var data in this.ConstantPool) {
            // Write constant info header
            writer.Write(data.TypeInfo.TypeTag);

            // Write constant data
            data.Encode(writer);
        }
    }
}