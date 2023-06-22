namespace Qkmaxware.Vm.Instructions;

/// <summary>
/// Invoke an imported subprogram
/// </summary>
public class CallExternal : Instruction {
    
    public CallExternal() {
        // Set opcode
        this.Opcode = 0x64; 
        
        // Arguments
        this.AddArgument(new Int32Argument("Import Index"));
        this.AddArgument(new Int32Argument("Argument Count"));
    }

    public override string Description => "Call an imported subprogram at the given import index.";

    public override void Action(VmValue[] args, RuntimeEnvironment runtime) {
        throw new MissingMethodException();
    }
}