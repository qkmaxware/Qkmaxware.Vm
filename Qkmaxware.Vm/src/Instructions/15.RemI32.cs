namespace Qkmaxware.Vm.Instructions;

/// <summary>
/// Remainder after division of 2 signed integers
/// </summary>
public class RemI32 : Instruction {

    public RemI32() {
        // Set opcode
        this.Opcode = 0x15; 
        
        // Arguments
    
        // Stack
        this.AddStackOperand("lhs");
        this.AddStackOperand("rhs");
        this.AddStackReturn("result");
    }

    public override string Description => "Arithmetic remainder between 2 integers at the top of the operand stack";

    public override void Action(VmValue[] args, RuntimeEnvironment runtime) {
        var rhs = runtime.Stack.PopTop();
        var lhs = runtime.Stack.PopTop();

        runtime.Stack.PushTop(Operand.From(lhs.Int32 % rhs.Int32));
    }
}