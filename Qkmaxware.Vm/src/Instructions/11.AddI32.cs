namespace Qkmaxware.Vm.Instructions;

/// <summary>
/// Add 2 signed integers
/// </summary>
public class AddI32 : Instruction {

    public AddI32() {
        // Set opcode
        this.Opcode = 0x11; 
        
        // Arguments

        // Stack
        this.AddStackOperand("lhs");
        this.AddStackOperand("rhs");
        this.AddStackReturn("result");
    }

    public override string Description => "Arithmetic addition between 2 integers at the top of the operand stack";

    public override void Action(VmValue[] args, RuntimeEnvironment runtime) {
        /*         |-------|    |-------|
         *         |       |    |       |
         * 3 + 4 = |   4   | -> |       |
         *         |   3   |    |   7   |
         *         |-------|    |-------|
        */

        var rhs = runtime.Stack.PopTop();
        var lhs = runtime.Stack.PopTop();

        runtime.Stack.PushTop(Operand.From(lhs.Int32 + rhs.Int32));
    }
}