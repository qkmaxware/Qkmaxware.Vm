namespace Qkmaxware.Vm;

/// <summary>
/// Interface between the VM and functionality of the host computer
/// </summary>
public class HostInterface {

    /// <summary>
    /// Standard input reader
    /// </summary>
    /// <value>reader</value>
    public TextReader StdIn {get; private set;}
    
    /// <summary>
    /// Standard output writer
    /// </summary>
    /// <value>writer</value>
    public TextWriter StdOut {get; private set;}

    public HostInterface(TextReader stdin, TextWriter stdout) {
        this.StdIn = stdin;
        this.StdOut = stdout;
    }

    public static HostInterface Default() {
        return new HostInterface(
            stdin: System.Console.In,
            stdout: System.Console.Out
        );
    }

}