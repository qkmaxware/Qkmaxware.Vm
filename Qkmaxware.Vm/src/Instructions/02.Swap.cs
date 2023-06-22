namespace Qkmaxware.Vm.Instructions;

/// <summary>
/// Swap top 2 stack elements
/// </summary>
public class Swap : Instruction {

    public Swap() {
        // Set opcode
        this.Opcode = 0x02; 
        
        // Arguments
    }

    public override string Description => "Swap the positions of the top 2 elements on the operand stack.";

    public override void Action(VmValue[] args, RuntimeEnvironment runtime) {
        var top = runtime.Stack.PopTop();
        var bottom = runtime.Stack.PopTop();

        runtime.Stack.PushTop(top);
        runtime.Stack.PushTop(bottom);
    }
}