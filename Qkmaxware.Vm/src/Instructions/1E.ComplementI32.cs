namespace Qkmaxware.Vm.Instructions;

/// <summary>
/// Bitwise complement a signed integer
/// </summary>
public class ComplementI32 : Instruction {

    public ComplementI32() {
        // Set opcode
        this.Opcode = 0x1E; 
        
        // Arguments
    }

    public override string Description => "Bitwise complement of the integer value on top of the operand stack";

    public override void Action(VmValue[] args, RuntimeEnvironment runtime) {
        var lhs = (Int32Operand)runtime.Stack.PopTop();

        runtime.Stack.PushTop(new Int32Operand(~lhs.Value));
    }
}