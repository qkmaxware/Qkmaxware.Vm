namespace Qkmaxware.Vm.Instructions;

/// <summary>
/// Immediate 32 bit signed integer loading
/// </summary>
public class ImmediateI32 : Instruction {

    public ImmediateI32() {
        // Set opcode
        this.Opcode = 0x10; 
        
        // Arguments
        this.AddArgument(new Int32Argument("Immediate Value"));
    }

    public override string Description => "Push an immediate integer value on top of the operand stack.";

    public override void Action(VmValue[] args, RuntimeEnvironment runtime) {
        var immediate = (Int32Operand)args[0];
        runtime.Stack.PushTop(immediate);
    }
}