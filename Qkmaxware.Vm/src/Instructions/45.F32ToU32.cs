namespace Qkmaxware.Vm.Instructions;

/// <summary>
/// Convert a float to an unsigned integer
/// </summary>
public class F32ToU32 : Instruction {

    public F32ToU32() {
        // Set opcode
        this.Opcode = 0x45; 
        
        // Arguments
    }

    public override string Description => "Convert the top of the stack from an 32bit floating point number to a 32bit unsigned integer.";

    public override void Action(VmValue[] args, RuntimeEnvironment runtime) {
        var value = runtime.Stack.PopTop().Float32;
        runtime.Stack.PushTop(Operand.From((uint)value));
    }
}