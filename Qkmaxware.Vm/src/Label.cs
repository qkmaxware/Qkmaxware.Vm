namespace Qkmaxware.Vm;

/// <summary>
/// A labeled anchor to a spot in bytecode
/// </summary>
public class Label {
    public string UniqueName {get; private set;}
    public long CodePosition {get; private set;}

    internal Label(string name, long pos) {
        this.UniqueName = name;
        this.CodePosition = pos;
    }
}