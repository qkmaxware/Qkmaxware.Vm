namespace Qkmaxware.Vm.Instructions;

/// <summary>
/// Multiply 2 floating point numbers
/// </summary>
public class MulF32 : Instruction {

    public MulF32() {
        // Set opcode
        this.Opcode = 0x33; 
        
        // Arguments

        // Stack
        this.AddStackOperand("lhs");
        this.AddStackOperand("rhs");
        this.AddStackReturn("result");
    }

    public override string Description => "Arithmetic multiplication between 2 floating-point values at the top of the operand stack";

    public override void Action(VmValue[] args, RuntimeEnvironment runtime) {
        var rhs = runtime.Stack.PopTop();
        var lhs = runtime.Stack.PopTop();

        runtime.Stack.PushTop(Operand.From(lhs.Float32 * rhs.Float32));
    }
}