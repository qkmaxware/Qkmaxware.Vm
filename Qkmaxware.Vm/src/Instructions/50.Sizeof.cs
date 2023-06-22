namespace Qkmaxware.Vm.Instructions;

/// <summary>
/// Get the size of a compound data instance from it's pointer
/// </summary>
public class Sizeof : Instruction {

    public Sizeof() {
        // Set opcode
        this.Opcode = 0x50; 
        
        // Arguments
    }

    public override string Description => "Using a pointer at the top of the operand stack, determine the size of the object in bytes and place that value on top of the operand stack.";

    public override void Action(VmValue[] args, RuntimeEnvironment runtime) {
        var ptr = (CompoundDataPointerOperand)runtime.Stack.PopTop();
        var bytes = ptr.CountBytes();
        runtime.Stack.PushTop(new Int32Operand(bytes));
    }
}