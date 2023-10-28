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
    /// Current operands on stack
    /// </summary>
    /// <returns>stack</returns>
    public OperandStack Stack {get; private set;} = new OperandStack();
    /// <summary>
    /// List of all memories in the module
    /// </summary>
    public List<Memory> Memories {get; private set;}

    public RuntimeEnvironment() {
        this.Host = HostInterface.Default();
        this.Memories = new List<Memory>();
    }

    public RuntimeEnvironment(Module module, HostInterface @interface, List<Memory> memories) {
        this.Host = @interface;
        this.Memories = memories;
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