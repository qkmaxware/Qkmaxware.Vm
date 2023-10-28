namespace Qkmaxware.Vm.Instructions;

public interface IMemoryAccessInstruction {}

/*
Load and Store
    load8_u             
    load8_s             
    load16_u
    load16_s
    load_32             
    store8
    store16
    store32

Meta
    sizeof

Lifecycle
    alloc
    free (0x5A)

Unused
    0x5B
    0x5C
    0x5D
    0x5E
    0x5F
*/

/// <summary>
/// Load an 8bit value onto the stack and zero-extend the value
/// </summary>
public class Load8U : Instruction, IMemoryAccessInstruction {

    public Load8U() {
        // Set opcode
        this.Opcode = 0x50; 
        
        // Arguments
        this.AddArgument(new Int32Argument("Memory Index"));

        // Stack
        this.AddStackOperand("address_base");
        this.AddStackOperand("offset");
        this.AddStackReturn("value");
    }

    public override string Description => "Load an 8bit value onto the stack and zero-extend the value.";

    public override void Action(VmValue[] args, RuntimeEnvironment runtime) {
        var memIdx = ((Operand)args[0]).Int32;
        var offset = runtime.Stack.PopTop().Int32;
        var addrBase = runtime.Stack.PopTop().Int32 + Memory.BlockHeaderSize.ByteCount;

        var memory = runtime.Memories[memIdx];
        var value = memory.Read8(addrBase + offset);
        var extend = 0b00000000_00000000_00000000_11111111u; // zero extend
        runtime.Stack.PushTop(Operand.From(extend & (uint)value));
    }
}

/// <summary>
/// Load an 8bit value onto the stack and sign-extend the value
/// </summary>
public class Load8S : Instruction, IMemoryAccessInstruction {

    public Load8S() {
        // Set opcode
        this.Opcode = 0x51; 
        
        // Arguments
        this.AddArgument(new Int32Argument("Memory Index"));

        // Stack
        this.AddStackOperand("address_base");
        this.AddStackOperand("offset");
        this.AddStackReturn("value");
    }

    public override string Description => "Load an 8bit value onto the stack and sign-extend the value.";

    public override void Action(VmValue[] args, RuntimeEnvironment runtime) {
        var memIdx = ((Operand)args[0]).Int32;
        var offset = runtime.Stack.PopTop().Int32;
        var addrBase = runtime.Stack.PopTop().Int32 + Memory.BlockHeaderSize.ByteCount;

        var memory = runtime.Memories[memIdx];
        var value = (UInt32)memory.Read8(addrBase + offset);
        if ((value & 0b00000000_00000000_00000000_10000000u) > 0) {
            // If the sign bit is 1
            value |= 0b11111111_11111111_11111111_00000000u;
        } else {
            // If the sign bit is 0
            value &= 0b00000000_00000000_00000000_11111111u;
        }
        runtime.Stack.PushTop(Operand.From(value));
    }
}

/// <summary>
/// Load an 16bit value onto the stack and zero-extend the value
/// </summary>
public class Load16U : Instruction, IMemoryAccessInstruction {

    public Load16U() {
        // Set opcode
        this.Opcode = 0x52; 
        
        // Arguments
        this.AddArgument(new Int32Argument("Memory Index"));

        // Stack
        this.AddStackOperand("address_base");
        this.AddStackOperand("offset");
        this.AddStackReturn("value");
    }

    public override string Description => "Load an 16bit value onto the stack and zero-extend the value.";

    public override void Action(VmValue[] args, RuntimeEnvironment runtime) {
        var memIdx = ((Operand)args[0]).Int32;
        var offset = runtime.Stack.PopTop().Int32;
        var addrBase = runtime.Stack.PopTop().Int32 + Memory.BlockHeaderSize.ByteCount;

        var memory = runtime.Memories[memIdx];
        var value = memory.Read16(addrBase + offset);
        var extend = 0b00000000_00000000_11111111_11111111u; // zero extend
        runtime.Stack.PushTop(Operand.From(extend & (uint)value));
    }
}

