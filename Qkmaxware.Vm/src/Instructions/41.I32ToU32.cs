namespace Qkmaxware.Vm.Instructions;

/// <summary>
/// Convert an integer to an unsigned integer
/// </summary>
public class I32ToU32 : Instruction {

    public I32ToU32() {
        // Set opcode
        this.Opcode = 0x41; 
        
        // Arguments
    }

    public override string Description => "Convert the top of the stack from an 32bit integer to a 32bit unsigned integer.";

    public override void Action(VmValue[] args, RuntimeEnvironment runtime) {
        var value = runtime.Stack.PopTop().Int32;
        runtime.Stack.PushTop(Operand.From((uint)value));
    }
}