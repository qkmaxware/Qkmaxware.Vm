namespace Qkmaxware.Vm;

/// <summary>
/// A flag to indicate that a given method is a macro
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class MacroAttribute : Attribute {
    /// <summary>
    /// Name of the macro
    /// </summary>
    /// <value>name</value>
    public string Name {get; private set;}
    
    public MacroAttribute(string name) {
        this.Name = name;
    }
}