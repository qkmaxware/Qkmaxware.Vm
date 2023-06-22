namespace Qkmaxware.Vm;

/// <summary>
/// Builder to simplify the creation of bytecode modules programmatically
/// </summary>
public partial class ModuleBuilder : IDisposable {

    private Dictionary<string, long> labels = new Dictionary<string, long>(); 
    private List<ConstantData> constants = new List<ConstantData>();
    private BinaryWriter bytecode;

    private List<Import> imports = new List<Import>();
    private List<Export> exports = new List<Export>();
    HashSet<string> export_names = new HashSet<string>();

    public ModuleBuilder() {
        bytecode = new BinaryWriter(new MemoryStream());
    }

    public void Dispose() {
        bytecode.Dispose();
    }

    public Module ToModule() {
        Module mod = new Module();

        mod.Exports.AddRange(this.exports);
        mod.Imports.AddRange(this.imports);
        mod.Code.AddRange(((MemoryStream)this.bytecode.BaseStream).ToArray());
        mod.ConstantPool.AddRange(this.constants);

        return mod;
    }
}