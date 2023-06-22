namespace Qkmaxware.Vm.Assembly;

/// <summary>
/// Interface for a assembly parser
/// </summary>
public interface IAssemblyParser {
    /// <summary>
    /// Check if this decoder supports the given module version
    /// </summary>
    /// <param name="dialect">assembly dialect name</param>
    /// <param name="Major">module major version</param>
    /// <param name="Minor">module minor version</param>
    /// <returns>true if module data can be decoded</returns>
    public bool SupportsVersion(string dialect, int Major, int Minor);
    /// <summary>
    /// Parse module assembly code
    /// </summary>
    /// <param name="reader">reader to read data from</param>
    public Module Parse(TextReader reader);
}