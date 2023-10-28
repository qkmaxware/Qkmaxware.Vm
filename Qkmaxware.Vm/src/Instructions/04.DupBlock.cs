namespace Qkmaxware.Vm.Instructions;

/// <summary>
/// Duplicate a block of values from the top of the stack
/// </summary>
public class DupBlock : Instruction {

    public DupBlock() {
        // Set opcode
        this.Opcode = 0x04; 
        
        // Arguments
        this.AddArgument(new Int32Argument("Block Size"));

        // Stack
        this.AddStackOperand("value1");
        this.AddStackOperand("...");
        this.AddStackOperand("valueN");
        
        this.AddStackReturn("value1");
        this.AddStackReturn("...");
        this.AddStackReturn("valueN");
        this.AddStackReturn("value1");
        this.AddStackReturn("...");
        this.AddStackReturn("valueN");
    }

    public override string Description => "Duplicate a block elements from the top of the operand stack.";

    public override void Action(VmValue[] args, RuntimeEnvironment runtime) {
        /* 
         * DupN(2)
         * [A,B,C,D] -> [A,B,C,D,C,D] 
        */
        var size = Math.Max(((Operand)args[0]).Int32, 0);
        var block = new Operand[size];
        for (var i = 0; i < size; i++) {
            var top = runtime.Stack.PopTop();
            block[block.Length - 1 - i] = top;
        }

        for (var duplicates = 0; duplicates < 2; duplicates++) {
            for (var i = 0; i < size; i++) {
                runtime.Stack.PushTop(block[i]);
            }
        }
    }
}