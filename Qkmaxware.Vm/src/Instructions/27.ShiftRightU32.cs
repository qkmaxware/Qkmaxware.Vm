namespace Qkmaxware.Vm.Instructions;

/// <summary>
/// Right shift a unsigned integer by another
/// </summary>
public class RightShiftU32 : Instruction {

    public RightShiftU32() {
        // Set opcode
        this.Opcode = 0x27; 
        
        // Arguments
    }

    public override string Description => "Right shift the elements of one unsigned integer by another";

    public override void Action(VmValue[] args, RuntimeEnvironment runtime) {
        var rhs = (UInt32Operand)runtime.Stack.PopTop();
        var lhs = (UInt32Operand)runtime.Stack.PopTop();

        runtime.Stack.PushTop(new UInt32Operand(lhs.Value >> (int)rhs.Value)); // TODO look at this later
    }
}