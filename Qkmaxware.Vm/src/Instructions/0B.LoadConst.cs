namespace Qkmaxware.Vm.Instructions;

/// <summary>
/// Load a constant from the constant pool onto the top of the stack
/// </summary>
public class LoadConst : Instruction {

    public LoadConst() {
        // Set opcode
        this.Opcode = 0x0B; 
        
        // Arguments
        this.AddArgument(new Int32Argument("Constant Index"));
    }

    public override string Description => "Push a value from the constant pool onto the top of the stack.";

    public override void Action(VmValue[] args, RuntimeEnvironment runtime) {
        var index = (Operand)args[0];
        runtime.Stack.PushTop(
            runtime.ConstantPool[index.Int32].LoadOperand()
        );
    }
}