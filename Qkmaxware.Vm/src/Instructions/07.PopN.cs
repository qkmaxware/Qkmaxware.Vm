namespace Qkmaxware.Vm.Instructions;

/// <summary>
/// Pop and discard the top N values of the stack
/// </summary>
public class PopN : Instruction {

    public PopN() {
        // Set opcode
        this.Opcode = 0x07; 
        
        // Arguments
        this.AddArgument(new Int32Argument("Popped Amount"));
    }

    public override string Description => "Remove the given amount of values from the top of the operand stack.";

    public override void Action(VmValue[] args, RuntimeEnvironment runtime) {
        var size = Math.Max(((Operand)args[0]).Int32, 0);

        for (var i = 0; i < size; i++) {
            runtime.Stack.PopTop();
        }
    }
}