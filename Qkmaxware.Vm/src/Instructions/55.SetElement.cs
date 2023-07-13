namespace Qkmaxware.Vm.Instructions;

/// <summary>
///Set an element from an array by it's offset and it's pointer
/// </summary>
public class SetElement : Instruction {

    public SetElement() {
        // Set opcode
        this.Opcode = 0x55; 
        
        // Arguments
    }

    public override string Description => "Using a pointer to an array at the top of the operand stack, store a value at a particular index.";

    public override void Action(VmValue[] args, RuntimeEnvironment runtime) {
        var value = (Operand)runtime.Stack.PopTop();
        var offset = (Operand)runtime.Stack.PopTop();
        var ptr = runtime.Stack.PopTop().Pointer32;
        if (ptr.IsConstantPoolIndex()) {
            throw new NotImplementedException();
        } else if (ptr.IsHeapAddress()) {
            var index = ptr.IntValue + (4 * offset.Int32); // On the heap elements are read as words (32bits/4bytes)
            runtime.Heap.WriteWord32(index, value.UInt32);
        } else {
            throw new NotImplementedException();
        }
    }
}