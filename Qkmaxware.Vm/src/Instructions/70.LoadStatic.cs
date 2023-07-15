namespace Qkmaxware.Vm.Instructions;

/// <summary>
/// Load a constant from the static pool onto the top of the stack
/// </summary>
public class LoadStatic : Instruction {

    public LoadStatic() {
        // Set opcode
        this.Opcode = 0x70; 
        
        // Arguments
        this.AddArgument(new Int32Argument("Static Index"));
    }

    public override string Description => "Push a value from the static pool onto the top of the stack.";

    public override void Action(VmValue[] args, RuntimeEnvironment runtime) {
        var index = (Operand)args[0];
        runtime.Stack.PushTop(
            runtime.StaticPool[index.Int32]
        );
    }
}