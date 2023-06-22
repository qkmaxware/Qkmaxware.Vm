namespace Qkmaxware.Vm.Instructions;

/// <summary>
/// Bitwise and of 2 signed integers
/// </summary>
public class AndI32 : Instruction {

    public AndI32() {
        // Set opcode
        this.Opcode = 0x1B; 
        
        // Arguments
    }

    public override string Description => "Bitwise AND between 2 integers at the top of the operand stack";

    public override void Action(VmValue[] args, RuntimeEnvironment runtime) {
        var rhs = (Int32Operand)runtime.Stack.PopTop();
        var lhs = (Int32Operand)runtime.Stack.PopTop();

        runtime.Stack.PushTop(new Int32Operand(lhs.Value & rhs.Value));
    }
}