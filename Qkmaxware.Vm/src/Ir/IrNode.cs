namespace Qkmaxware.Vm.Ir;

/// <summary>
/// Base class for all IR nodes
/// </summary>
public abstract class IrNode {
    /// <summary>
    /// Convert the IR to it's bytecode equivalent using the provided module builder
    /// Based on my MiniC compiler here https://github.com/qkmaxware/MiniC/blob/root/C/src/CToBytecode.cs
    /// </summary>
    /// <param name="builder">the builder to use to emit bytecode to</param>
    public abstract void ToBytecode(ModuleBuilder builder);
    
    /// <summary>
    /// Convert the IR to it's bytecode equivalent module 
    /// </summary>
    /// <returns></returns>
    public Module ToModule() {
        ModuleBuilder builder = new ModuleBuilder();
        this.ToBytecode(builder);
        return builder.ToModule();
    }
}