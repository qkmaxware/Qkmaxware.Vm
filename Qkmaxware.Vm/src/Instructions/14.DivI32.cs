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
        var rhs = (Int32Operand)runtime.Stack.PopTop();
        var lhs = (Int32Operand)runtime.Stack.PopTop();

        // TODO divide by 0 safety checks
        runtime.Stack.PushTop(new Int32Operand(lhs.Value / rhs.Value));
    }
}