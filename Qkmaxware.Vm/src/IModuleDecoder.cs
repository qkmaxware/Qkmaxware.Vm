namespace Qkmaxware.Vm;

/// <summary>
/// Base interface for module data decoders
/// </summary>
public interface IModuleDecoder {
    /// <summary>
    /// Check if this decoder supports the given module version
    /// </summary>
    /// <param name="Major">module major version</param>
    /// <param name="Minor">module minor version</param>
    /// <returns>true if module data can be decoded</returns>
    public bool SupportsVersion(int Major, int Minor);
    /// <summary>
    /// Decode module data, header should be removed prior to use.
    /// </summary>
    /// <param name="module">module to decode data into</param>
    /// <param name="reader">reader to decode data from</param>
    public void DecodeData(Module module, BinaryReader reader);
}