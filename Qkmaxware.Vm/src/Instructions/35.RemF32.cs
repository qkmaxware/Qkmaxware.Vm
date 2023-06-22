namespace Qkmaxware.Vm.Instructions;

/// <summary>
/// Remainder after division of 2 floating point numbers
/// </summary>
public class RemF32 : Instruction {

    public RemF32() {
        // Set opcode
        this.Opcode = 0x35; 
        
        // Arguments
    }

    public override string Description => "Arithmetic remainder between 2 floating-point values at the top of the operand stack";

    public override void Action(VmValue[] args, RuntimeEnvironment runtime) {
        var rhs = (Float32Operand)runtime.Stack.PopTop();
        var lhs = (Float32Operand)runtime.Stack.PopTop();

        runtime.Stack.PushTop(new Float32Operand(lhs.Value % rhs.Value));
    }
}