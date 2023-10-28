using System.Reflection.Metadata;
using Qkmaxware.Vm.Instructions;

namespace Qkmaxware.Vm;

/// <summary>
/// Builder to simplify the creation of bytecode modules programmatically
/// </summary>
public partial class ModuleBuilder {
    /// <summary>
    /// Declare a name as an imported label
    /// </summary>
    /// <param name="name">subprogram external name</param>
    /// <returns>import reference</returns>
    public Vm.Import ImportSubprogram(string name) {
        var imp = new Vm.Import(name);
        this.imports.Add(imp);
        return imp;
    }

    /// <summary>
    /// Checks if a subprogram has been imported. If so it gets the import's index.
    /// </summary>
    /// <param name="name">subprogram name</param>
    /// <param name="import">subprogram index</param>
    /// <returns>true if a subprogram with the given name has been imported</returns>
    public bool HasImportedSubprogram(string? name, out int index) {
        index = -1;
        for (var i = 0; i < imports.Count; i++) {
            if (imports[i].Name == name) {
                index = i;
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Declare this spot of the code to be an exported subprogram
    /// </summary>
    /// <param name="name">subprogram external name</param>
    /// <returns>exported reference</returns>
    public Vm.Export ExportSubprogram(string name) {
        // Generate unique name
        var unique_index = 0;
        var unique_name = name;
        while(export_names.Contains(unique_name)) {
            unique_name = name + (unique_index++);
        }

        // Store it
        var index = bytecode.BaseStream.Position;
        export_names.Add(name);
        var export = new Export(name, (int)index);
        exports.Add(export);
        return export;
    }

    /// <summary>
    /// Create a named label to the next instruction in the code section
    /// </summary>
    /// <param name="str">desired label name</param>
    /// <returns>label</returns>
    public Label Label(string str) {
        // Generate unique name
        var unique_index = 0;
        var unique_name = str;
        while(labels.ContainsKey(unique_name)) {
            unique_name = str + (unique_index++);
        }

        // Store it
        var index = bytecode.BaseStream.Position;
        labels[unique_name] = index;
        return new Label(unique_name, index);
    }
    
    /// <summary>
    /// Get an anchor to the next instruction in the code section
    /// </summary>
    /// <returns></returns>
    public long Anchor() {
        return bytecode.BaseStream.Position;
    }

    /// <summary>
    /// Rewind the builder stream to the given position. Dangerous only use if you know what you are doing.
    /// </summary>
    /// <param name="position">stream position to rewind to</param>
    public void RewindStream(long position) {
        bytecode.BaseStream.Position = position;
    }

    /// <summary>
    /// Push a new instruction into the code section
    /// </summary>
    /// <param name="instr">instruction to add</param>
    /// <param name="args">instruction arguments</param>
    public void AddInstruction(IInstruction instr, params VmValue[] args) {
        // Verify arity
        if (args.Length != instr.Arity) {
            throw new ArgumentException($"Wrong number of arguments for instruction '{instr.Name}'.");
        }
        
        // Encode to bytecode
        this.bytecode.Write(instr.Opcode);
        foreach (var arg in args) {
            arg.WriteValue(this.bytecode);
        }
    }

    /// <summary>
    /// Push a new instruction into the code section
    /// </summary>
    /// <param name="instr_code">instruction's opcode</param>
    /// <param name="args">instruction arguments</param>
    public void AddInstruction(byte instr_code, params VmValue[] args) {
        var instr = InstructionMap.Instance[instr_code];
        if (instr == null)  
            throw new ArgumentException($"No instruction with opcode '{instr_code}'");
        AddInstruction(instr, args);
    }

    /// <summary>
    /// Push a new instruction into the code section
    /// </summary>
    /// <param name="instr_code">instruction's name</param>
    /// <param name="args">instruction arguments</param>
    public void AddInstruction(string instr_name, params VmValue[] args) {
        var instr = InstructionMap.Instance[instr_name];
        if (instr == null)  
            throw new ArgumentException($"No instruction with name '{instr_name}'");
        AddInstruction(instr, args);
    }

    /// <summary>
    /// Append the contents of this module into this builder
    /// </summary>
    /// <param name="module">module to append</param>
    public void Append(Module module) {
        var code_offset = this.Anchor();
        var memories_offset = this._additionalMems.Count + this.AdditionalMemoryOffsetIndex;

        // Add imports and exports, modify exports to point to new code location
        this.imports.AddRange(module.Imports);
        this.exports.AddRange(module.Exports.Select(x => new Export(x.Name, (int)(code_offset + x.CodePosition))));

        // Update constant pool
        this._additionalMems.AddRange(module.Memories);

        // Update code, point to new constants when required to.
        // Since we aren't linking we don't need to update import and export references (call external)
        var dis = new Disassembler();
        foreach (var instr in dis.DisassembleCode(module)) {
            if (instr.Instruction is IMemoryAccessInstruction) {
                // Re-write the index
                var new_index = Operand.From(((Operand)instr.Arguments.ElementAt(0)).Int32 + memories_offset);
                this.AddInstruction(instr.Instruction, new VmValue[]{ new_index });
            } else {
                this.AddInstruction(instr.Instruction, instr.Arguments.ToArray());
            }
        }
    }
}