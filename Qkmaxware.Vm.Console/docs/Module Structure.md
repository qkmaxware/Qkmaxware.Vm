# Bytecode Module Structure
Bytecode modules are broken up into multiple sections. Which are listed in order of appearance. Modules should be written and read using little endian byte order. 

## Header
The header is used to indicate that the file is a valid bytecode module and to indicate what version of the bytecode specification if conforms to. Module loaders should read this header to determine if the file is able to be successfully read.

| Bytes        | Description                              |
|--------------|------------------------------------------|
| 4 bytes      | The letters "qkbc"                       |
| 4 bytes      | Major version number as a signed integer |
| 4 bytes      | Minor version number as a signed integer |

## Export List
The export list defines a list of labels which are provided by the bytecode module. Using static or dynamic linking, these labels can be used to link together multiple bytecode modules so that they may provide each other with specific functionality. Each label is simply a pointer to somewhere in the code. 

The header of the export list contains only the number of exported labels followed by that number of labels.

| Bytes        | Description                              |
|--------------|------------------------------------------|
| 4 bytes      | Number of exported labels                |
| *            | Labels                                   |

Each label is defined by a number of bytes for the name of the label followed by the UTF8 bytes of the label name and then a 32bit number for the offset within the code section that that label points to.

| Bytes        | Description                              |
|--------------|------------------------------------------|
| 4 bytes      | Number of bytes in the label name        |
| 4 bytes*     | Single UTF8 codepoint                    |
| 4 bytes      | Offset into the code section             |

* The number of bytes used varies from character to character and are repeatedly read until the number of bytes in the label name are read

## Import List
The import list defines a list of names for labels that are required to be linked to this module in order for it to run successfully. During static or dynamic linking imports of one module should be connected with exports of other modules by matching their names.  

The header of the import list contains only the number of imports followed by the imported names.

| Bytes        | Description                              |
|--------------|------------------------------------------|
| 4 bytes      | Number of exported names                 |
| *            | Names                                    |

Each name is defined by a number of bytes followed by the UTF8 bytes of the name.

| Bytes        | Description                              |
|--------------|------------------------------------------|
| 4 bytes      | Number of bytes in the name              |
| 4 bytes*     | Single UTF8 codepoint                    |

* The number of bytes used varies from character to character and are repeatedly read until the number of bytes in the label name are read

## Code 
The code section defines the actual bytecode that is run by the virtual machine. 

The header of the code section simply provides a single number that indicates how many bytes are in the code section.

| Bytes        | Description                              |
|--------------|------------------------------------------|
| 4 bytes      | Number of bytes in the code section      |

Following that, each instruction is encoded using a single byte for the opcode followed by varying numbers of bytes depending on the instruction's required arguments. 

| Bytes        | Description                              |
|--------------|------------------------------------------|
| 1 byte       | Instruction opcode                       |
| *            | Instruction arguments                    |

For more information of each instruction and it's arguments read about the [[Bytecode Instruction Set]].

## Data Segment
The data segment of the module is used for storing data which can be accessed at runtime. The data segment is broken up into 2 different pools. The size of the data-section remains fixed during the execution of a program. The **Constant Pool** is used for storing constant data that will not change during the length of the program's execution. On the other hand, the **Static Pool** stored data that can be both read and updated from the pool at runtime. 

### Constant Pool
The constant pool is a region in the bytecode file for storing values that are to remain unchanged throughout the runtime of a program.

The constant pool is strongly typed and each element can be accessed by a single index into the pool. This makes the Constant Pool more flexible than the Static Pool for storing values as values that take up larger amounts of memory will still only occupy a single slot in the constant pool thanks to this type system allowing for each value to be read one at a time. 

The header of the constant pool, like other sections, is composed only of a single integer representing the number of entries in the constant pool. 

| Bytes        | Description                              |
|--------------|------------------------------------------|
| 4 bytes      | Number of entries in the constant pool   |

Each subsequent entry is then encoded in the following format. 

| Bytes        | Description                              |
|--------------|------------------------------------------|
| 1 byte       | Tag representing the type of data        |
| *            | Data bytes differing by data type        |

Every type will encode it's data differently. Some of the included constant types are described in the table below.

| Type    | Tag Value | Encoding                          |
|---------|-----------|-----------------------------------|
| Int32   | 0x01      | 4 bytes little endian integer     |
| UInt32  | 0x02      | 4 bytes little endian integer     |
| Float32 | 0x03      | 4 bytes little endian single      |
| Array   | 0x09      | 1 byte element type, 4 for element count, elements encoded individually after |
| ASCII   | 0x10      | 4 bytes length then characters    |
| UTF8    | 0x11      | 4 bytes length then characters    |
| UTF32   | 0x12      | 4 bytes length then characters    |

### Static Pool
The static pool is a region in the bytecode file for storing values that can change over the lifetime of a program. While the values in the static pool can be updated as a program executes, the total size of the pool remains fixed and as such you can't add or remove elements from the pool, only change existing elements.

Additionally, the Static Pool only contains 32bit values to maintain full compatibility with the stack. These values are un-typed and as such can be treated as any primitive value by whatever instructions operate on them once loaded to the stack. If you wish to store an array in the Static Pool, you should store the array in the heap and then save a pointer to the array in the Static Pool. 

Changes made during runtime to the Static Pool will not effect the bytecode module that they were initially loaded from. 

Since only 32bit values are stored in the Static Pool it's encoding is very simple.

| Bytes        | Description                              |
|--------------|------------------------------------------|
| 4 bytes      | Number of entries in the constant pool   |
| 4 bytes*     | Linear array of 32bit values             |