namespace Qkmaxware.Vm;

internal class Module2xDecoder : IModuleDecoder {

    public bool SupportsVersion(int Major, int Minor) => Major == 2;

    public void DecodeData(Module module, BinaryReader reader) {
        // -----------------------------------------------------------
        // Read Data
        // -----------------------------------------------------------
        var exportLength = reader.ReadInt32();
        module.Exports.EnsureCapacity(exportLength);
        for (var i = 0; i < exportLength; i++) {
            var byteCount = reader.ReadInt32();
            var bytes = reader.ReadBytes(byteCount);
            var str = System.Text.Encoding.UTF8.GetString(bytes);
            var anchor = reader.ReadInt32();
            module.Exports.Add(new Export(str, anchor));
        }

        var importLength = reader.ReadInt32();
        module.Imports.EnsureCapacity(importLength);
        for (var i = 0; i < importLength; i++) {
            var byteCount = reader.ReadInt32();
            var bytes = reader.ReadBytes(byteCount);
            var str = System.Text.Encoding.UTF8.GetString(bytes);
            module.Imports.Add(new Import(str));
        }

        var length = reader.ReadInt32();
        module.Code.EnsureCapacity(length);
        for (var i = 0; i < length; i++) {
            module.Code.Add(reader.ReadByte());
        }

        var count = reader.ReadInt32();
        module.Memories.EnsureCapacity(count);
        for (var i = 0; i < count; i++) {
            module.Memories.Add(MemorySpec.Decode(reader));
        }
    }
}