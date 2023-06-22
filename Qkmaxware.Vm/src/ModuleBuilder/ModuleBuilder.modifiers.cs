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
    /// Add a constant to the constant pool
    /// </summary>
    /// <param name="data">constant to add</param>
    public ConstantRef AddConstant(ConstantData data) {
        var @ref = new ConstantRef(constants.Count);
        constants.Add(data);
        return @ref;
    }

    /// <summary>
    /// Add an 32bit signed integer constant to the constant pool
    /// </summary>
    /// <param name="value">value to add</param>
    public ConstantRef AddConstantInt(Int32 value) {
        return this.AddConstant(new Int32Constant(value));
    }

    /// <summary>
    /// Add an 32bit unsigned integer constant to the constant pool
    /// </summary>
    /// <param name="value">value to add</param>
    public ConstantRef AddConstantUInt(UInt32 value) {
        return this.AddConstant(new UInt32Constant(value));
    }

    /// <summary>
    /// Add an 32bit floating point constant to the constant pool
    /// </summary>
    /// <param name="value">value to add</param>
    public ConstantRef AddConstantFloat(Single value) {
        return this.AddConstant(new Float32Constant(value));
    }   

    /// <summary>
    /// Add a string constant to the constant pool
    /// </summary>
    /// <param name="value">value to add</param>
    public ConstantRef AddConstantAsciiString(string str) {
        return this.AddConstant(new StringConstant(ConstantInfo.Ascii, str));
    }

    /// <summary>
    /// Add a string constant to the constant pool terminated by the null character
    /// </summary>
    /// <param name="value">value to add</param>
    public ConstantRef AddConstantAsciizString(string str) {
        return this.AddConstant(new StringConstant(ConstantInfo.Ascii, str + '\0'));
    }

    /// <summary>
    /// Add a string constant to the constant pool
    /// </summary>
    /// <param name="value">value to add</param>
    public ConstantRef AddConstantUtf8String(string str) {
        return this.AddConstant(new StringConstant(ConstantInfo.Utf8, str));
    }

    /// <summary>
    /// Add a string constant to the constant pool
    /// </summary>
    /// <param name="value">value to add</param>
    public ConstantRef AddConstantUtf32String(string str) {
        return this.AddConstant(new StringConstant(ConstantInfo.Utf32, str));
    }
}