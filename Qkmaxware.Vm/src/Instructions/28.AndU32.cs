namespace Qkmaxware.Vm.Instructions;

/// <summary>
/// Bitwise and of 2 unsigned integers
/// </summary>
public class AndU32 : Instruction {

    public AndU32() {
        // Set opcode
        this.Opcode = 0x28; 
        
        // Arguments
    }

    public override string Description => "Bitwise AND between 2 unsigned integers at the top of the operand stack";

    public override void Action(VmValue[] args, RuntimeEnvironment runtime) {
        var rhs = runtime.Stack.PopTop();
        var lhs = runtime.Stack.PopTop();

        runtime.Stack.PushTop(Operand.From(lhs.UInt32 & rhs.UInt32));
    }
}