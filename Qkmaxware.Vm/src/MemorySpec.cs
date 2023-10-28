namespace Qkmaxware.Vm;

public class Limits {
    private int _min;
    public int Min {
        get => _min;
        set => _min = Math.Max(0, value);
    }

    private int? _max;
    public int? Max {
        get => _max;
        set => _max = !value.HasValue || value.Value < 0 ? null : (Math.Max(value.Value, _min));
    }

    public Limits() {
        Min = 0;
        Max = null;
    }

    public Limits(DataSize min) {
        Min = min.ByteCount;
        Max = null;
    }

    public Limits (DataSize min, DataSize max) {
        var actualMin = DataSize.SmallestOf(min, max);
        var actualMax = DataSize.LargestOf(min, max);
        this.Min = actualMin.ByteCount;
        this.Max = actualMax.ByteCount;
    }

    public override string ToString() {
        var end = this.Max.HasValue ? this.Max.Value.ToString() : "inf";
        return $"{this.Min}..{end}";
    }
}

public enum Mutability : byte {
    None      = 0b00,
    ReadOnly  = 0b01,
    WriteOnly = 0b10,
    ReadWrite = 0b11,
}

public class MemorySpec {
    public Mutability Mutability {get; set;}
    public Limits Limits {get; set;}
    public List<Byte> Initializer {get; set;}

    public bool CanRead() => ((byte)Mutability & 0b01) > 0;
    public bool CanWrite() => ((byte)Mutability & 0b10) > 0;

    public MemorySpec(Mutability mutability, Limits limits, List<Byte>? initializer = null) {
        this.Mutability = mutability;
        this.Limits = limits;
        this.Initializer = initializer ?? new List<byte>();
    }

    public void Encode(BinaryWriter writer) {
        // Write the memory header
        writer.Write((byte)Mutability);
        writer.Write(Limits.Min);
        writer.Write(Limits.Max.HasValue ? Limits.Max.Value : -1);

        // Write the initializer
        if (ReferenceEquals(null, Initializer)) {
            writer.Write(0);
            return;
        }
        writer.Write(Initializer.Count);
        foreach (var b in Initializer) {
            writer.Write(b);
        }
    }

    public static MemorySpec Decode(BinaryReader reader) {
        // Read the memory header
        var mutability = (Mutability)reader.ReadByte();
        var min = reader.ReadInt32();
        var max = reader.ReadInt32();

        // Read the initializer
        var count = reader.ReadInt32();
        var bytes = new List<byte>(count);
        for (var i = 0; i < count; i++) {
            bytes.Add(reader.ReadByte());
        }

        return new MemorySpec(
            mutability, 
            new Limits {
                Max = max,
                Min = min
            },
            bytes
        );
    }

    public Memory Instantiate() {
        var initialSize = this.Limits.Min;
        var mem = new LinearByteArrayMemory(DataSize.Bytes(initialSize));
        //Console.WriteLine("Creating memory of " + mem.Size.ByteCount + "bytes based on spec of " + this.Limits);
        
        // Write the initialized memory
        var memory = new Memory(
            this.Mutability, 
            DataSize.Bytes(this.Limits.Min), 
            this.Limits.Max.HasValue ? DataSize.Bytes(Math.Max(this.Limits.Max.Value, this.Limits.Min)) : null,
            mem
        );
        // Initialize
        if (ReferenceEquals(this.Initializer, null) || this.Initializer.Count == 0) {
            //Console.WriteLine("     No initializer");
            // If there was no initialized data do default initialization
            memory.InitBlock();
        } else {
            // We have an initializer, copy it in byte by byte
            var initializerSize = this.Initializer.Count < mem.Size.ByteCount
                ? this.Initializer.Count
                : mem.Size.ByteCount;
            //Console.Write("     Initializing with " + initializerSize + "bytes [");
            for (var i = 0; i < initializerSize; i++) {
                //Console.Write(this.Initializer[i] + " ");
                mem.WriteByte(i, this.Initializer[i]);
            }
            //Console.WriteLine("]");
        }
        // Return the initialized memory
        return memory;
    }
}

public class Memory {
    public Mutability Mutability {get; private set;}
    public DataSize MinSize {get; private set;}
    public DataSize CurrentSize => backing.Size;
    public DataSize? MaxSize {get; private set;}
    private IRandomAccessMemory backing;

    public string ASCII => System.Text.Encoding.ASCII.GetString(this.backing.GetBytes());

    public Memory(Mutability mutability, DataSize min, DataSize? max, IRandomAccessMemory backing) {
        this.Mutability = mutability;
        this.MinSize = min;
        this.MaxSize = max;
        this.backing = backing;
    }

    public bool IsGrowable() => ReferenceEquals(null, MaxSize) ? true : CurrentSize.ByteCount < MaxSize.ByteCount;
    public bool CanRead() => ((byte)Mutability & 0b01) > 0;
    public bool CanWrite() => ((byte)Mutability & 0b10) > 0;

