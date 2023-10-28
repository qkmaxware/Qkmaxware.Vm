namespace Qkmaxware.Vm;

/// <summary>
/// Interface representing an instruction for the virtual machine
/// </summary>
public interface IInstruction {
    /// <summary>
    /// Unique digits used to identify this instruction in a binary stream
    /// </summary>
    /// <value>operation code</value>
    public byte Opcode {get;}

    /// <summary>
    /// "human-readable" instruction name
    /// </summary>
    /// <value>instruction name</value>
    public string Name {get;}

    /// <summary>
    /// Number of arguments required for this instruction
    /// </summary>
    /// <value>argument count</value>
    public int Arity {get;}

    /// <summary>
    /// A description of what the instruction does
    /// </summary>
    /// <value>description</value>
    public string Description {get;}

    /// <summary>
    /// The arguments required to make this instruction work. Argument values are read from the binary stream after the opcode is recognized.
    /// </summary>
    /// <value>all arguments in order of appearance</value>
    public IEnumerable<Argument> Arguments {get;}

    /// <summary>
    /// The stack requirements for this instruction
    /// </summary>
    /// <value>names of stack elements, last element is the top of the stack</value>
    public IEnumerable<string> StackInputs {get;}

    /// <summary>
    /// The resulting stack after the instruction is executed
    /// </summary>
    /// <value>names of stack elements, last element is the top of the stack</value>
    public IEnumerable<string> StackOutputs {get;}

    /// <summary>
    /// The action that this instruction performs
    /// </summary>
    /// <param name="args">read argument values mapped 1-to-1 with the arguments from the Arguments property</param>
    /// <param name="runtime">current runtime of the virtual machine</param>
    public void Action(VmValue[] args, RuntimeEnvironment runtime);
}