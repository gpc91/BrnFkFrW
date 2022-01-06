using System;
using System.Collections.Generic;
using BrnFkFramework.Brainfuck.Instructions;

namespace BrnFkFramework.Brainfuck
{
    public class BrainfuckInterpreter : Interpreter
    {
        public BrainfuckInterpreter(int allocatedMemory = 30000) : base(allocatedMemory)
        {
            Instructions = new Dictionary<char, IInstruction>()
            {
                {'.', new BrainfuckPrint()}, // print
                {',', new BrainfuckRead()}, // read
                {'+', new BrainfuckAdd()}, // add
                {'-', new BrainfuckSub()}, // sub
                {'>', new BrainfuckRight()}, // shr
                {'<', new BrainfuckLeft()}, // shl
                {'[', new BrainfuckCondLeft()}, // lpl
                {']', new BrainfuckCondRight()} // lpr
            };
        }
        
    }
}