namespace Qkmaxware.Vm.Instructions;

/// <summary>
/// Flip bits of a unsigned integer
/// </summary>
public class FlipU32 : Instruction {

    public FlipU32() {
        // Set opcode
        this.Opcode = 0x2B; 
        
        // Arguments
    }

    public override string Description => "Bitwise flip of the integer value on top of the operand stack";

    public override void Action(VmValue[] args, RuntimeEnvironment runtime) {
        var lhs = (Int32Operand)runtime.Stack.PopTop();

        runtime.Stack.PushTop(new Int32Operand(~lhs.Value));
    }
}