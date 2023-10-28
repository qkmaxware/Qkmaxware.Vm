namespace Qkmaxware.Vm;

public class MemoryRef {
    public int MemoryIndex {get; private set;}
    public int Offset {get; private set;}

    public MemoryRef(int memory, int offset) {
        this.MemoryIndex = memory;
        this.Offset = offset;
    }
}

/// <summary>
/// Builder to simplify the creation of bytecode modules programmatically
/// </summary>
public partial class ModuleBuilder : IDisposable {

    class HeapObject {
        public DataSize size {get; private set;}
        public byte[] data {get; private set;}

        public HeapObject(byte[] data) {
            this.size = DataSize.Bytes(data.Length);
            this.data = data;
        }
    }

    private Dictionary<string, long> labels = new Dictionary<string, long>(); 
    private List<HeapObject> _constantPool = new List<HeapObject>();
    private int nextConstantIndex = 0;
    private List<HeapObject> _staticPool = new List<HeapObject>();
    private int nextStaticIndex = 0;
    private List<MemorySpec> _additionalMems = new List<MemorySpec>();

    public int ConstantPoolIndex => 0;
    public int StaticPoolIndex => 1;
    public int AdditionalMemoryOffsetIndex => 2;
    private BinaryWriter bytecode;

    private List<Import> imports = new List<Import>();
    private List<Export> exports = new List<Export>();
    HashSet<string> export_names = new HashSet<string>();

    /// <summary>
    /// Create a new module builder that writes bytecode in-memory
    /// </summary>
    public ModuleBuilder() {
        bytecode = new BinaryWriter(new MemoryStream());
    }

    /// <summary>
    /// Create a new module builder that writes bytecode to the specific stream
    /// </summary>
    /// <param name="bytecodeStream">stream to write bytecode to</param>
    //public ModuleBuilder(Stream bytecodeStream) {
        //bytecode = new BinaryWriter(bytecodeStream);
    //}

    /// <summary>
    /// Dispose of this module builder
    /// </summary>
    public void Dispose() {
        bytecode.Dispose();
    }

    private List<byte> toDataInitializer(List<HeapObject> data) {
        List<byte> bytes = new List<byte>(data.Count * 5);

        foreach (var obj in data) {
            // Write header
            bytes.Add(0x01); // used memory flag
            var size = BitConverter.GetBytes(obj.size.ByteCount); // obj size
            if (!BitConverter.IsLittleEndian)
                Array.Reverse(size);
            foreach (var b in size) {
                bytes.Add(b);
            }

            // Write data
            foreach (var b in obj.data) {
                bytes.Add(b);
            }
        }

        return bytes;
    }

    /// <summary>
    /// Create a module from the given builder
    /// </summary>
    /// <returns>module</returns>
    public Module ToModule() {
        Module mod = new Module();

        mod.Exports.AddRange(this.exports);
        mod.Imports.AddRange(this.imports);
        mod.Code.AddRange(((MemoryStream)this.bytecode.BaseStream).ToArray());
        
        List<byte> data = toDataInitializer(_constantPool);
        mod.Memories.Add(new MemorySpec(
            mutability: Mutability.ReadOnly,
            limits: new Limits {
                Min = data.Count,
                Max = data.Count, // Same size as min
            },
            initializer: data
        ));
        data = toDataInitializer(_staticPool);
        mod.Memories.Add(new MemorySpec(
            mutability: Mutability.ReadWrite,
            limits: new Limits {
                Min = data.Count,
                Max = data.Count, // Same size as min
            },
            initializer: data
        ));
        mod.Memories.AddRange(this._additionalMems);

        return mod;
    }
}