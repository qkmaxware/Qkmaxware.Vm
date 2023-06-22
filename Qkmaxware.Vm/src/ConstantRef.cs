namespace Qkmaxware.Vm;

/// <summary>
/// A reference to a constant in the constant pool
/// </summary>
public class ConstantRef {
    public int PoolIndex {get; private set;}

    internal ConstantRef(int index) {
        this.PoolIndex = index;
    }
}