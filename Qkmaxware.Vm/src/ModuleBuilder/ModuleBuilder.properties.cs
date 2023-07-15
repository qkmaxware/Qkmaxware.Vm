namespace Qkmaxware.Vm;

/// <summary>
/// Builder to simplify the creation of bytecode modules programmatically
/// </summary>
public partial class ModuleBuilder : IDisposable {

    private Dictionary<string, long> labels = new Dictionary<string, long>(); 
    private List<ConstantData> constants = new List<ConstantData>();
    private List<Operand> statics = new List<Operand>();
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

    /// <summary>
    /// Create a module from the given builder
    /// </summary>
    /// <returns>module</returns>
    public Module ToModule() {
        Module mod = new Module();

        mod.Exports.AddRange(this.exports);
        mod.Imports.AddRange(this.imports);
        mod.Code.AddRange(((MemoryStream)this.bytecode.BaseStream).ToArray());
        mod.ConstantPool.AddRange(this.constants);
        mod.StaticPool.AddRange(this.statics);

        return mod;
    }
}