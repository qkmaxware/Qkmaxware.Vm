namespace Qkmaxware.Vm.Instructions;

/// <summary>
/// Get the length of an array from it's pointer
/// </summary>
public class Len : Instruction {

    public Len() {
        // Set opcode
        this.Opcode = 0x51; 
        
        // Arguments
    }

    public override string Description => "Using a pointer to an array at the top of the operand stack, determine the length of the array and place that value on top of the operand stack.";

    public override void Action(VmValue[] args, RuntimeEnvironment runtime) {
        var ptr = (ArrayPointerOperand)runtime.Stack.PopTop();
        var length = ptr.CountElements();
        runtime.Stack.PushTop(new Int32Operand(length));
    }
}