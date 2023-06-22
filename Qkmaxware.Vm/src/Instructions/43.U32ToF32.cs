namespace Qkmaxware.Vm.Instructions;

/// <summary>
/// Convert an unsigned integer to a float
/// </summary>
public class U32ToF32 : Instruction {

    public U32ToF32() {
        // Set opcode
        this.Opcode = 0x43; 
        
        // Arguments
    }

    public override string Description => "Convert the top of the stack from an 32bit unsigned integer to a 32bit floating point value.";

    public override void Action(VmValue[] args, RuntimeEnvironment runtime) {
        var value = runtime.Stack.PopTop().UInt32;
        runtime.Stack.PushTop(Operand.From((float)value));
    }
}