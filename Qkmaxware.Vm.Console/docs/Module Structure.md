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

## Data
The data segment of the module is used for storing data which can be accessed at runtime. The data segment is composed of different "memories". Memories can have different sizes and differing types of permissions. You may have any number of memories (up to Int32.Max) and each memory can have between 0 and Int32.Max bytes. 

The header of the data segment simply provides a single number that indicates how many memories are defined in the module.

| Bytes        | Description                              |
|--------------|------------------------------------------|
| 4 bytes      | Number of memories in the code section   |

Following the header, each memory is described in order. Each memory is composed of the following bytes.

| Bytes        | Description                                          |
|--------------|------------------------------------------------------|
| 1 bytes      | Access Permissions (None=00, R=01, W=10, RW=11)      |
| 4 bytes      | Min size in bytes                                    |
| 4 bytes      | Max size in bytes (-1 if no max size provided)       |
| 4 bytes      | Count of bytes to copy into memory when instantiated |
| * bytes      | Bytes to copy into memory when instantiated          |

The above is referred to as a memory spec and will be instantiated into a full memory by the virtual machine when the module is loaded. If element 4 (byte count) is 0 then the memory is initialized with a default empty block. If element 4 is not zero then the following bytes are copied into the newly initialized memory. This is referred to a data initializer. As such, the data initializer should be properly formatted for the VM to properly read the memory once initialized.

Memory should be properly formatted into blocks. There may be 1 or more blocks in a memory. Each block is defined by a single byte indicating if the block is free for use (0) or used (not zero). This is followed by the size of the block in memory as an Int32. After the last byte, the next block can start. For an "empty" memory there should be a single block spanning the entire size of the memory which is marked as free. The ModuleBuilder class can be used to more easily create data initializers with well-formatted blocks.  

### Constant Pool
A constant pool is a region in the bytecode file for storing values that are to remain unchanged throughout the runtime of a program. In the module syntax, a constant pool is simply a memory with read-only access. 

Any store instructions directed towards read-only memories will result in runtime exceptions within the VM.

### Static Pool
A static pool is a region in the bytecode file for storing globally accessible values that can change over the lifetime of a program. While the values in the static pool can be updated as a program executes, the total size of the pool remains fixed and as such you can't add or remove elements from the pool, only change existing elements. In the module syntax, a static pool is simply a memory with read-write access, but the max size is equal to the minium size. 

Changes made during runtime to the Static Pool will not effect the bytecode module that they were initially loaded from. 

### Heap
A heap is a dynamic region of memory in which values can be created or destroyed over the lifetime of the program. Typically this is used for structures or objects in object oriented languages. In the module syntax, a heap can be created with a memory that has read-write access, but also is capable of growing. This means that the max size of the memory is either -1 meaning no limit other than Int32.Max or the max size is greater than the minimum size. 