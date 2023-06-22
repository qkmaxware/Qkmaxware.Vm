using System.Text.RegularExpressions;

namespace Qkmaxware.Vm.Assembly;

/// <summary>
/// Assembler from textual assembly to module 
/// </summary>
public class Assembler {

    public List<IAssemblyParser> parsers = new List<IAssemblyParser>();

    public Assembler() {
        // Register default parsers
        registerAllParsers();
    }

    private void registerAllParsers() {
        var all_parser = typeof(Assembler).Assembly
            .GetTypes()
            .Where((type) => type.IsClass && !type.IsAbstract && type.IsAssignableTo(typeof(IAssemblyParser)))
            .Select((type) => (IAssemblyParser?)Activator.CreateInstance(type));
        foreach (var parser in all_parser) {
            if (parser == null)
                continue;
            RegisterParser(parser);
        }
    }

    /// <summary>
    /// Add a parser for a given dialect and version to this assembler
    /// </summary>
    /// <param name="parser">dialect parser</param>
    public void RegisterParser(IAssemblyParser parser) {
        this.parsers.Add(parser);
    }

    /// <summary>
    /// Assemble content from a file at the given path
    /// </summary>
    /// <param name="pathlike">path to file</param>
    /// <returns>assembled module</returns>
    public Module FromFile(string pathlike) {
        using (var reader = new StreamReader(pathlike)) {
            return FromStream(reader);
        }
    }

    /// <summary>
    /// Assemble content from a file at the given reference
    /// </summary>
    /// <param name="file">file reference</param>
    /// <returns>assembled module</returns>
    public Module FromFile(FileInfo file) {
        return FromFile(file.FullName);
    }

    /// <summary>
    /// Check the header of an assembly file to see if it is valid
    /// </summary>
    /// <param name="reader">file reader</param>
    /// <param name="dialect">assembly dialect</param>
    /// <param name="major_version">dialect major version number</param>
    /// <param name="minor_version">dialect minor version number</param>
    /// <returns>true if the content has a valid assembly header</returns>
    public static bool IsContentAssembly(TextReader reader, out string dialect, out int major_version, out int minor_version) {
        var headerStr = reader.ReadLine() ?? string.Empty;
        var match = headerPattern.Match(headerStr);
        if (!match.Success) {
            dialect = string.Empty;
            major_version = 0;
            minor_version = 0;
            return false;
        } else {
                dialect = match.Groups["dialect"].Value;
                major_version = int.Parse(match.Groups["major_version"].Value);
                minor_version = int.Parse(match.Groups["minor_version"].Value);
            return true;
        }
    }

    private static Regex headerPattern = new Regex(@"^\s*use\s+(?<dialect>\w+)\s+(?<major_version>\d+)\.(?<minor_version>\d+)\s*$");
    /// <summary>
    /// Assemble content from a character stream
    /// </summary>
    /// <param name="reader">text reader wrapping a character stream</param>
    /// <returns>assembled module</returns>
    public Module FromStream(TextReader reader) {
        // Make sure this is a valid Qkmaxware Assembly file
        string dialect;
        int major;
        int minor;
        if (!IsContentAssembly(reader, out dialect, out major, out minor)) {
            throw new ArgumentException("File is not a valid Qkmaxware Assembly file");
        }

        // Find a valid parser for this dialect & version
        var parser = this.parsers.Where(p => p.SupportsVersion(dialect, major, minor)).FirstOrDefault();
        if (parser == null) {
            throw new ArgumentException($"No supported parser for dialect '{dialect}' version {major}.{minor}");
        }

        // Parse
        return parser.Parse(reader);
    }
}