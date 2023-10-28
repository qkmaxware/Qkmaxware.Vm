namespace Qkmaxware.Vm.Instructions;

/// <summary>
/// Euclidean modulus of 2 signed integers
/// </summary>
public class ModI32 : Instruction {

    public ModI32() {
        // Set opcode
        this.Opcode = 0x16; 
        
        // Arguments
    
        // Stack
        this.AddStackOperand("lhs");
        this.AddStackOperand("rhs");
        this.AddStackReturn("result");
    }

    public override string Description => "Arithmetic modulus between 2 integers at the top of the operand stack";

    public override void Action(VmValue[] args, RuntimeEnvironment runtime) {
        var rhs = runtime.Stack.PopTop();
        var lhs = runtime.Stack.PopTop();

        if (rhs.Int32 == -1) {
            runtime.Stack.PushTop(Operand.From(0));
            return;
        }

        var mod = lhs.Int32 - (int)Math.Floor((double)lhs.Int32 / rhs.Int32) * rhs.Int32;

        runtime.Stack.PushTop(Operand.From(mod));
    }
}