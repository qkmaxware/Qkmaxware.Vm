namespace Qkmaxware.Vm.Instructions;

/// <summary>
/// Duplicate an element "n" below the top of the stack
/// </summary>
public class DupBelow : Instruction {

    public DupBelow() {
        // Set opcode
        this.Opcode = 0x05; 
        
        // Arguments
        this.AddArgument(new Int32Argument("Stack Depth"));

        // Stack
        this.AddStackOperand("value");
        this.AddStackOperand("...");
        this.AddStackReturn("value");
        this.AddStackReturn("...");
        this.AddStackReturn("value");
    }

    public override string Description => "Duplicate a value below the top of the stack given by the depth argument and place the result on the top of the stack.";

    public override void Action(VmValue[] args, RuntimeEnvironment runtime) {
        var depth = (Operand)args[0];

        // Remove from the stack until we get to the index we want
        var stack = new Operand[depth.Int32];
        for (var i = 0; i < stack.Length; i++) {
            stack[i] = runtime.Stack.PopTop();
        }

        // Copy the top
        var top = runtime.Stack.PopTop();
        runtime.Stack.PushTop(top);

        // Reinsert the stack values in order
        for (var i = 0; i < stack.Length; i++) {
            runtime.Stack.PushTop(stack[stack.Length - 1 - i]);
        }

        // Add the duplicate
        runtime.Stack.PushTop(top);
    }
}