namespace Qkmaxware.Vm.Instructions;

/// <summary>
/// Divide 2 signed integers
/// </summary>
public class DivI32 : Instruction {

    public DivI32() {
        // Set opcode
        this.Opcode = 0x14; 
        
        // Arguments
    }

    public override string Description => "Arithmetic division between 2 integers at the top of the operand stack";

    public override void Action(VmValue[] args, RuntimeEnvironment runtime) {
        var rhs = runtime.Stack.PopTop();
        var lhs = runtime.Stack.PopTop();

        // TODO divide by 0 safety checks
        runtime.Stack.PushTop(Operand.From(lhs.Int32 / rhs.Int32));
    }
}