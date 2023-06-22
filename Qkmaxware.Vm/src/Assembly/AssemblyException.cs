namespace Qkmaxware.Vm.Assembly;

/// <summary>
/// Wrapper exception for errors when assembling assembly
/// </summary>
public class AssemblyException : Exception {
    public long LineIndex {get; private set;}
    public long LineNumber => LineIndex + 1;
    public AssemblyException(long lineIndex, Exception inner) : base(inner.Message, inner) {
        this.LineIndex = lineIndex;
    }
}