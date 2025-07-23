# Hack Assembler

A machine language assembler for the 16-bit [Hack assembly language](https://en.wikipedia.org/wiki/Hack_computer#Assembly_language),
implemented as part of the [Nand2Tetris course](https://www.nand2tetris.org/). The assembler takes a .asm file and outputs a .hack file which 
can be loaded into the [Hack computer](https://nand2tetris.github.io/web-ide/cpu).

Successful comparison of Pong.hack to the official Hack Assembler's output:

[!Successful comparison of Pong.hack](screenshots/successful-comparison-pong.png)

## Instructions

1. Place your .asm file into the same directory as the hack assembler.
2. Run the hack assembler and pass Filename.asm as a command-line argument (e.g.: Pong.asm).
3. The assembler will write the resulting .hack file to the same directory as the assembler, in the format: Filename.hack
(e.g.: Pong.hack).
4. The resulting file can then be loaded into the [Hack computer](https://nand2tetris.github.io/web-ide/cpu) or tested
using the steps below.

## Testing

Load the target .hack file into the Hack Computer's [online assembler](https://nand2tetris.github.io/web-ide/asm)
and compare the results to the built-in example programs (see screenshot below for an example). 

For convenience, three test asm files (Max.asm, Rect.asm, and Pong.asm) are copied from the 
[HackAssemblyFiles folder](/HackAssemblyFiles) to the build directory when the project is compiled.