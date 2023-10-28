namespace Qkmaxware.Vm.Instructions;

/// <summary>
/// Unconditionally jump to a position in the program using it's offset from the current position if the top of the stack is not 0
/// </summary>
public class GotoIfNzero : Instruction {

    public GotoIfNzero() {
        // Set opcode
        this.Opcode = 0x62; 
        
        // Arguments
        this.AddArgument(new Int32Argument("Offset"));

        // Stack
        this.AddStackOperand("condition");
    }

    public override string Description => "If the top of the stack is an integer not equal to 0 then jump to another position in the program at PC + Offset.";

    public override void Action(VmValue[] args, RuntimeEnvironment runtime) {
        var offset = ((Operand)args[0]);
        var condition = (Operand)runtime.Stack.PopTop();
        
        if (condition.Int32 != 0) {
            var now = runtime.PC;
            var next = now + offset.Int32;
            runtime.PC = next;
            //runtime.Host.StdOut.Write("Jumping from " + runtime.PC + " by " + offset.Value + " to " + next);
        }
    }
}