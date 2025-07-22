# Hack Assembler

A machine language assembler for the 16-bit Hack assembly language, implemented as part of the 
[Nand2Tetris](https://www.nand2tetris.org/) course. The assembler takes a .asm file and outputs a .hack file which can be loaded into the Hack
computer.

## Instructions

1. Place your .asm file into the same directory as the hack assembler.
2. Run the hack assembler and pass <FILENAME>.asm as a command-line argument (eg: PongL.asm).
3. The assembler will write the file to the same directory as the assembler, in the format <FILENAME.hack>
(eg: PongL.hack)
4. Load the resulting .hack file into the Hack Computer's online [CPU emulator](https://nand2tetris.github.io/web-ide/cpu)