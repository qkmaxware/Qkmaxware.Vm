namespace Qkmaxware.Vm.Instructions;

/// <summary>
/// Unconditionally jump to a position in the program using it's offset from the current position
/// </summary>
public class Goto : Instruction {

    public Goto() {
        // Set opcode
        this.Opcode = 0x60; 
        
        // Arguments
        this.AddArgument(new Int32Argument("Offset"));
    }

    public override string Description => "Jump to another position in the program at PC + Offset.";

    public override void Action(VmValue[] args, RuntimeEnvironment runtime) {
        var offset = ((Int32Operand)args[0]);
        
        var now = runtime.PC;
        var next = now + offset.Value;
        runtime.PC = next;
    }
}