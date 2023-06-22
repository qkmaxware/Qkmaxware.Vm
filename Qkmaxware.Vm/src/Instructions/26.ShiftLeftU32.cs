namespace Qkmaxware.Vm.Instructions;

/// <summary>
/// Left shift a unsigned integer by another
/// </summary>
public class LeftShiftU32 : Instruction {

    public LeftShiftU32() {
        // Set opcode
        this.Opcode = 0x26; 
        
        // Arguments
    }

    public override string Description => "Left shift the elements of one unsigned integer by another";

    public override void Action(VmValue[] args, RuntimeEnvironment runtime) {
        var rhs = (UInt32Operand)runtime.Stack.PopTop();
        var lhs = (UInt32Operand)runtime.Stack.PopTop();

        runtime.Stack.PushTop(new UInt32Operand(lhs.Value << (int)rhs.Value)); // TODO look at this later
    }
}