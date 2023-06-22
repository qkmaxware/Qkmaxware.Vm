using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using System.Reflection;

namespace Qkmaxware.Vm;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
internal class DataSizeUnit : Attribute {
    public string Unit {get; private set;}
    public DataSizeUnit(string unit) {
        this.Unit = unit;
    }
}

/// <summary>
/// Size of a block of data
/// /// </summary>
public class DataSize : IParsable<DataSize> {
    public int ByteCount {get; private set;}

    private DataSize(int bytes) {
        this.ByteCount = bytes;
    }

    // B or byte or bytes
    [DataSizeUnit("B")]
    [DataSizeUnit("byte")]
    [DataSizeUnit("bytes")]
    public static DataSize Bytes(int count) {
        return new DataSize(count);
    }

    // kB or Kilobyte or Kilobytes
    [DataSizeUnit("kB")]
    [DataSizeUnit("kilobyte")]
    [DataSizeUnit("kilobytes")]
    public static DataSize Kilobytes(int count) {
        return Bytes(count * 1000);
    }

    // kiB or Kibibytes or Kibibyte
    [DataSizeUnit("KiB")]
    [DataSizeUnit("kibibyte")]
    [DataSizeUnit("kibibytes")]
    public static DataSize Kibibytes(int count) {
        return Bytes(count * 1024);
    }

    // MB or Megabytes or Megabyte
    [DataSizeUnit("MB")]
    [DataSizeUnit("megabyte")]
    [DataSizeUnit("megabytes")]
    public static DataSize Megabytes(int count) {
        return Bytes(count * 1000000);
    }

    // MiB or Mebibytes or Mebibyte
    [DataSizeUnit("MiB")]
    [DataSizeUnit("mebibyte")]
    [DataSizeUnit("mebibytes")]
    public static DataSize Mebibytes(int count) {
        return Bytes(count * 1049000);
    }

    // GB or Gigabytes or Gigabyte
    [DataSizeUnit("GB")]
    [DataSizeUnit("gigabyte")]
    [DataSizeUnit("gigabytes")]
    public static DataSize Gigabytes(int count) {
        return Bytes(count * 1000000000);
    }

    // GiB or Gibibytes or Gibibyte
    [DataSizeUnit("GiB")]
    [DataSizeUnit("gibibyte")]
    [DataSizeUnit("gibibytes")]
    public static DataSize Gibibytes(int count) {
        return Bytes(count * 1074000000);
    }

    public override string ToString() {
        return this.ByteCount + "bytes";
    }

    // <Some Integer Number> <Some Unit of Measure>
    private static Regex format = new Regex(@"(?<value>\d+)\s*(?<unit>\w+)");
    public static DataSize Parse(string s, IFormatProvider? provider) {
        var match = format.Match(s);
        if (!match.Success)
            throw new FormatException("Invalid memory size format. Expecting sizes to be in the form of <integer> <unit>.");
        
        var value = int.Parse(match.Groups["value"].Value);
        var unit = (match.Groups["unit"].Value);

        var factory = typeof(DataSize)
        .GetMethods(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public)
        .Where(method => !method.IsVirtual 
                      && Attribute.IsDefined(method, typeof(DataSizeUnit)) 
                      && method.ReturnType == typeof(DataSize)
                      && method.GetCustomAttributes<DataSizeUnit>().Where(spec => spec.Unit == unit).Any()
        ).FirstOrDefault();

        if (factory == null) {
            throw new FormatException($"Unknown memory size unit of measure '{unit}'.");
        }

        #nullable disable
        var size = (DataSize)factory.Invoke(null, new object[]{ value });
        return size;
        #nullable restore
    }

    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out DataSize result) {
        result = null;

        if (s == null) { 
            return false;
        }

        var match = format.Match(s);
        if (!match.Success)
            return false;

        var value = int.Parse(match.Groups["value"].Value);
        var unit = (match.Groups["unit"].Value);

        var factory = typeof(DataSize)
        .GetMethods(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public)
        .Where(method => !method.IsVirtual 
                      && Attribute.IsDefined(method, typeof(DataSizeUnit)) 
                      && method.ReturnType == typeof(DataSize)
                      && method.GetCustomAttributes<DataSizeUnit>().Where(spec => spec.Unit == unit).Any()
        ).FirstOrDefault();

        if (factory == null) {
            throw new FormatException($"Unknown memory size unit of measure '{unit}'.");
        }

        #nullable disable
        var size = (DataSize)factory.Invoke(null, new object[]{ value });
        result = size;
        return true;
        #nullable restore
    }
}