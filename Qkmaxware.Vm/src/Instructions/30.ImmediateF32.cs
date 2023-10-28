namespace Qkmaxware.Vm.Instructions;

/// <summary>
/// Immediate 32 bit floating point value loading
/// </summary>
public class ImmediateF32 : Instruction {

    public ImmediateF32() {
        // Set opcode
        this.Opcode = 0x30; 
        
        // Arguments
        this.AddArgument(new Float32Argument("Immediate Value"));

        // Stack
        this.AddStackReturn("value");
    }

    public override string Description => "Push an immediate floating-point value on top of the operand stack.";

    public override void Action(VmValue[] args, RuntimeEnvironment runtime) {
        var immediate = (Operand)args[0];
        runtime.Stack.PushTop(immediate);
    }
}