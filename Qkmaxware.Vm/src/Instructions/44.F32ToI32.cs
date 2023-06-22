namespace Qkmaxware.Vm.Instructions;

/// <summary>
/// Convert a float to an integer
/// </summary>
public class F32ToI32 : Instruction {

    public F32ToI32() {
        // Set opcode
        this.Opcode = 0x44; 
        
        // Arguments
    }

    public override string Description => "Convert the top of the stack from an 32bit floating point number to a 32bit integer.";

    public override void Action(VmValue[] args, RuntimeEnvironment runtime) {
        var value = runtime.Stack.PopTop().Float32;
        runtime.Stack.PushTop(Operand.From((int)value));
    }
}