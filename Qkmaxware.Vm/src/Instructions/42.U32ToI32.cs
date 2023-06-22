namespace Qkmaxware.Vm.Instructions;

/// <summary>
/// Convert an unsigned integer to an integer
/// </summary>
public class U32ToI32 : Instruction {

    public U32ToI32() {
        // Set opcode
        this.Opcode = 0x42; 
        
        // Arguments
    }

    public override string Description => "Convert the top of the stack from an 32bit unsigned integer to a 32bit integer.";

    public override void Action(VmValue[] args, RuntimeEnvironment runtime) {
        var value = runtime.Stack.PopTop().UInt32;
        runtime.Stack.PushTop(Operand.From((int)value));
    }
}