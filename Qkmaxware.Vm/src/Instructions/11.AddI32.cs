namespace Qkmaxware.Vm.Instructions;

/// <summary>
/// Add 2 signed integers
/// </summary>
public class AddI32 : Instruction {

    public AddI32() {
        // Set opcode
        this.Opcode = 0x11; 
        
        // Arguments
    }

    public override string Description => "Arithmetic addition between 2 integers at the top of the operand stack";

    public override void Action(VmValue[] args, RuntimeEnvironment runtime) {
        /*         |-------|    |-------|
         *         |       |    |       |
         * 3 + 4 = |   4   | -> |       |
         *         |   3   |    |   7   |
         *         |-------|    |-------|
        */

        var rhs = (Int32Operand)runtime.Stack.PopTop();
        var lhs = (Int32Operand)runtime.Stack.PopTop();

        runtime.Stack.PushTop(new Int32Operand(lhs.Value + rhs.Value));
    }
}