/// <summary>
/// Load an 16bit value onto the stack and sign-extend the value
/// </summary>
public class Load16S : Instruction, IMemoryAccessInstruction {

    public Load16S() {
        // Set opcode
        this.Opcode = 0x53; 
        
        // Arguments
        this.AddArgument(new Int32Argument("Memory Index"));

        // Stack
        this.AddStackOperand("address_base");
        this.AddStackOperand("offset");
        this.AddStackReturn("value");
    }

    public override string Description => "Load an 16bit value onto the stack and sign-extend the value.";

    public override void Action(VmValue[] args, RuntimeEnvironment runtime) {
        var memIdx = ((Operand)args[0]).Int32;
        var offset = runtime.Stack.PopTop().Int32;
        var addrBase = runtime.Stack.PopTop().Int32 + Memory.BlockHeaderSize.ByteCount;

        var memory = runtime.Memories[memIdx];
        var value = (UInt32)memory.Read8(addrBase + offset);
        if ((value & 0b00000000_00000000_10000000_00000000u) > 0) {
            // If the sign bit is 1
            value |= 0b11111111_11111111_00000000_00000000u;
        } else {
            // If the sign bit is 0
            value &= 0b00000000_00000000_11111111_11111111u;
        }
        runtime.Stack.PushTop(Operand.From(value));
    }
}

/// <summary>
/// Load an 32bit value onto the stack 
/// </summary>
public class Load32 : Instruction, IMemoryAccessInstruction {

    public Load32() {
        // Set opcode
        this.Opcode = 0x54; 
        
        // Arguments
        this.AddArgument(new Int32Argument("Memory Index"));

        // Stack
        this.AddStackOperand("address_base");
        this.AddStackOperand("offset");
        this.AddStackReturn("value");
    }

    public override string Description => "Load an 16bit value onto the stack.";

    public override void Action(VmValue[] args, RuntimeEnvironment runtime) {
        var memIdx = ((Operand)args[0]).Int32;
        var offset = runtime.Stack.PopTop().Int32;
        var addrBase = runtime.Stack.PopTop().Int32 + Memory.BlockHeaderSize.ByteCount;

        var memory = runtime.Memories[memIdx];
        var value = memory.Read32(addrBase + offset);
        runtime.Stack.PushTop(Operand.From(value));
    }
}

/// <summary>
/// Store an 8bit value on top of the stack to the memory address
/// </summary>
public class Store8 : Instruction, IMemoryAccessInstruction {

    public Store8() {
        // Set opcode
        this.Opcode = 0x55; 
        
        // Arguments
        this.AddArgument(new Int32Argument("Memory Index"));

        // Stack
        this.AddStackOperand("address_base");
        this.AddStackOperand("offset");
        this.AddStackOperand("value");
    }

    public override string Description => "Store an 8bit value on top of the stack to the memory address.";

    public override void Action(VmValue[] args, RuntimeEnvironment runtime) {
        var memIdx = ((Operand)args[0]).Int32;
        var value = runtime.Stack.PopTop().UInt32;
        var offset = runtime.Stack.PopTop().Int32;
        var addrBase = runtime.Stack.PopTop().Int32 + Memory.BlockHeaderSize.ByteCount;

        var memory = runtime.Memories[memIdx];
        memory.Write8(addrBase + offset, (byte)value);
    }
}

/// <summary>
/// Store an 16bit value on top of the stack to the memory address
/// </summary>
public class Store16 : Instruction, IMemoryAccessInstruction {

    public Store16() {
        // Set opcode
        this.Opcode = 0x56; 
        
        // Arguments
        this.AddArgument(new Int32Argument("Memory Index"));

        // Stack
        this.AddStackOperand("address_base");
        this.AddStackOperand("offset");
        this.AddStackOperand("value");
    }

    public override string Description => "Store an 16bit value on top of the stack to the memory address.";

