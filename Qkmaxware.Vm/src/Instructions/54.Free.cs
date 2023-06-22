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
        var address = runtime.Stack.PopTop().Pointer32;
        if (!address.IsHeapAddress())
            throw new NotImplementedException("Cannot free memory no on the heap");
        runtime.Heap.Free(address.IntValue);
    }
}