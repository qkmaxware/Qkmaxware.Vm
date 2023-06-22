namespace Qkmaxware.Vm.Instructions;

/// <summary>
/// Euclidean modulus of 2 signed integers
/// </summary>
public class ModI32 : Instruction {

    public ModI32() {
        // Set opcode
        this.Opcode = 0x16; 
        
        // Arguments
    }

    public override string Description => "Arithmetic modulus between 2 integers at the top of the operand stack";

    public override void Action(VmValue[] args, RuntimeEnvironment runtime) {
        var rhs = (Int32Operand)runtime.Stack.PopTop();
        var lhs = (Int32Operand)runtime.Stack.PopTop();

        if (rhs.Value == -1) {
            runtime.Stack.PushTop(new Int32Operand(0));
            return;
        }

        var mod = lhs.Value - (int)Math.Floor((double)lhs.Value / rhs.Value) * rhs.Value;

        runtime.Stack.PushTop(new Int32Operand(mod));
    }
}