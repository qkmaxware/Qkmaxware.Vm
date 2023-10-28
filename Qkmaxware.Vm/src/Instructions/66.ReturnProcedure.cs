namespace Qkmaxware.Vm.Instructions;

/// <summary>
/// Return from a subprogram call to the caller position in the code, no stack preservation
/// </summary>
public class ReturnProcedure : Instruction {
    
    public ReturnProcedure() {
        // Set opcode
        this.Opcode = 0x66; 
        
        // Arguments

        // Stack
    }

    public override string Description => "Return from a subprogram to it's original call location with no return value.";

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
        while (runtime.Stack.SP > runtime.Stack.FP) {
            runtime.Stack.PopTop();
        }

        // Jump
        runtime.PC = prev_pc.UInt32;
        runtime.Stack.FP = prev_fp.Int32;
    }
}