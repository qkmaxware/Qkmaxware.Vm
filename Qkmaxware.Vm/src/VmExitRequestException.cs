namespace Qkmaxware.Vm;

public class VmExitRequestException : Exception {
    public int ExitCode {get; private set;}
    public VmExitRequestException(int status) : base() {
        this.ExitCode = status;
    }  
}