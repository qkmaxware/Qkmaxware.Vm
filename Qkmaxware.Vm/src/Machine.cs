namespace Qkmaxware.Vm;

/// <summary>
/// Virtual machine
/// </summary>
public class Machine {

    private HostInterface host;
    private IRandomAccessMemory heap;

    public Machine() : this(HostInterface.Default(), new LinearByteArrayMemory(DataSize.Kibibytes(2))) {}

    public Machine(HostInterface host) : this(host, new LinearByteArrayMemory(DataSize.Kibibytes(2))) {}

    public Machine(HostInterface host, IRandomAccessMemory heap) {
        this.host = host;
        this.heap = heap;
    }

    public ThreadOfExecution LoadProgram(Module module) {
        return new ThreadOfExecution(module, new RuntimeEnvironment(module, host, heap));
    }

}

/// <summary>
/// Virtual machine thread of execution
/// </summary>
public class ThreadOfExecution {

    private Module module;
    private BinaryReader codeReader;
    public RuntimeEnvironment Environment {get; private set;}
    public long PC {
        get => codeReader.BaseStream.Position;
        set => codeReader.BaseStream.Position = value;
    }

    public bool HasNextInstruction => !(PC < 0 || PC >= codeReader.BaseStream.Length);

    public ThreadOfExecution(Module module, RuntimeEnvironment environment) {
        this.module = module;
        this.codeReader = new BinaryReader(new BytecodeStream(module));
        this.Environment = environment;
        environment.SetThread(this);
    }

    /// <summary>
    /// Run a module until the module is complete
    /// </summary>
    public void RunUntilComplete() {
        while (true) {
            var success = RunNext();
            if (!success)
                break;
        }
    }

    private HashSet<long> breakpoints = new HashSet<long>();
    /// <summary>
    /// Create a breakpoint in the module code
    /// </summary>
    /// <param name="index">instruction byte offset</param>
    public void AddBreakpoint(long index) {
        this.breakpoints.Add(index);
    }
    /// <summary>
    /// Enumerate a list of all breakpoints
    /// </summary>
    /// <returns>list of points</returns>
    public IEnumerable<long> EnumerateBreakpoints() {
        foreach (var bp in breakpoints)
            yield return bp;
    }
    /// <summary>
    /// Remove a breakpoint in the module code
    /// </summary>
    /// <param name="index">instruction byte offset</param>
    public void RemoveBreakpoint(long index) {
        this.breakpoints.Remove(index);
    }
    /// <summary>
    /// Clear all breakpoints
    /// </summary>
    public void ClearBreakpoints() {
        this.breakpoints.Clear();
    }

    /// <summary>
    /// Run a module until a breakpoint is encountered 
    /// </summary>
    public void RunUntilBreakpoint() {
        while (!this.breakpoints.Contains(PC)) {
            var success = RunNext();
            if (!success)
                break;
        }
    }

    /// <summary>
    /// Run the next instruction in the loaded module
    /// </summary>
    /// <returns>true if the instruction was able to execute</returns>
    public bool RunNext() {
        if (!HasNextInstruction) {
            return false;
        }

        // Fetch
        var opcode = codeReader.ReadByte();

        // Decode
        var instr = InstructionMap.Instance[opcode];
        if (instr == null) {
            throw new InvalidOpcodeException(opcode);
        }
        var args = new VmValue[instr.Arity];
        var arg = 0;
        foreach (var argSpec in instr.Arguments) {
            args[arg++] = argSpec.ReadValue(codeReader);
        }

        // Execute
        try {
            instr.Action(args, this.Environment);
            return true;
        } catch (VmExitRequestException) {
            return false;
        }
    }

}