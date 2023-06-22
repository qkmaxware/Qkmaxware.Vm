namespace Qkmaxware.Vm.Instructions;

/// <summary>
/// Bitwise exclusive or of 2 signed integers
/// </summary>
public class XorI32 : Instruction {

    public XorI32() {
        // Set opcode
        this.Opcode = 0x1D; 
        
        // Arguments
    }

    public override string Description => "Bitwise Exclusive OR between 2 integers at the top of the operand stack";

    public override void Action(VmValue[] args, RuntimeEnvironment runtime) {
        var rhs = (Operand)runtime.Stack.PopTop();
        var lhs = (Operand)runtime.Stack.PopTop();

        runtime.Stack.PushTop(Operand.From(lhs.Int32 ^ rhs.Int32));
    }
}