namespace Qkmaxware.Vm;

/// <summary>
/// Builder to simplify the creation of bytecode modules programmatically
/// </summary>
public partial class ModuleBuilder {

    /// <summary>
    /// Load a 32bit constant from the given address
    /// </summary>
    [Macro("load_const32", description: "Load a 32bit constant from the given address.")]
    public void LoadConst32(MemoryRef constantRef) {
        this.PushInt32(constantRef.Offset);     // Address base
        this.PushInt32(0);                      // Offset
        this.Load32(constantRef.MemoryIndex);   // Load
    }

    /// <summary>
    /// Insert all instructions required to print a string from the top of the stack
    /// </summary>
    [Macro("printstr", description: "Insert all instructions required to print a string from the top of the stack.")]
    public void PrintString(int fromMemory) {
        // High level version of what we are attempting to do in bytecode assembly
        /*
            var string = "";
            var len = length(string);
            var index = 0;
            for (; index < len; index++) {
                printchar(string[index]);
            }
        */

        // Assume a string pointer is at the top of the stack
        // Stack [string_ptr]
        this.DuplicateStackTop();
        // Stack [string_ptr, string_ptr]
        this.ObjectSize(fromMemory); 
        // Stack [string_ptr, string_length]
        this.PushInt32(0);
        // Stack [string_ptr, string_length, index]
        var print_loop = this.Anchor();
        this.DuplicateStackElement(2); 
        // Stack [string_ptr, string_length, index, string_ptr]
        this.DuplicateStackElement(1);
        // Stack [string_ptr, string_length, index, string_ptr, index]
        this.Load8(fromMemory, Extend.Zero);
        // Stack [string_ptr, string_length, index, character]
        this.AddInstruction("putchar");
        // Stack [string_ptr, string_length, index]
        this.PushInt32(1);
        // Stack [string_ptr, string_length, index, 1]
        this.AddInstruction("add_i32");
        // Stack [string_ptr, string_length, index + 1]
         // Do the condition
        // (index) - (string_length) {
        //      < 0 IF index <  string_length
        //      > 0 IF index >  string_length
        //      = 0 IF index == string_length
        // }   
        this.DuplicateStackTop();
        // Stack [string_ptr, string_length, index + 1, index + 1]
        this.DuplicateStackElement(2);             
        // Stack [string_ptr, string_length, index + 1, index + 1, string_length]
        this.AddInstruction("sub_i32");                       
        // Stack [string_ptr, string_length, index + 1, condition]
        this.AddInstruction(
            "goto_if_nzero", 
            Operand.From((int)(print_loop - (this.Anchor() + 5)))
        );                                          
        // Stack [string_ptr, string_length, index + 1]

        // Cleanup the stack
        this.AddInstruction("pop");                           
        // Stack [string_ptr, string_length]
        this.AddInstruction("pop");                           
        // Stack [string_ptr]
        this.AddInstruction("pop");                           
        // Stack []
    }

    /// <summary>
    /// Create an ascii string constant and immediately load it to the stack
    /// </summary>
    /// <param name="text">constant text</param>
    [Macro("immediate_ascii", description: "Create a constant pool reference for an ASCII encoded string and load a pointer to that constant onto the stack.")]
    public void PushAscii(string text) {
        var constant = this.AddConstantAsciiString(text);
        this.PushAddressOf(constant);
    }
    /*
    /// <summary>
    /// Create a UTF8 string constant and immediately load it to the stack
    /// </summary>
    /// <param name="text">constant text</param>
    [Macro("immediate_utf8", description: "Create a constant pool reference for an UTF8 encoded string and load a pointer to that constant onto the stack.")]
    public void PushUtf8(string text) {
        var constant = this.AddConstantUtf8String(text);
        this.PushAddressOf(constant);
    }

    /// <summary>
    /// Create a UTF32 string constant and immediately load it to the stack
    /// </summary>
    /// <param name="text">constant text</param>
    [Macro("immediate_utf32", description: "Create a constant pool reference for an UTF32 encoded string and load a pointer to that constant onto the stack.")]
    public void PushUtf32(string text) {
        var constant = this.AddConstantUtf32String(text);
        this.PushAddressOf(constant);
    }
    */
}