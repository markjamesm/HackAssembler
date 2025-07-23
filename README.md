# Hack Assembler

A machine language assembler for the 16-bit Hack assembly language, implemented as part of the 
[Nand2Tetris](https://www.nand2tetris.org/) course. The assembler takes a .asm file and outputs a .hack file which can be loaded into the Hack
computer.

## Instructions

1. Place your .asm file into the same directory as the hack assembler. For convenience, three test asm files are included
in the build directory (Max.asm, Rect.asm, and Pong.asm)
2. Run the hack assembler and pass <FILENAME>.asm as a command-line argument (e.g.: Pong.asm).
3. The assembler will write the resulting .hack file to the same directory as the assembler, in the format <FILENAME.hack>
(e.g.: Pong.hack)
4. Load the resulting .hack file into the Hack Computer's online [Hack Assembler](https://nand2tetris.github.io/web-ide/asm) 
and compare the results.

## Screenshot

Successful comparison of Pong.hack to the official Hack Assembler's output:

<img width="2559" height="1279" alt="Image" src="https://github.com/user-attachments/assets/c77e8259-a19c-4e1e-8b39-49617522ea07" />