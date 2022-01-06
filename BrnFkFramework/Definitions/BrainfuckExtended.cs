using System.Collections.Generic;
using BrnFkFramework.Brainfuck.Instructions;

namespace BrnFkFramework.Definitions
{
    public class BrainfuckExtended : Interpreter
    {
        public BrainfuckExtended(int allocatedMemory = 30000) : base(allocatedMemory)
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