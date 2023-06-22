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
        var ptr = runtime.Stack.PopTop().Pointer32;
        if (ptr.IsConstantPoolIndex()) {
            var elements = runtime.ConstantPool[ptr.IntValue].ElementCount();
            runtime.Stack.PushTop(Operand.From(elements));
        } else if (ptr.IsHeapAddress()) {
            var block = runtime.Heap.BlockInfo(ptr.IntValue);
            runtime.Stack.PushTop(Operand.From(block.Size.ByteCount / 4));
        } else {
            throw new NotImplementedException();
        }
    }
}