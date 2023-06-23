# Assembly
This virtual machine supports its own assembly code which can be used to write programs using a human readable textual representation. For simplicity, this assembly is simply called Qkmaxware Assembly and are usually saved in .qkasm files.

## Dialects
All valid assembly files will begin with a preamble such as can be seen below.
```
use asm 1.0
```
This preamble indicates three pieces of information to the assembler. The first is the name of the assembly dialect. Assemblers can choose to support more dialects than the default one provided in this reference. In the above example the dialect name is simply "asm" which is the name of the dialect described in the remainder of this document.

The preamble also indicates what version of the dialect it was written in by using a {major version}.{minor version} syntax. This version number should be checked by assemblers to make sure that it is able to process the remainder of the assembly file. 

## Asm
The default dialect described in this document is simply known as "asm" and it is currently on version 1.0. 

Each line of the file is treated as its own command. Most commands are directly mapped to instructions within the virtual machine. Each type of command is detailed below. 

### Comments
Comments are standard C-style line comments that can be placed on their own line or at the end of other lines.
```
// This is a comment
```

### Imports
Imports are indicators that a subprogram is provided by another module. These imports can be used in "call_external" instructions. The syntax for an import command is the world **import** followed by a doubly quoted JSON formatted string representing the name of the imported subprogram. When linking, imports of one module are matched by name to the exports of another module.
```
import "ExternalModule.SubprogramName"
```

### Exports
Exports are indicators that a given subprogram in the module can be used by another module. Exports act as named labels to parts of your code and should be placed at the start of the subprogram code. The syntax for an export command is the world **export** followed by a doubly quoted JSON formatted string representing the name of the exported subprogram.
```
export "MySubprogram"
```

### Labels
Labels are named markers to particular spots in your code. Labels are mainly used when performing jumps or calls within your code so that you don't need to worry about updating you jump offsets whenever you add or remove code from the file as these offsets will be computed when the file is assembled. The syntax for a label is simply a '.' followed by at least one of the following characters [a-zA-Z0-9_]. Labels can be paired with exports to allow for subprograms to be called both within the file and by files that link to this one. 
```
.label
```

### Constants
Constant commands create data within the constant pool. The syntax for constant commands is the '@' followed by at least one of the following characters [a-zA-Z0-9_] for the constant name. Following the name, there is an equals sign and then a value. The value itself determines the constant type. These constants can then be used by load_const instructions to load the values from the constant pool. 
```
@string = "Fizz Buzz"
@int = 4
@float = 4.2 // or 4f
@uint = 4u
```

### Macros
Macros are shorthands which are expanded to one or more other commands. The syntax for macro commands is the '!' followed by at least one of the following characters [a-zA-Z0-9_] for the macro name. Following the name one can provide 0 or more space separated arguments for the macro. Numbers, and string are allowed are arguments.

```
!immediate_ascii "Hello World"
```

You can see a list of all currently supported macros by reading the [[Macro Listing]] document.

### Instructions
Every other line is treated as an instruction which is mapped directly to instructions supported by the bytecode virtual machine. For a full list of instructions, read the [[Bytecode Instruction Set]] document. The syntax for instructions is an instruction name composed of at least one of the following characters [a-zA-Z0-9_] followed by 0 or more space separated arguments. Numbers, constants (prefixed with @), labels (prefixed with .) and in some cases strings (referencing imports) are able to be used as arguments.

```
load_const @hello
call_external "System.Console.PrintString" 1
exit 0
```

## Example
The following is a full example that shows off many of the different elements of the asm dialect. The program below is designed to be linked to another module that provides a method to print strings. The result after linking and running should be the Hello World application. 
```
use asm 1.0

import "System.Console.PrintString"

@hello = "Hello World"

export "Main"
.main
    load_const @hello
    call_external ""System.Console.PrintString"" 1
    exit 0
```