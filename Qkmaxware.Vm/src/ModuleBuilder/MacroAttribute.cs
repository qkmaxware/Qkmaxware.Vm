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
    /// <summary>
    /// Description of what the macro does
    /// </summary>
    /// <value>description</value>
    public string? Description {get; private set;}
    public MacroAttribute(string name, string? description = null) {
        this.Name = name;
        this.Description = description;
    }
}

public enum Extend {
    Zero, Sign
}