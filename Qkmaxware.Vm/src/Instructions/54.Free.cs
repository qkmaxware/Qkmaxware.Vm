namespace Qkmaxware.Vm.Instructions;

/// <summary>
/// Free a reserved block of memory in the heap
/// </summary>
public class Free : Instruction {

    public Free() {
        // Set opcode
        this.Opcode = 0x56; 
        
        // Arguments
    }

    public override string Description => "Free a reserved block of memory in the heap.";

    public override void Action(VmValue[] args, RuntimeEnvironment runtime) {
        var size = (HeapDataPointerOperand)runtime.Stack.PopTop();
        runtime.Heap.Free(size.Address);
    }
}