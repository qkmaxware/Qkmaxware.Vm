namespace Qkmaxware.Vm.Instructions;

/// <summary>
/// Subtract 2 unsigned integers
/// </summary>
public class SubU32 : Instruction {

    public SubU32() {
        // Set opcode
        this.Opcode = 0x22; 
        
        // Arguments
    }

    public override string Description => "Arithmetic subtraction between 2 unsigned integers at the top of the operand stack";

    public override void Action(VmValue[] args, RuntimeEnvironment runtime) {
        var rhs = (UInt32Operand)runtime.Stack.PopTop();
        var lhs = (UInt32Operand)runtime.Stack.PopTop();

        runtime.Stack.PushTop(new UInt32Operand(lhs.Value - rhs.Value));
    }
}