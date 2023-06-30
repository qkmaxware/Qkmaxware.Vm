namespace Qkmaxware.Vm.Instructions;

/// <summary>
/// Read a single character from the input stream and add it to the top of the stack
/// </summary>
public class Readchar : Instruction {

    public Readchar() {
        // Set opcode
        this.Opcode = 0x81; 
        
        // Arguments
    }

    public override string Description => "Read a single character from the input stream and add it to the top of the stack.";

    public override void Action(VmValue[] args, RuntimeEnvironment runtime) {
        var read = runtime.Host.StdIn.Read();
        if (read == -1)
            runtime.Stack.PushTop(Operand.From('\0'));
        else 
            runtime.Stack.PushTop(Operand.From((char)read));
    }
}