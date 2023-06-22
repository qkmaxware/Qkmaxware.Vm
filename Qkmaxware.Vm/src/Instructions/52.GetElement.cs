namespace Qkmaxware.Vm.Instructions;

/// <summary>
/// Get an element from an array by it's offset and it's pointer
/// </summary>
public class GetElement : Instruction {

    public GetElement() {
        // Set opcode
        this.Opcode = 0x52; 
        
        // Arguments
    }

    public override string Description => "Using a pointer to an array at the top of the operand stack, fetch s particular element in that array and place that value on top of the operand stack.";

    public override void Action(VmValue[] args, RuntimeEnvironment runtime) {
        var offset = (Int32Operand)runtime.Stack.PopTop();
        var ptr = (ArrayPointerOperand)runtime.Stack.PopTop();
        runtime.Stack.PushTop(ptr.GetElementAt(offset.Value));
    }
}