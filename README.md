# BrnFkFrW
.Net Framework Brainfuck Interpreter.

## What is 'Brainfuck'?
Brainfuck is an esoteric programming language that uses only 8 instructions `+-<>[],.` to manipulate program memory. It is [Turing Complete](https://en.wikipedia.org/wiki/Turing_completeness).

The interpreter emulates a system that contains 30,000 byte-sized memory addresses. You use the `<` and `>` instructions to traverse the memory. To modify the memory you use `+` and `-`. To view, or write to memory through the CLI, you use `,` and `.`. You can utilise conditional loops using `[` and `]`.

For a more in-depth explanation of Brainfuck check out the [wikipedia page](https://en.wikipedia.org/wiki/Brainfuck). Included in the [Debug folder](../../tree/master/BrnFkFramework/bin/Debug) are several demo brainfuck programs.

## About this project

Allows for easy expansibility by providing interfaces that can be used to define custom tokens for use by the interpreter.

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
