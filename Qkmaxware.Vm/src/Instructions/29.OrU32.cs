namespace Qkmaxware.Vm.Instructions;

/// <summary>
/// Bitwise or of 2 unsigned integers
/// </summary>
public class OrU32 : Instruction {

    public OrU32() {
        // Set opcode
        this.Opcode = 0x29; 
        
        // Arguments

        // Stack
        this.AddStackOperand("lhs");
        this.AddStackOperand("rhs");
        this.AddStackReturn("result");
    }

    public override string Description => "Bitwise OR between 2 unsigned integers at the top of the operand stack";

    public override void Action(VmValue[] args, RuntimeEnvironment runtime) {
        var rhs = runtime.Stack.PopTop();
        var lhs = runtime.Stack.PopTop();

        runtime.Stack.PushTop(Operand.From(lhs.UInt32 | rhs.UInt32));
    }
}