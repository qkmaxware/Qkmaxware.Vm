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
        var rhs = runtime.Stack.PopTop();
        var lhs = runtime.Stack.PopTop();

        runtime.Stack.PushTop(Operand.From(lhs.Int32 * rhs.Int32));
    }
}