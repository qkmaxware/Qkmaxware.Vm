namespace Qkmaxware.Vm.Ir;

/// <summary>
/// Base class for all expressions. Expressions should always store emit a value to the top of the stack.
/// </summary>
public abstract class Expression : IrNode {}