namespace Qkmaxware.Vm.Instructions;

/// <summary>
/// Return from a subprogram call to the caller position in the code, preserve the top of the stack
/// </summary>
public class ReturnFunction : Instruction {
    
    public ReturnFunction() {
        // Set opcode
        this.Opcode = 0x65; 
        
        // Arguments
    }

    public override string Description => "Return from a subprogram to it's original call location preserving the top of the operand stack as a returned value.";

    public override void Action(VmValue[] args, RuntimeEnvironment runtime) {      
        // Modify SP
        var prev_sp = runtime.Stack.GetFrameRelative(3);
        
        // Modify FP
        var prev_fp = runtime.Stack.GetFrameRelative(2);
        
        // Modify PC 
        var prev_pc = runtime.Stack.GetFrameRelative(1);
        
        // Store Argument Count
        var argc = runtime.Stack.GetFrameRelative(0);
        
        // Begin popping stack
        var top = runtime.Stack.PeekTop();
        while (runtime.Stack.SP > runtime.Stack.FP) {
            runtime.Stack.PopTop();
        }

        // Preserve the top of the stack
        if (top != null)
            runtime.Stack.PushTop(top);

        // Jump
        runtime.PC = prev_pc.UInt32;
        runtime.Stack.FP = prev_fp.Int32;
    }
}