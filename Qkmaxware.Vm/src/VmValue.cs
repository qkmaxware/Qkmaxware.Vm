namespace Qkmaxware.Vm;

/// <summary>
/// Base class for values associated with instructions
/// </summary>
public abstract class VmValue {
    /// <summary>
    /// Write this value to it's bytecode representation 
    /// </summary>
    /// <param name="writer">writer to write to</param>
    public abstract void WriteValue(BinaryWriter writer);

    /// <summary>
    /// String representation of the stored value
    /// </summary>
    public abstract string ValueToString();
}