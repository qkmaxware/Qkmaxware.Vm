namespace Qkmaxware.Vm.Instructions;

/// <summary>
/// Multiply 2 unsigned integers
/// </summary>
public class MulU32 : Instruction {

    public MulU32() {
        // Set opcode
        this.Opcode = 0x23; 
        
        // Arguments
    }

    public override string Description => "Arithmetic multiplication between 2 unsigned integers at the top of the operand stack";

    public override void Action(VmValue[] args, RuntimeEnvironment runtime) {
        var rhs = (UInt32Operand)runtime.Stack.PopTop();
        var lhs = (UInt32Operand)runtime.Stack.PopTop();

        runtime.Stack.PushTop(new UInt32Operand(lhs.Value * rhs.Value));
    }
}