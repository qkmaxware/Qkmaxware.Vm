namespace Qkmaxware.Vm;

/// <summary>
/// Stream the bytecode from a module
/// </summary>
public class BytecodeStream : Stream {

    private Module bytecodeModule;

    public BytecodeStream(Module module) {
        this.bytecodeModule = module;
    }

    public override bool CanRead => true;

    public override bool CanSeek => true;

    public override bool CanWrite => false;

    public override long Length => bytecodeModule.CodeLength;

    public override long Position { 
        get; set;
    }

    public override void Flush() {}

    public override int Read(byte[] buffer, int offset, int count) {
        int read = 0;
        for (var i = 0; i < count; i++) {
            if (Position < bytecodeModule.CodeLength) {
                buffer[offset + i] = bytecodeModule.Code[(int)Position++];
                read++;
            } else {
                break;
            }
        }
        return read;
    }

    public override long Seek(long offset, SeekOrigin origin) {
        return origin switch {
            SeekOrigin.Begin => Position = offset,
            SeekOrigin.Current => Position = Position + offset,
            SeekOrigin.End => Length + offset,
            _ => Position = offset
        };
    }

    public override void SetLength(long value) {}

    public override void Write(byte[] buffer, int offset, int count) {
        throw new NotImplementedException();
    }
}