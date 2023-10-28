namespace Qkmaxware.Vm.Instructions;

/// <summary>
/// Right shift a unsigned integer by another
/// </summary>
public class RightShiftU32 : Instruction {

    public RightShiftU32() {
        // Set opcode
        this.Opcode = 0x27; 
        
        // Arguments

        // Stack
        this.AddStackOperand("lhs");
        this.AddStackOperand("rhs");
        this.AddStackReturn("result");
    }

    public override string Description => "Right shift the elements of one unsigned integer by another";

    public override void Action(VmValue[] args, RuntimeEnvironment runtime) {
        var rhs = runtime.Stack.PopTop();
        var lhs = runtime.Stack.PopTop();

        runtime.Stack.PushTop(Operand.From(lhs.UInt32 >> (int)rhs.UInt32)); // TODO look at this later
    }
}