    // TODO access methods
    public byte Read8(int offset) {
        if (!this.CanRead()) {
            throw new ArgumentException("Memory is flagged as not readable.");
        }
        return backing.ReadByte(offset);
    }
    public void Write8(int offset, byte value) {
        if (!this.CanWrite()) {
            throw new ArgumentException("Memory is flagged as not writeable.");
        }
        backing.WriteByte(offset, value);
    }
    public ushort Read16(int offset) {
        if (!this.CanRead()) {
            throw new ArgumentException("Memory is flagged as not readable.");
        }
        byte low = backing.ReadByte(offset);
        byte high = backing.ReadByte(offset + 1);
        return (ushort)(low | (high << 8));
    }
    public void Write16(int offset, ushort value) {
        if (!this.CanWrite()) {
            throw new ArgumentException("Memory is flagged as not writeable.");
        }
        var low = value &  0x00FFu;
        var high = value & 0xFF00u;
        this.Write8(offset, (byte)low);
        this.Write8(offset + 1, (byte)high);
    }
    public uint Read32(int offset) {
        if (!this.CanRead()) {
            throw new ArgumentException("Memory is flagged as not readable.");
        }
        var low = (uint)backing.ReadByte(offset);
        var midlow = (uint)backing.ReadByte(offset + 1);
        var midhigh = (uint)backing.ReadByte(offset + 2);
        var high = (uint)backing.ReadByte(offset + 3);
        return (low | (midlow << 8) | (midhigh << 16) | (high << 24));
    }
    public void Write32(int offset, uint value) {
        if (!this.CanWrite()) {
            throw new ArgumentException("Memory is flagged as not writeable.");
        }
        var low = value    & 0x000000FFu;
        var midlow = value & 0x0000FF00u;
        var midhigh = value& 0x00FF0000u;
        var high = value   & 0xFF000000u;
        this.Write8(offset, (byte)low);
        this.Write8(offset + 1, (byte)midlow);
        this.Write8(offset + 2, (byte)midhigh);
        this.Write8(offset + 3, (byte)high);
    }

    public static readonly DataSize BlockHeaderSize = DataSize.Bytes(5) /*1 for tag, 4 for size*/;
    public void InitBlock() {
        // Write block header
        backing.WriteByte(0, 0x0);
        
        var size = (uint)CurrentSize.ByteCount - 5;
        var low = size    & 0x000000FFu;
        var midlow = size & 0x0000FF00u;
        var midhigh = size& 0x00FF0000u;
        var high = size   & 0xFF000000u;
        backing.WriteByte(1, (byte)low);
        backing.WriteByte(1 + 1, (byte)midlow);
        backing.WriteByte(1 + 2, (byte)midhigh);
        backing.WriteByte(1 + 3, (byte)high);
    }
    public AllocatedMemoryBlock GetBlockInfo(int offset) {
        // Read block header
        var tag = Read8(offset);
        var size = Read32(offset + 1);

        // Return structured data
        return new AllocatedMemoryBlock(
            free: tag == 0x0,
            startAt: offset,
            dataStartAt: offset + BlockHeaderSize.ByteCount,
            size: (int)size
        );
    }

    public int Reserve(int byteCount) {
        for (int i = 0; i < CurrentSize.ByteCount; i++) {
            var tag = Read8(i);
            var size = (int)Read32(i + 1);

            // Load tag
            if (tag == 0x0) {
                // Free space
                if (size > (byteCount + BlockHeaderSize.ByteCount)) {
                    // Consume this space
                    Write8(i, 0x1);
                    Write32(i + 1, (uint)byteCount);

                    // Create the empty block at the end of this space
                    i += BlockHeaderSize.ByteCount + byteCount;
                    Write8(i, 0x0);
                    Write32(i + 1, (uint)(size - byteCount - BlockHeaderSize.ByteCount));

                    return i;
                } 
                else if (size >= byteCount) {
                    // Consume this space
                    Write8(i, 0x1);
                    Write32(i + 1, (uint)size);

                    return i;
                }
                else {
                    // Skip this space
                    i = i + BlockHeaderSize.ByteCount + size - 1;
                }
            } else  {
                // Consumed space
                i = i + BlockHeaderSize.ByteCount + size - 1;
            }
        }

        throw new OutOfMemoryException();
    }

    public void Free(AllocatedMemoryBlock block) {
        Free(block.BlockAddress);
    }

    public void Free(int address) {
        AllocatedMemoryBlock? prev = null;
        AllocatedMemoryBlock? current = null;
        AllocatedMemoryBlock? next = null;
        foreach (var block in new HeapBlockIterator(this)) {
            if (block.BlockAddress == address) {
                current = block;
            } else if (current == null) {
                prev = block;
            } else if (current != null) {
                next = block;
                break;
            }
        }
        if (current == null)
            return;

        // Compute the size of the new free'd area
        AllocatedMemoryBlock startAt = current;
        DataSize freeSpace = current.Size;
        if (prev != null && prev.IsFree) {
            startAt = prev;
            freeSpace += prev.Size + BlockHeaderSize; // Eat the header of the current block;
        }
        if (next != null && next.IsFree) {
            freeSpace += next.Size + BlockHeaderSize; // Eat the next block and it's header
        }

        // Clear the tag (free memory)
        Write8(startAt.BlockAddress, (byte)0x0);
        Write32(startAt.BlockAddress + 1, (uint)freeSpace.ByteCount); 
    }
}