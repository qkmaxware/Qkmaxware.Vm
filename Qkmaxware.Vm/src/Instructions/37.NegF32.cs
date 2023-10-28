namespace Qkmaxware.Vm.Instructions;

/// <summary>
/// Negate a floating point number
/// </summary>
public class NegF32 : Instruction {

    public NegF32() {
        // Set opcode
        this.Opcode = 0x37; 
        
        // Arguments

        // Stack
        this.AddStackReturn("value");
    }

    public override string Description => "Negation of the floating-point value on top of the operand stack";

    public override void Action(VmValue[] args, RuntimeEnvironment runtime) {
        var lhs = runtime.Stack.PopTop();

        runtime.Stack.PushTop(Operand.From(-lhs.Float32));
    }
}