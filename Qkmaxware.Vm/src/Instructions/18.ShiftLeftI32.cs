namespace Qkmaxware.Vm.Instructions;

/// <summary>
/// Left shift a signed integer by another
/// </summary>
public class LeftShiftI32 : Instruction {

    public LeftShiftI32() {
        // Set opcode
        this.Opcode = 0x18; 
        
        // Arguments
    }

    public override string Description => "Left shift the elements of one integer by another";

    public override void Action(VmValue[] args, RuntimeEnvironment runtime) {
        var rhs = (Int32Operand)runtime.Stack.PopTop();
        var lhs = (Int32Operand)runtime.Stack.PopTop();

        runtime.Stack.PushTop(new Int32Operand(lhs.Value << rhs.Value));
    }
}