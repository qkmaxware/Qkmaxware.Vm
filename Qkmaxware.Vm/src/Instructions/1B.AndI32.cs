namespace Qkmaxware.Vm.Instructions;

/// <summary>
/// Bitwise and of 2 signed integers
/// </summary>
public class AndI32 : Instruction {

    public AndI32() {
        // Set opcode
        this.Opcode = 0x1B; 
        
        // Arguments

        // Stack
        this.AddStackOperand("lhs");
        this.AddStackOperand("rhs");
        this.AddStackReturn("result");
    }

    public override string Description => "Bitwise AND between 2 integers at the top of the operand stack";

    public override void Action(VmValue[] args, RuntimeEnvironment runtime) {
        var rhs = (Operand)runtime.Stack.PopTop();
        var lhs = (Operand)runtime.Stack.PopTop();

        runtime.Stack.PushTop(Operand.From(lhs.Int32 & rhs.Int32));
    }
}