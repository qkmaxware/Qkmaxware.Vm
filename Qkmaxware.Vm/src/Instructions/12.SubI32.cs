namespace Qkmaxware.Vm.Instructions;

/// <summary>
/// Subtract 2 signed integers
/// </summary>
public class SubI32 : Instruction {

    public SubI32() {
        // Set opcode
        this.Opcode = 0x12; 
        
        // Arguments
    }

    public override string Description => "Arithmetic subtraction between 2 integers at the top of the operand stack";

    public override void Action(VmValue[] args, RuntimeEnvironment runtime) {
        /*         |-------|    |-------|
         *         |       |    |       |
         * 3 - 4 = |   4   | -> |       |
         *         |   3   |    |   -1  |
         *         |-------|    |-------|
        */

        var rhs = (Int32Operand)runtime.Stack.PopTop();
        var lhs = (Int32Operand)runtime.Stack.PopTop();

        runtime.Stack.PushTop(new Int32Operand(lhs.Value - rhs.Value));
    }
}