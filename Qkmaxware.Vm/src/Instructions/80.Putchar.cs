namespace Qkmaxware.Vm.Instructions;

/// <summary>
/// Print the top of the stack as a character to the output stream
/// </summary>
public class Putchar : Instruction {

    public Putchar() {
        // Set opcode
        this.Opcode = 0x80; 
        
        // Arguments

        // Stack
        this.AddStackOperand("character");
    }

    public override string Description => "Treat the element at the top of the stack as a character and print it to the standard output device.";

    public override void Action(VmValue[] args, RuntimeEnvironment runtime) {
        var ptr = runtime.Stack.PopTop();
        var character = (char)ptr.Int32;
        runtime.Host.StdOut.Write(character);
    }
}