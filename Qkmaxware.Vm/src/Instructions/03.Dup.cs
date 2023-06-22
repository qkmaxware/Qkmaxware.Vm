namespace Qkmaxware.Vm.Instructions;

/// <summary>
/// Duplicate the top of the stack
/// </summary>
public class Dup : Instruction {

    public Dup() {
        // Set opcode
        this.Opcode = 0x03; 
        
        // Arguments
    }

    public override string Description => "Duplicate the element at the top of the operand stack.";

    public override void Action(VmValue[] args, RuntimeEnvironment runtime) {
        var top = runtime.Stack.PopTop();

        runtime.Stack.PushTop(top);
        runtime.Stack.PushTop(top);
    }
}