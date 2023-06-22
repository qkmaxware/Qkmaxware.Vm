namespace Qkmaxware.Vm;

internal class Module1Decoder : IModuleDecoder {

    public bool SupportsVersion(int Major, int Minor) => Major == 1;

    private List<ConstantInfo> supportedConstantTypes = new List<ConstantInfo> {
        ConstantInfo.Int32,
        ConstantInfo.UInt32,
        ConstantInfo.Float32,

        ConstantInfo.Ascii,
        ConstantInfo.Utf8,
        ConstantInfo.Utf32
    };

    public void DecodeData(Module module, BinaryReader reader) {
        // -----------------------------------------------------------
        // Read Data
        // -----------------------------------------------------------
        var length = reader.ReadInt32();
        module.Code.EnsureCapacity(length);
        for (var i = 0; i < length; i++) {
            module.Code.Add(reader.ReadByte());
        }

        var count = reader.ReadInt32();
        module.ConstantPool.EnsureCapacity(count);
        for (var i = 0; i < count; i++) {
            // Read constant info header
            var tag = reader.ReadByte();
            
            // Read constant value
            var typeInfo = supportedConstantTypes.Where(info => info.TypeTag == tag).FirstOrDefault();
            if (typeInfo == null)
                throw new ArgumentException("Constants of type '{tag}' and not supported in the version of Qkmaxware Bytecode");
            var constant = typeInfo.Decode(reader);
            module.ConstantPool.Add(constant);
        }
    }
}