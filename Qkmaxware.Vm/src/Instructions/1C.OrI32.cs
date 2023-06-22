namespace Qkmaxware.Vm.Instructions;

/// <summary>
/// Bitwise or of 2 signed integers
/// </summary>
public class OrI32 : Instruction {

    public OrI32() {
        // Set opcode
        this.Opcode = 0x1C; 
        
        // Arguments
    }

    public override string Description => "Bitwise OR between 2 integers at the top of the operand stack";

    public override void Action(VmValue[] args, RuntimeEnvironment runtime) {
        var rhs = (Int32Operand)runtime.Stack.PopTop();
        var lhs = (Int32Operand)runtime.Stack.PopTop();

        runtime.Stack.PushTop(new Int32Operand(lhs.Value | rhs.Value));
    }
}