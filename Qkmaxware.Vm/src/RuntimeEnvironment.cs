namespace Qkmaxware.Vm;

/// <summary>
/// Environment of data accessible by instructions for the virtual machine.
/// </summary>
public class RuntimeEnvironment {
    /// <summary>
    /// Interface between the VM and the host machine
    /// </summary>
    /// <returns>interface</returns>
    public HostInterface Host {get; private set;}
    /// <summary>
    /// Constants accessible to the runtime
    /// </summary>
    /// <returns>constants</returns>
    public ConstantPool ConstantPool {get; private set;} = new ConstantPool();
    /// <summary>
    /// Pool of static variables accessible to the runtime
    /// </summary>
    /// <value>static variabels</value>
    public StaticPool StaticPool {get; private set;} = new StaticPool();
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

    public RuntimeEnvironment() {
        this.Host = HostInterface.Default();
    }

    public RuntimeEnvironment(Module module, HostInterface @interface, IRandomAccessMemory heap) {
        this.Host = @interface;
        this.StaticPool = module.StaticPool.Clone();
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