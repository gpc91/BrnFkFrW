# BrnFkFrW
.Net Framework Brainfuck Interpreter.

Allows for easy exansibility by providing interfaces that can be used to define custom tokens for use by the interpreter.

Standard commands supported with the addition of command modifiers.

Utilizes Serilog for logging events and errors.

## Modifiers
For debugging purposes, included are a number of basic modifiers.

Read (`,`) modifiers:
- Explicit Read (`*`):
    - The input will attempt to pass any string entered as a byte instead of trying to read a single char. For example, if you type `127` the value will be converted to a byte as 127 and added to the cell currently being pointed to.

Print (`.`) Modifiers:
- Integer (`!`):
    - Prints an integer representation of the memory cell currently being pointed to.
- Memory (`#`):
    - Prints out a hex representation of the memory cell currently being pointed to.
- Mem At (`@`):
    - Prints out the currently pointed to memory cell and the 6 surrounding cells.
    - `[memory index] char (int) #hex`

Some elements are a bit wonky and still being worked on.
