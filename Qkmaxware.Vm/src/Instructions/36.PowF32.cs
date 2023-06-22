namespace Qkmaxware.Vm.Instructions;

/// <summary>
/// Exponentiation of one floating point number by another
/// </summary>
public class PowF32 : Instruction {

    public PowF32() {
        // Set opcode
        this.Opcode = 0x36; 
        
        // Arguments
    }

    public override string Description => "Arithmetic exponentiation between 2 floating-point values at the top of the operand stack";

    public override void Action(VmValue[] args, RuntimeEnvironment runtime) {
        var rhs = runtime.Stack.PopTop();
        var lhs = runtime.Stack.PopTop();

        runtime.Stack.PushTop(Operand.From(MathF.Pow(lhs.Float32, rhs.Float32)));
    }
}