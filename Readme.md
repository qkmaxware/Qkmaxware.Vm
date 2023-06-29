# Qkmaxware.VM
A simple stack based virtual machine capable of executing specially designed bytecode files + assembly language designed to be used as a compilation target for student compiler lessons. 

- [Qkmaxware.VM](#qkmaxwarevm)
  - [Packages](#packages)
    - [Qkmaxware.Vm](#qkmaxwarevm-1)
    - [Qkmaxware.Vm.Console](#qkmaxwarevmconsole)
  - [Virtual Machine](#virtual-machine)
    - [Bytecode File Format](#bytecode-file-format)
    - [Supported Instructions](#supported-instructions)
    - [Static Linking](#static-linking)
  - [Assembly](#assembly)
    - [Assembly Syntax](#assembly-syntax)
    - [Assembling](#assembling)
    - [Online Demo](#online-demo)

## Packages
This repository produces 2 individual packages. 

### Qkmaxware.Vm
The core package consisting of all the source code for the virtual machine and all related functionality. You can use package management to include this into your own projects such as for use as a compilation target in your own compiler.

### Qkmaxware.Vm.Console
A console application that can be installed as a dotnet tool and invoked using the `qkvm` command. It is capable of running code, assembling bytecode, inspecting bytecode and providing quick access to documentation. You should install this to run and code produced in a stand-alone manor and not as a part of another application. 

## Virtual Machine
The virtual machine is a stack based virtual machine. Each instructions is fetched from a bytecode file, decoded, mapped to an existing instruction, and then executed. All common virtual machine instructions are supported for 32 bit integers, 32 bit unsigned integers, and 32bit floating point numbers. 

The virtual machine has limited interaction with the host machine. Currently no access to the host's file-system is supported though the functionality could be added in future revisions. 

### Bytecode File Format
For more information on the bytecode file format read about the [module structure](Qkmaxware.Vm.Console/docs/Module%20Structure.md).

### Supported Instructions
All the supported hardware instructions can be found [here](Qkmaxware.Vm/src/Instructions/).

### Static Linking
Bytecode files (modules) can provide exports and imports which can allow code to be generated in smaller pieces and then statically linked together to create a single larger executable. This can be done using the [Linker](Qkmaxware.Vm/src/Linker.cs) class or via the command line tool with the `qkvm link` command. 

Once 2 modules have been linked, a new third module will be produced which is the combination of the original 2. To link more than 2 modules together you can chain calls to the link function. This is done automatically if using the command line tool. 

## Assembly
An assembly language is also provided to allow one to write code in a human readable format before assembling it into executable bytecode. 

### Assembly Syntax
For more information on the syntax of the assembly language you can read about it [here](Qkmaxware.Vm.Console/docs/Assembly.md).

### Assembling
Assembling of assembly files to bytecode files is able to be done using the [Assembler](Qkmaxware.Vm/src/Assembly/Assembler.cs) class or via the command line tool with the `qkvm assemble` command. 

### Online Demo
You can experiment with the assembler online by visiting this repository's related [github-pages](https://qkmaxware.github.io/Qkmaxware.Vm/) site.