namespace Qkmaxware.Vm.Instructions;

/// <summary>
/// Negate a signed integer
/// </summary>
public class NegI32 : Instruction {

    public NegI32() {
        // Set opcode
        this.Opcode = 0x17; 
        
        // Arguments
    }

    public override string Description => "Negation of the integer value on top of the operand stack";

    public override void Action(VmValue[] args, RuntimeEnvironment runtime) {
        var lhs = (Int32Operand)runtime.Stack.PopTop();

        runtime.Stack.PushTop(new Int32Operand(-lhs.Value));
    }
}