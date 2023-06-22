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
        var rhs = runtime.Stack.PopTop();
        var lhs = runtime.Stack.PopTop();

        runtime.Stack.PushTop(Operand.From(lhs.UInt32 ^ rhs.UInt32));
    }
}