namespace Qkmaxware.Vm;

/// <summary>
/// Environment of data accessible by instructions for the virtual machine.
/// </summary>
public class RuntimeEnvironment {
    /// <summary>
    /// Interface between the VM and the host machine
    /// </summary>
    /// <returns>interface</returns>
    public HostInterface Host {get; private set;} = HostInterface.Default();
    /// <summary>
    /// Constants accessible to the runtime
    /// </summary>
    /// <returns>constants</returns>
    public ConstantPool ConstantPool {get; private set;} = new ConstantPool();
    /// <summary>
    /// Current operands on stack
    /// </summary>
    /// <returns>stack</returns>
    public OperandStack Stack {get; private set;} = new OperandStack();

    /// <summary>
    /// Random access memory segment
    /// </summary>
    /// <value>heap</value>
    public IRandomAccessMemory Heap {get; private set;} = LinearByteArrayMemory.Zero;

    public RuntimeEnvironment() {}

    public RuntimeEnvironment(Module module, HostInterface @interface, IRandomAccessMemory heap) {
        this.Host = @interface;
        this.ConstantPool = module.ConstantPool;
        this.Heap = heap;
    }

    private ThreadOfExecution? thread;
    public void SetThread(ThreadOfExecution thread) {
        this.thread = thread;
    }

    /// <summary>
    /// Current program pointer
    /// </summary>
    /// <value>program pointer</value>
    public long PC {
        get {
            return thread?.PC ?? 0;
        }
        set {
            if (thread != null)
                thread.PC = value;
        }
    }
}