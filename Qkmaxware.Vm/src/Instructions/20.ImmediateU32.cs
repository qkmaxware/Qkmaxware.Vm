namespace Qkmaxware.Vm.Instructions;

/// <summary>
/// Immediate 32 bit unsigned integer loading
/// </summary>
public class ImmediateU32 : Instruction {

    public ImmediateU32() {
        // Set opcode
        this.Opcode = 0x20; 
        
        // Arguments
        this.AddArgument(new UInt32Argument("Immediate Value"));

        // Stack
        this.AddStackReturn("value");
    }

    public override string Description => "Push an immediate unsigned integer value on top of the operand stack.";

    public override void Action(VmValue[] args, RuntimeEnvironment runtime) {
        var immediate = (Operand)args[0];
        runtime.Stack.PushTop(immediate);
    }
}