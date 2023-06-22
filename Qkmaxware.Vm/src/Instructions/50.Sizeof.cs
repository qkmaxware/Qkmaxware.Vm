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
        var ptr = runtime.Stack.PopTop().Pointer32;
        if (ptr.IsConstantPoolIndex()) {
            var bytes = runtime.ConstantPool[ptr.IntValue].ByteCount();
            runtime.Stack.PushTop(Operand.From(bytes));
        } else if (ptr.IsHeapAddress()) {
            var block = runtime.Heap.BlockInfo(ptr.IntValue);
            runtime.Stack.PushTop(Operand.From(block.Size.ByteCount));
        } else {
            throw new NotImplementedException();
        }
    }
}