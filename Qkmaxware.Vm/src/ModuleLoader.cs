using System.Linq;

namespace Qkmaxware.Vm;

/// <summary>
/// Load a bytecode module from a given file
/// </summary>
public class ModuleLoader {
    private List<IModuleDecoder> decoders = new List<IModuleDecoder>();

    public ModuleLoader() {
        registerAllDecoders();
    }

    public void RegisterDecoder(IModuleDecoder decoder) {
        this.decoders.Add(decoder);
    }

    private void registerAllDecoders() {
        var all_decoders = typeof(ModuleLoader).Assembly
            .GetTypes()
            .Where((type) => type.IsClass && !type.IsAbstract && type.IsAssignableTo(typeof(IModuleDecoder)))
            .Select((type) => (IModuleDecoder?)Activator.CreateInstance(type));
        foreach (var decoder in all_decoders) {
            if (decoder == null)
                continue;
            RegisterDecoder(decoder);
        }
    }

    /// <summary>
    /// Load a module from a file at the given path
    /// </summary>
    /// <param name="path">path to file</param>
    /// <returns>module</returns>
    public Module FromFile(string path) {
        using (var reader = new BinaryReader(File.OpenRead(path))) {
            return FromStream(reader);
        }
    }

    /// <summary>
    /// Load a module from a given file
    /// </summary>
    /// <param name="file">file reference</param>
    /// <returns>module</returns>
    public Module FromFile(FileInfo file) {
        return FromFile(file.FullName);
    }

    /// <summary>
    /// Load a module from a stream wrapped in a binary reader
    /// </summary>
    /// <param name="reader">stream reader</param>
    /// <returns>module</returns>
    public Module FromStream(BinaryReader reader) {
        // -----------------------------------------------------------
        // Read Header
        // -----------------------------------------------------------
        foreach (var magic_byte in Module.MagicNumbers) {
            if (reader.ReadByte() != magic_byte) {
                throw new ArgumentException("File is not a Qkmaxware Bytecode Module.");
            }
        }
        var major = reader.ReadInt32();
        var minor = reader.ReadInt32();

        // -----------------------------------------------------------
        // Read Data
        // -----------------------------------------------------------
        // Create empty module
        Module mod = new Module();
        
        // Find first decoder that supports this version
        var decoder = decoders.Where(decoder => decoder.SupportsVersion(major, minor)).FirstOrDefault();
        if (decoder == null) {
            throw new ArgumentException($"Qkmaxware Bytecode version {major}.{minor} is not supported on this platform.");
        }

        // Do decoding
        decoder.DecodeData(mod, reader);
        return mod;
    }

}