namespace Qkmaxware.Vm.Instructions;

/// <summary>
/// Convert an integer to a string on the heap
/// </summary>
public class I32ToString : Instruction {

    public I32ToString() {
        // Set opcode
        this.Opcode = 0x46; 
        
        // Arguments
        this.AddArgument(new Int32Argument("Memory Index"));
    }

    public override string Description => "Convert the top of the stack from an 32bit integer into a string on the heap. Requires cleanup after use.";

    public override void Action(VmValue[] args, RuntimeEnvironment runtime) {
        var value = runtime.Stack.PopTop().Int32.ToString();
        var bytes = 4 * value.Length;
        var memIdx = ((Operand)args[0]).Int32;

        var memory = runtime.Memories[memIdx];
        var address = memory.Reserve(bytes);
        for (int offset = 0; offset < value.Length; offset++) {
            var c = value[offset];
            memory.Write8(address + Memory.BlockHeaderSize.ByteCount + offset, (byte)c);
        }
        runtime.Stack.PushTop(Operand.From(address));
    }
}