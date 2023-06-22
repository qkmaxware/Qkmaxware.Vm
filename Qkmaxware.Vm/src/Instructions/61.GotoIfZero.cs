namespace Qkmaxware.Vm.Instructions;

/// <summary>
/// Unconditionally jump to a position in the program using it's offset from the current position if the top of the stack is 0
/// </summary>
public class GotoIfZero : Instruction {

    public GotoIfZero() {
        // Set opcode
        this.Opcode = 0x61; 
        
        // Arguments
        this.AddArgument(new Int32Argument("Offset"));
    }

    public override string Description => "If the top of the stack is an integer equal to 0 then jump to another position in the program at PC + Offset.";

    public override void Action(VmValue[] args, RuntimeEnvironment runtime) {
        var offset = ((Int32Operand)args[0]);
        var condition = (Int32Operand)runtime.Stack.PopTop();
        
        if (condition.Value == 0) {
            var now = runtime.PC;
            var next = now + offset.Value;
            runtime.PC = next;
        }
    }
}