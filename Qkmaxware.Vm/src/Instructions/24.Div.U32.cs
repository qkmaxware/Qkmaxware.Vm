namespace Qkmaxware.Vm.Instructions;

/// <summary>
/// Divide 2 unsigned integers
/// </summary>
public class DivU32 : Instruction {

    public DivU32() {
        // Set opcode
        this.Opcode = 0x24; 
        
        // Arguments
    }

    public override string Description => "Arithmetic division between 2 unsigned integers at the top of the operand stack";

    public override void Action(VmValue[] args, RuntimeEnvironment runtime) {
        var rhs = runtime.Stack.PopTop();
        var lhs = runtime.Stack.PopTop();

        runtime.Stack.PushTop(Operand.From(lhs.UInt32 / rhs.UInt32));
    }
}