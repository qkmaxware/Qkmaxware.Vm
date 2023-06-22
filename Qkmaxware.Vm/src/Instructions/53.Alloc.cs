namespace Qkmaxware.Vm.Instructions;

/// <summary>
/// Allocate a contiguous block of memory in the heap
/// </summary>
public class Alloc : Instruction {

    public Alloc() {
        // Set opcode
        this.Opcode = 0x53; 
        
        // Arguments
    }

    public override string Description => "Allocate a contiguous block of memory in the heap.";

    public override void Action(VmValue[] args, RuntimeEnvironment runtime) {
        var size = runtime.Stack.PopTop();
        var address = runtime.Heap.Reserve(size.Int32);

        runtime.Stack.PushTop(Operand.From(new Pointer(PointerType.HeapAddress, address)));
    }
}