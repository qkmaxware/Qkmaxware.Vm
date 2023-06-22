namespace Qkmaxware.Vm;

public abstract class ExternalName {
    public string Name {get; private set;}
    public ExternalName(string name) {
        this.Name = name;
    }
}

public class Import : ExternalName {
    public Import(string name) : base(name) {}
}