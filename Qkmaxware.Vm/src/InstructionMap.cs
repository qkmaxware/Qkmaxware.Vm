using System.Collections;

namespace Qkmaxware.Vm;

/// <summary>
/// Mapping of all instructions 
/// </summary>
public class InstructionMap : IEnumerable<IInstruction> {

    private static InstructionMap? _instance;

    /// <summary>
    /// Singleton instance
    /// </summary>
    /// <returns>mapping</returns>
    public static InstructionMap Instance {
        get {
            if (_instance == null) {
                _instance = new InstructionMap(); // Lazy loading of the map in case we never use it
                return _instance;
            } else {
                return _instance;
            }
        }
    }

    private InstructionMap() {
        /*
        this.Add(new Add());
        this.Add(new Sub());
        ...
        this.Add(new Return());
        */
        var all_instructions = typeof(InstructionMap).Assembly
            .GetTypes()
            .Where((type) => type.IsClass && !type.IsAbstract && type.IsAssignableTo(typeof(IInstruction)))
            .Select((type) => (IInstruction?)Activator.CreateInstance(type));
        foreach (var instr in all_instructions) {
            if (instr == null)
                continue;
            Add(instr);
        }
    }

    private Dictionary<string, IInstruction> by_name = new Dictionary<string, IInstruction>();
    private IInstruction?[] by_opcode = new IInstruction?[255];

    /// <summary>
    /// Test to see if any instructions have been added to this map or not
    /// </summary>
    /// <value>true if no instructions have been added to the map, false otherwise</value>
    public bool IsEmpty {get; private set;} = true;

    private void Add(IInstruction instruction) {
        if (instruction == null)
            return;

        by_name[instruction.Name] = instruction;
        by_opcode[instruction.Opcode] = instruction;
        IsEmpty = false;
    }

    /// <summary>
    /// Fetch an instruction by it's opcode number. Null if instruction doesn't exist.
    /// </summary>
    public IInstruction? this[byte opcode] => opcode < 0 || opcode >= by_opcode.Length ? null : by_opcode[opcode];
    
    /// <summary>
    /// Fetch an instruction by its "human-readable" name. Null if the instruction doesn't exist.
    /// </summary>
    public IInstruction? this[string name] {
        get {
            IInstruction? fetched = null;
            by_name.TryGetValue(name, out fetched);
            return fetched;
        }
    }

    public IEnumerator<IInstruction> GetEnumerator() {
        for (int i = 0; i < this.by_opcode.Length; i++) {
            var op = this.by_opcode[i];
            if (op != null)
                yield return op;
        }
    }

    IEnumerator IEnumerable.GetEnumerator() {
        return this.GetEnumerator();
    }
}