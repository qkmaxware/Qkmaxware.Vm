namespace Qkmaxware.Vm.Instructions;

/// <summary>
/// Convert a floating point value to a string on the heap
/// </summary>
public class F32ToString : Instruction {

    public F32ToString() {
        // Set opcode
        this.Opcode = 0x48; 
        
        // Arguments
        this.AddArgument(new Int32Argument("Memory Index"));
    }

    public override string Description => "Convert the top of the stack from an 32bit float into a string on the heap. Requires cleanup after use.";

    public override void Action(VmValue[] args, RuntimeEnvironment runtime) {
        var i = runtime.Stack.PopTop().Float32;
        var value = i.ToString();
        var bytes = value.Length;
        var memIdx = ((Operand)args[0]).Int32;

        var memory = runtime.Memories[memIdx];
        Console.WriteLine("Converting " + i + " into " + value );
        Console.WriteLine("Saving  " + bytes + " bytes into memory " + memIdx );
        var address = memory.Reserve(bytes);
        for (int offset = 0; offset < value.Length; offset++) {
            var c = value[offset];
            memory.Write8(address + Memory.BlockHeaderSize.ByteCount + offset, (byte)c);
        }
        runtime.Stack.PushTop(Operand.From(address));
    }
}