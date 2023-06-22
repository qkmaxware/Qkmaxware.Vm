namespace Qkmaxware.Vm.Instructions;

/// <summary>
/// Multiply 2 signed integers
/// </summary>
public class MulI32 : Instruction {

    public MulI32() {
        // Set opcode
        this.Opcode = 0x13; 
        
        // Arguments
    }

    public override string Description => "Arithmetic multiplication between 2 integers at the top of the operand stack";

    public override void Action(VmValue[] args, RuntimeEnvironment runtime) {
        var rhs = (Int32Operand)runtime.Stack.PopTop();
        var lhs = (Int32Operand)runtime.Stack.PopTop();

        runtime.Stack.PushTop(new Int32Operand(lhs.Value * rhs.Value));
    }
}