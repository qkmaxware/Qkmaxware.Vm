using System.Text.RegularExpressions;

namespace Qkmaxware.Vm;

/// <summary>
/// Base class for the majority of virtual machine instructions
/// </summary>
public abstract class Instruction : IInstruction {
    public byte Opcode {get; protected set;}
    
    public abstract string Description {get;}

    private static readonly Regex camelCase = new Regex(
        @"(?<!^)[A-Z]"
    );
    public string Name {
        get {
            // IAdd => i_add
            return camelCase.Replace(this.GetType().Name, (match) => {
                return "_" + match.Value;
            }).ToLower();
        }
    }

    private List<Argument> _arguments = new List<Argument>();
    /// <summary>
    /// Add an argument to this instruction
    /// </summary>
    /// <param name="arg">argument to add</param>
    protected void AddArgument(Argument arg) {
        _arguments.Add(arg);
    }
    public int Arity => _arguments.Count;
    public IEnumerable<Argument> Arguments => _arguments.AsReadOnly();


    public abstract void Action (VmValue[] args, RuntimeEnvironment runtime);
}