namespace Qkmaxware.Vm.Instructions;

/// <summary>
/// right shift a signed integer, signed extended, by another
/// </summary>
public class RightShiftArithmeticI32 : Instruction {

    public RightShiftArithmeticI32() {
        // Set opcode
        this.Opcode = 0x19; 
        
        // Arguments
    }

    public override string Description => "Arithmetic right shift the elements of one integer by another";

    public override void Action(VmValue[] args, RuntimeEnvironment runtime) {
        var rhs = (Int32Operand)runtime.Stack.PopTop();
        var lhs = (Int32Operand)runtime.Stack.PopTop();

        runtime.Stack.PushTop(new Int32Operand(lhs.Value >> rhs.Value));
    }
}