namespace Qkmaxware.Vm.Instructions;

/// <summary>
/// Convert a floating point value to a string on the heap
/// </summary>
public class F32ToString : Instruction {

    public F32ToString() {
        // Set opcode
        this.Opcode = 0x48; 
        
        // Arguments
    }

    public override string Description => "Convert the top of the stack from an 32bit float into a string on the heap. Requires cleanup after use.";

    public override void Action(VmValue[] args, RuntimeEnvironment runtime) {
        var value = runtime.Stack.PopTop().Float32.ToString();
        var bytes = 4 * value.Length;
        var address = runtime.Heap.Reserve(bytes);
        for (int offset = 0; offset < value.Length; offset++) {
            var c = value[offset];
            runtime.Heap.WriteWord32(address + 4 * offset, c);
        }
        runtime.Stack.PushTop(Operand.From(new Pointer(PointerType.HeapAddress, address)));
    }
}