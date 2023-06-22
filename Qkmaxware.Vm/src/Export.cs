namespace Qkmaxware.Vm;

/// <summary>
/// An exported labeled anchor to a spot in bytecode
/// </summary>
public class Export : ExternalName {
    public int CodePosition {get; private set;}

    internal Export(string name, int pos) : base(name) {
        this.CodePosition = pos;
    }
}