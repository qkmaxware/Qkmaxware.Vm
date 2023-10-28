namespace Qkmaxware.Vm;

/// <summary>
/// Single bytecode module
/// </summary>
public class Module {
    #region Module Header Properties
    private static readonly byte[] Magic = new byte[]{(byte)'q', (byte)'k', (byte)'b', (byte)'c'};
    public static IEnumerable<byte> MagicNumbers => Array.AsReadOnly(Magic);
    public static int MajorVersion {get; private set;} = 2;
    public static int MinorVersion {get; private set;} = 0;
    #endregion

    #region Module Data
    public int ExportCount => Exports.Count;
    public List<Export> Exports {get; private set;} = new List<Export>();

    public int ImportCount => Imports.Count;
    public List<Import> Imports {get; private set;} = new List<Import>();

    public int CodeLength => Code.Count;
    public List<byte> Code {get; private set;} = new List<byte>();

    public int MemoryCount => Memories.Count;
    public List<MemorySpec> Memories {get; private set;} = new List<MemorySpec>();
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
        writer.Write(this.ExportCount);
        foreach (var e in this.Exports) {
            // Write string
            var bytes = System.Text.Encoding.UTF8.GetBytes(e.Name);
            writer.Write(bytes.Length);     // Length
            foreach (var b in bytes) {
                writer.Write(b);            // Bytes
            }
            // Write anchor
            writer.Write(e.CodePosition);   // Anchor
        }

        writer.Write(this.ImportCount);
        foreach (var i in this.Imports) {
            // Write string
            var bytes = System.Text.Encoding.UTF8.GetBytes(i.Name);
            writer.Write(bytes.Length);     // Length
            foreach (var b in bytes) {
                writer.Write(b);            // Bytes
            }
        }

        writer.Write(this.CodeLength);
        foreach (var b in this.Code) {
            writer.Write(b);
        }

        writer.Write(this.MemoryCount);
        foreach (var memory in this.Memories) {
            // Write constant data
            memory.Encode(writer);
        }
    }
}