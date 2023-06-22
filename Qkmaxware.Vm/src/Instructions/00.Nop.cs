namespace Qkmaxware.Vm.Instructions;

/// <summary>
/// Nop or "no-operation" instruction
/// </summary>
public class Nop : Instruction {

    public Nop() {
        // Set opcode
        this.Opcode = 0x00; 
        
        // Arguments
    }

    public override string Description => "Do nothing.";

    public override void Action(VmValue[] args, RuntimeEnvironment runtime) { }
}