namespace Qkmaxware.Vm.Instructions;

/// <summary>
/// Invoke a subprogram at the given offset from the current position
/// </summary>
public class Call : Instruction {
    
    public Call() {
        // Set opcode
        this.Opcode = 0x63; 
        
        // Arguments
        this.AddArgument(new Int32Argument("Offset"));
        this.AddArgument(new Int32Argument("Argument Count"));
    }

    public override string Description => "Call a subprogram in the program at PC + Offset using the last few vales on the operand stack as the subprogram arguments.";

    public override void Action(VmValue[] args, RuntimeEnvironment runtime) {
        // Modify SP
        var prev_sp = runtime.Stack.SP;
        
        // Modify FP
        var prev_fp = runtime.Stack.FP;
        
        // Modify PC 
        var prev_pc = runtime.PC;
        
        // Store Argument Count
        var argc = ((Operand)args[1]);
        
        // Preserve old values
        runtime.Stack.FP = prev_sp;
        runtime.Stack.PushTop(argc);
        runtime.Stack.PushTop(Operand.From((uint)prev_pc));
        runtime.Stack.PushTop(Operand.From(prev_fp));
        runtime.Stack.PushTop(Operand.From(prev_sp));

        // Jump
        var offset = ((Operand)args[0]);
        var next = prev_pc + offset.Int32;
        runtime.PC = next;
    }
}