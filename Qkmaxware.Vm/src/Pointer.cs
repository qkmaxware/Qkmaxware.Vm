namespace Qkmaxware.Vm;

public enum PointerType : uint {
    ConstantPoolIndex   = 0b10000000_00000000_00000000_00000000,
    HeapAddress         = 0b00000000_00000000_00000000_00000000,
}

public struct Pointer {
    private uint value;

    public uint UIntValue => value;

    public int IntValue {
        get => (int)(value & ~(1 << 31)); // Remove high bit
    }

    internal Pointer(uint value) {
        this.value = value;
    }

    public Pointer(PointerType type, int value) {
        if (value < 0)
            value = 0;
        this.value = BitConverter.ToUInt32(BitConverter.GetBytes(value)) | (uint)type;
    }

    public Pointer(ConstantData constant) : this(PointerType.ConstantPoolIndex, constant.PoolIndex) {}

    public bool IsConstantPoolIndex() => ((uint)PointerType.ConstantPoolIndex & value) == (uint)PointerType.ConstantPoolIndex;
    public bool IsHeapAddress() => !(IsConstantPoolIndex()); //((uint)PointerType.HeapAddress & value) == (uint)PointerType.HeapAddress;
}