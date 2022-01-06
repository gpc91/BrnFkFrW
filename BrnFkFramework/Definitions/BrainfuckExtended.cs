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
                {'+', new ExtendedAdd()}, // add
                {'-', new ExtendedSub()}, // sub
                {'>', new BrainfuckRight()}, // shr
                {'<', new BrainfuckLeft()}, // shl
                {'[', new BrainfuckCondLeft()}, // lpl
                {']', new BrainfuckCondRight()} // lpr
            };
        }
    }

    public class ExtendedAdd : IInstruction
    {
        public void Execute(Parser parser)
        {
            switch (parser.Interpreter.InputString[(int) parser.Interpreter.SourceParser.Pointer])
            {
                case '*':
                    parser.Interpreter.logger?.Verbose("Cell Multiply");
                    parser.Interpreter.WorkingMemory.Cell *= 2;
                    return;
                default:
                    parser.Interpreter.logger?.Verbose("Cell Add");
                    parser.Interpreter.WorkingMemory.Add();
                    return;
            }
            parser.Interpreter.logger?.Verbose("Cell Add");
        }
    }

    public class ExtendedSub : IInstruction
    {
        public void Execute(Parser parser)
        {
            switch (parser.Interpreter.InputString[(int) parser.Interpreter.SourceParser.Pointer])
            {
                case '*':
                    parser.Interpreter.logger?.Verbose("Cell Div");
                    parser.Interpreter.WorkingMemory.Cell /= 2;
                    return;
                default:
                    parser.Interpreter.logger?.Verbose("Cell Sub");
                    parser.Interpreter.WorkingMemory.Sub();
                    return;
            }
        }
    }
    
}