namespace Qkmaxware.Vm.Instructions;

/// <summary>
/// Store the top of the stack into a local from the current stack frame 
/// </summary>
public class StoreStatic : Instruction {

    public StoreStatic() {
        // Set opcode
        this.Opcode = 0x71; 
        
        // Arguments
        this.AddArgument(new Int32Argument("Static Index"));
    }

    public override string Description => "Save a value on the top of the stack into a static pool location";

    public override void Action(VmValue[] args, RuntimeEnvironment runtime) {
        var index = (Operand)args[0];
        runtime.StaticPool[index.Int32] = runtime.Stack.PopTop();
    }
}