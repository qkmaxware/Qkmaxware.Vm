namespace Qkmaxware.Vm.Instructions;

/// <summary>
/// Negate a signed integer
/// </summary>
public class NegI32 : Instruction {

    public NegI32() {
        // Set opcode
        this.Opcode = 0x17; 
        
        // Arguments

        // Stack
        this.AddStackOperand("operand");
        this.AddStackReturn("result");
    }

    public override string Description => "Negation of the integer value on top of the operand stack";

    public override void Action(VmValue[] args, RuntimeEnvironment runtime) {
        var lhs = runtime.Stack.PopTop();

        runtime.Stack.PushTop(Operand.From(-lhs.Int32));
    }
}