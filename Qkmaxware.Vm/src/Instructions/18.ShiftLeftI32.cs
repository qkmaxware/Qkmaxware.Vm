namespace Qkmaxware.Vm.Instructions;

/// <summary>
/// Left shift a signed integer by another
/// </summary>
public class LeftShiftI32 : Instruction {

    public LeftShiftI32() {
        // Set opcode
        this.Opcode = 0x18; 
        
        // Arguments

        // Stack
        this.AddStackOperand("lhs");
        this.AddStackOperand("rhs");
        this.AddStackReturn("result");
    }

    public override string Description => "Left shift the elements of one integer by another";

    public override void Action(VmValue[] args, RuntimeEnvironment runtime) {
        var rhs = runtime.Stack.PopTop();
        var lhs = runtime.Stack.PopTop();

        runtime.Stack.PushTop(Operand.From(lhs.Int32 << rhs.Int32));
    }
}