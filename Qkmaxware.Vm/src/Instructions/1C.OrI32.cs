namespace Qkmaxware.Vm.Instructions;

/// <summary>
/// Bitwise or of 2 signed integers
/// </summary>
public class OrI32 : Instruction {

    public OrI32() {
        // Set opcode
        this.Opcode = 0x1C; 
        
        // Arguments

        // Stack
        this.AddStackOperand("lhs");
        this.AddStackOperand("rhs");
        this.AddStackReturn("result");
    }

    public override string Description => "Bitwise OR between 2 integers at the top of the operand stack";

    public override void Action(VmValue[] args, RuntimeEnvironment runtime) {
        var rhs = (Operand)runtime.Stack.PopTop();
        var lhs = (Operand)runtime.Stack.PopTop();

        runtime.Stack.PushTop(Operand.From(lhs.Int32 | rhs.Int32));
    }
}