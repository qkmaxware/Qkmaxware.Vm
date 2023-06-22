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

        var rhs = runtime.Stack.PopTop();
        var lhs = runtime.Stack.PopTop();

        runtime.Stack.PushTop(Operand.From(lhs.Int32 - rhs.Int32));
    }
}