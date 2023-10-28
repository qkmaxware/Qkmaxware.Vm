namespace Qkmaxware.Vm.Instructions;

/// <summary>
/// Pop and discard top of the stack
/// </summary>
public class Pop : Instruction {

    public Pop() {
        // Set opcode
        this.Opcode = 0x06; 
        
        // Arguments
        this.AddStackOperand("value");
    }

    public override string Description => "Remove the value at the top of the operand stack.";

    public override void Action(VmValue[] args, RuntimeEnvironment runtime) {
        var top = runtime.Stack.PopTop();
    }
}