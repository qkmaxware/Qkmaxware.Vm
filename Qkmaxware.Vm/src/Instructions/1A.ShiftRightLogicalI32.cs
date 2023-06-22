namespace Qkmaxware.Vm.Instructions;

/// <summary>
/// right shift a signed integer, zero extending, by another
/// </summary>
public class RightShiftLogicalI32 : Instruction {

    public RightShiftLogicalI32() {
        // Set opcode
        this.Opcode = 0x1A; 
        
        // Arguments
    }

    public override string Description => "Logical right shift the elements of one integer by another";

    public override void Action(VmValue[] args, RuntimeEnvironment runtime) {
        var rhs = (Operand)runtime.Stack.PopTop();
        var lhs = (Operand)runtime.Stack.PopTop();

        runtime.Stack.PushTop(Operand.From(lhs.Int32 >>> rhs.Int32));
    }
}