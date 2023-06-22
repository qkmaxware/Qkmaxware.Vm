namespace Qkmaxware.Vm.Instructions;

/// <summary>
/// Load an subprogram argument from the current stack frame onto the top of the stack
/// </summary>
public class LoadArg : Instruction {

    public LoadArg() {
        // Set opcode
        this.Opcode = 0x67; 
        
        // Arguments
        this.AddArgument(new Int32Argument("Argument Index"));
    }

    /*
        add_i32
        load_arg 1
        load_arg 0
        ..
        ..
        ..
        2 
    FP ----
        3
        4
    */

    public override string Description => "In the given subprogram, load a value from a subprogram argument onto the top of the operand stack";

    public override void Action(VmValue[] args, RuntimeEnvironment runtime) {
        var index = (Int32Operand)args[0];
        var argc = (Int32Operand)runtime.Stack.GetFrameRelative(0);
        runtime.Stack.PushTop(
            runtime.Stack.GetFrameRelative(-argc.Value + index.Value)
        );
    }
}