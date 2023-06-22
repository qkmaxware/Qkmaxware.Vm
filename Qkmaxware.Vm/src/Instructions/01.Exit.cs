namespace Qkmaxware.Vm.Instructions;

/// <summary>
/// VM exit operation
/// </summary>
public class Exit : Instruction {

    public Exit() {
        // Set opcode
        this.Opcode = 0x01; 
        
        // Arguments
        this.AddArgument(new Int32Argument("Status Code")); // Maybe switch to byte
    }

    public override string Description => "Exit the program with the given status code. Status code 0 should indicate successful execution of the program.";

    public override void Action(VmValue[] args, RuntimeEnvironment runtime) {
        var code = (Int32Operand)args[0];
        throw new VmExitRequestException(code.Value);
    }
}