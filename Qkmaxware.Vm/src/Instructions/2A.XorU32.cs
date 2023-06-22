namespace Qkmaxware.Vm.Instructions;

/// <summary>
/// Bitwise exclusive or of 2 unsigned integers
/// </summary>
public class XorU32 : Instruction {

    public XorU32() {
        // Set opcode
        this.Opcode = 0x2A; 
        
        // Arguments
    }

    public override string Description => "Bitwise Exclusive OR between 2 unsigned integers at the top of the operand stack";

    public override void Action(VmValue[] args, RuntimeEnvironment runtime) {
        var rhs = (UInt32Operand)runtime.Stack.PopTop();
        var lhs = (UInt32Operand)runtime.Stack.PopTop();

        runtime.Stack.PushTop(new UInt32Operand(lhs.Value ^ rhs.Value));
    }
}