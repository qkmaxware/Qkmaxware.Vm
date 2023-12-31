namespace Qkmaxware.Vm.Instructions;

/// <summary>
/// Store the top of the stack into a local from the current stack frame 
/// </summary>
public class StoreLocal : Instruction {

    public StoreLocal() {
        // Set opcode
        this.Opcode = 0x69; 
        
        // Arguments
        this.AddArgument(new Int32Argument("Local Index"));
    }

    public override string Description => "Save a value on the top of the stack into a local variable in the current subprogram";

    public override void Action(VmValue[] args, RuntimeEnvironment runtime) {
        var index = (Operand)args[0];
        runtime.Stack.SetFrameRelative(
            index.Int32 + 4,
            runtime.Stack.PopTop()
        );
    }
}