    public override void Action(VmValue[] args, RuntimeEnvironment runtime) {
        var memIdx = ((Operand)args[0]).Int32;
        var value = runtime.Stack.PopTop().UInt32;
        var offset = runtime.Stack.PopTop().Int32;
        var addrBase = runtime.Stack.PopTop().Int32 + Memory.BlockHeaderSize.ByteCount;

        var memory = runtime.Memories[memIdx];
        memory.Write16(addrBase + offset, (ushort)value);
    }
}

/// <summary>
/// Store an 32bit value on top of the stack to the memory address
/// </summary>
public class Store32 : Instruction, IMemoryAccessInstruction {

    public Store32() {
        // Set opcode
        this.Opcode = 0x57; 
        
        // Arguments
        this.AddArgument(new Int32Argument("Memory Index"));

        // Stack
        this.AddStackOperand("address_base");
        this.AddStackOperand("offset");
        this.AddStackOperand("value");
    }

    public override string Description => "Store an 32bit value on top of the stack to the memory address.";

    public override void Action(VmValue[] args, RuntimeEnvironment runtime) {
        var memIdx = ((Operand)args[0]).Int32;
        var value = runtime.Stack.PopTop().UInt32;
        var offset = runtime.Stack.PopTop().Int32;
        var addrBase = runtime.Stack.PopTop().Int32 + Memory.BlockHeaderSize.ByteCount;

        var memory = runtime.Memories[memIdx];
        memory.Write32(addrBase + offset, value);
    }
}

/// <summary>
/// Get the size of a compound data instance from it's pointer
/// </summary>
public class Sizeof : Instruction, IMemoryAccessInstruction {

    public Sizeof() {
        // Set opcode
        this.Opcode = 0x58; 
        
        // Arguments
        this.AddArgument(new Int32Argument("Memory Index"));

        // Stack
        this.AddStackOperand("address_base");
        this.AddStackReturn("size");
    }

    public override string Description => "Using a pointer at the top of the operand stack, determine the size of the object in bytes and place that value on top of the operand stack.";

    public override void Action(VmValue[] args, RuntimeEnvironment runtime) {
        var memIdx = ((Operand)args[0]).Int32;
        var ptr = runtime.Stack.PopTop().Int32;
        
        var memory = runtime.Memories[memIdx];
        var block = memory.GetBlockInfo(ptr);
        runtime.Stack.PushTop(Operand.From(block.Size.ByteCount));
    }
}


/// <summary>
/// Allocate a contiguous block of memory in the heap
/// </summary>
public class Alloc : Instruction, IMemoryAccessInstruction {

    public Alloc() {
        // Set opcode
        this.Opcode = 0x59; 
        
        // Arguments
        this.AddArgument(new Int32Argument("Memory Index"));
        
        // Stack
        this.AddStackOperand("size");
        this.AddStackReturn("address");
    }

    public override string Description => "Allocate a contiguous block of memory in the heap.";

    public override void Action(VmValue[] args, RuntimeEnvironment runtime) {
        var memIdx = ((Operand)args[0]).Int32;
        var size = runtime.Stack.PopTop().Int32;
        
        var address = runtime.Memories[memIdx].Reserve(size);
        runtime.Stack.PushTop(Operand.From(address));
    }
}

/// <summary>
/// Free a reserved block of memory in the heap
/// </summary>
public class Free : Instruction, IMemoryAccessInstruction {

    public Free() {
        // Set opcode
        this.Opcode = 0x5A; 
        
        // Arguments
        this.AddArgument(new Int32Argument("Memory Index"));

        // Stack
        this.AddStackOperand("address_base");
    }

    public override string Description => "Free a reserved block of memory in the heap.";

    public override void Action(VmValue[] args, RuntimeEnvironment runtime) {
        var memIdx = ((Operand)args[0]).Int32;
        var address = runtime.Stack.PopTop().Int32;

        runtime.Memories[memIdx].Free(address);
    }
}