namespace Qkmaxware.Vm;

public class InvalidOpcodeException : Exception {
    public InvalidOpcodeException(byte opcode) : base($"Invalid opcode '0x{opcode:X}'") {}  
}