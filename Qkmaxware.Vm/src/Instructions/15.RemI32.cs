namespace Qkmaxware.Vm.Instructions;

/// <summary>
/// Remainder after division of 2 signed integers
/// </summary>
public class RemI32 : Instruction {

    public RemI32() {
        // Set opcode
        this.Opcode = 0x15; 
        
        // Arguments
    }

    public override string Description => "Arithmetic remainder between 2 integers at the top of the operand stack";

    public override void Action(VmValue[] args, RuntimeEnvironment runtime) {
        var rhs = (Int32Operand)runtime.Stack.PopTop();
        var lhs = (Int32Operand)runtime.Stack.PopTop();

        runtime.Stack.PushTop(new Int32Operand(lhs.Value % rhs.Value));
    }
}