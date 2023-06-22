namespace Qkmaxware.Vm.Instructions;

/// <summary>
/// Convert an integer to a floating point number
/// </summary>
public class I32ToF32 : Instruction {

    public I32ToF32() {
        // Set opcode
        this.Opcode = 0x40; 
        
        // Arguments
    }

    public override string Description => "Convert the top of the stack from an 32bit integer to a 32bit floating point number.";

    public override void Action(VmValue[] args, RuntimeEnvironment runtime) {
        var value = runtime.Stack.PopTop().Int32;
        runtime.Stack.PushTop(Operand.From((float)value));
    }
}