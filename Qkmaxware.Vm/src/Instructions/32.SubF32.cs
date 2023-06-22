namespace Qkmaxware.Vm.Instructions;

/// <summary>
/// Subtract 2 floating point numbers
/// </summary>
public class SubF32 : Instruction {

    public SubF32() {
        // Set opcode
        this.Opcode = 0x32; 
        
        // Arguments
    }

    public override string Description => "Arithmetic subtraction between 2 floating-point values at the top of the operand stack";

    public override void Action(VmValue[] args, RuntimeEnvironment runtime) {
        var rhs = runtime.Stack.PopTop();
        var lhs = runtime.Stack.PopTop();

        runtime.Stack.PushTop(Operand.From(lhs.Float32 - rhs.Float32));
    }
}