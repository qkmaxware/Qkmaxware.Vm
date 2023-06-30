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
        var offset = (Operand)runtime.Stack.PopTop();
        var ptr = runtime.Stack.PopTop().Pointer32;
        if (ptr.IsConstantPoolIndex()) {
            var element = (ArrayConstant)runtime.ConstantPool[ptr.IntValue];
            runtime.Stack.PushTop(element.GetElementAt(offset.Int32));
        } else if (ptr.IsHeapAddress()) {
            var index = ptr.IntValue + (4 * offset.Int32); // On the heap elements are read as words (32bits/4bytes)
            runtime.Stack.PushTop(Operand.From(runtime.Heap.ReadWord32(index)));
        } else {
            throw new NotImplementedException();
        }
    }
}