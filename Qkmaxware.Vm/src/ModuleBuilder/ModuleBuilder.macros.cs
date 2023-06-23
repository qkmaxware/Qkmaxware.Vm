namespace Qkmaxware.Vm;

/// <summary>
/// Builder to simplify the creation of bytecode modules programmatically
/// </summary>
public partial class ModuleBuilder {

    /// <summary>
    /// Insert all instructions required to print a string from the top of the stack
    /// </summary>
    [Macro("printstr", description: "Insert all instructions required to print a string from the top of the stack.")]
    public void PrintString() {
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
        // Setup the initial variables
        this.AddInstruction("dup");                           // [string_ptr, string_ptr]
        this.AddInstruction("len");                           // [string_ptr, string_length]
        this.PushInt32(0);                          // [string_ptr, string_length, index]

        // Print the next character in the string and increment the character index 
        var print_loop = this.Anchor();     
        this.DuplicateStackElement(2);              // [string_ptr, string_length, index, string_ptr]
        this.DuplicateStackElement(1);              // [string_ptr, string_length, index, string_ptr, index]
        this.AddInstruction("get_element");                   // [string_ptr, string_length, index, character]
        this.AddInstruction("putchar");                       // [string_ptr, string_length, index]
        this.PushInt32(1);                          // [string_ptr, string_length, index, 1]
        this.AddInstruction("add_i32");                       // [string_ptr, string_length, index + 1]
        
        // Do the condition
        // (index) - (string_length) {
        //      < 0 IF index <  string_length
        //      > 0 IF index >  string_length
        //      = 0 IF index == string_length
        // }    
        this.AddInstruction("dup");                           // [string_ptr, string_length, index + 1, index + 1]
        this.DuplicateStackElement(2);              // [string_ptr, string_length, index + 1, index + 1, string_length]
        this.AddInstruction("sub_i32");                       // [string_ptr, string_length, index + 1, condition]
        this.AddInstruction(
            "goto_if_nzero", 
            Operand.From((int)(print_loop - (this.Anchor() + 5)))
        );                                          // [string_ptr, string_length, index + 1]

        // Cleanup the stack
        this.AddInstruction("pop");                           // [string_ptr, string_length]
        this.AddInstruction("pop");                           // [string_ptr]
        this.AddInstruction("pop");                           // []
    }

    /// <summary>
    /// Create an ascii string constant and immediately load it to the stack
    /// </summary>
    /// <param name="text">constant text</param>
    [Macro("immediate_ascii", description: "Create a constant pool reference for an ASCII encoded string and load a pointer to that constant onto the stack.")]
    public void PushAscii(string text) {
        var constant = this.AddConstantAsciiString(text);
        this.PushConstant(constant);
    }

    /// <summary>
    /// Create a UTF8 string constant and immediately load it to the stack
    /// </summary>
    /// <param name="text">constant text</param>
    [Macro("immediate_utf8", description: "Create a constant pool reference for an UTF8 encoded string and load a pointer to that constant onto the stack.")]
    public void PushUtf8(string text) {
        var constant = this.AddConstantUtf8String(text);
        this.PushConstant(constant);
    }

    /// <summary>
    /// Create a UTF32 string constant and immediately load it to the stack
    /// </summary>
    /// <param name="text">constant text</param>
    [Macro("immediate_utf32", description: "Create a constant pool reference for an UTF32 encoded string and load a pointer to that constant onto the stack.")]
    public void PushUtf32(string text) {
        var constant = this.AddConstantUtf32String(text);
        this.PushConstant(constant);
    }

}