namespace Qkmaxware.Vm.Instructions;

/// <summary>
/// Load a local from the current stack frame onto the top of the stack
/// </summary>
public class LoadLocal : Instruction {

    public LoadLocal() {
        // Set opcode
        this.Opcode = 0x68; 
        
        // Arguments
        this.AddArgument(new Int32Argument("Local Index"));

        // Stack
        this.AddStackReturn("local");
    }

    public override string Description => "In the given subprogram, load a value from a local variable onto the top of the operand stack";

    public override void Action(VmValue[] args, RuntimeEnvironment runtime) {
        var index = (Operand)args[0];
        runtime.Stack.PushTop(
            runtime.Stack.GetFrameRelative(index.Int32 + 4)
        );
    }
}