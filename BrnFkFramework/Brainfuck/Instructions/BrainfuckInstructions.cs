using System;
using System.Runtime.Remoting.Messaging;

namespace BrnFkFramework.Brainfuck.Instructions
{
    public class BrainfuckAdd : IInstruction
    {
        public void Execute(Parser parser)
        {
            parser.Interpreter.WorkingMemory.Add();
        }
    }
    
    public class BrainfuckSub : IInstruction
    {
        public void Execute(Parser parser)
        {
            parser.Interpreter.WorkingMemory.Sub();
        }
    }
    public class BrainfuckPrint : IInstruction
    {
        public void Execute(Parser parser)
        {
            byte cell = parser.Interpreter.WorkingMemory.Cell;
            switch (parser.Interpreter.InputString[(int) parser.Interpreter.SourceParser.Pointer])
            {
                case '!' :
                    Console.WriteLine($"{cell}");
                    parser.Interpreter.SourceParser.Pointer++;
                    return;
                case '@':
                    Console.WriteLine($"{(char)cell} ({cell})");
                    parser.Interpreter.SourceParser.Pointer++;
                    return;
                case '£' :
                    Console.WriteLine($"{cell} ({(int)cell})");
                    parser.Interpreter.SourceParser.Pointer++;
                    return;
                case '#' :
                    parser.Interpreter.PrintMemory();
                    parser.Interpreter.SourceParser.Pointer++;
                    break;
                default :
                    Console.Write($"{(char) cell}");
                    return;
            }
        }
    }
    public class BrainfuckRead : IInstruction
    {
        public void Execute(Parser parser)
        {
            switch (parser.Interpreter.InputString[(int)parser.Interpreter.SourceParser.Pointer])
            {
                case '!' :
                    byte val = (byte) Console.Read();
                    parser.Interpreter.WorkingMemory.Cell = val;
                    Console.WriteLine(); // clear to next line to correct printing
                    return;
                default:
                    byte _b;
                    if (byte.TryParse(Console.ReadLine(), out _b))
                    {
                        parser.Interpreter.WorkingMemory.Cell = _b;
                        return;
                    }
                    throw new Exception("Failed to read input.");
            }
        }
    }

    public class BrainfuckRight : IInstruction
    {
        public void Execute(Parser parser)
        {
            parser.Interpreter.WorkingMemory.Right();
        }
    }
    public class BrainfuckLeft : IInstruction
    {
        public void Execute(Parser parser)
        {
            parser.Interpreter.WorkingMemory.Left();
        }
    }

    public class BrainfuckCondLeft : IInstruction
    { 
        public void Execute(Parser parser)
        {
            if (parser.Interpreter.WorkingMemory.Cell > 0)
            {
                new Parser(parser.Interpreter).Run();
            }
            else
            {
                while ((char) parser.Interpreter.SourceParser.Stream.Peek() != ']')
                {
                    parser.Interpreter.SourceParser.Pointer++;
                }
            }
        }
    }
    
    public class BrainfuckCondRight : IInstruction
    { 
        public void Execute(Parser parser)
        {
            if (parser.Interpreter.WorkingMemory.Cell > 0)
            {
                parser.Interpreter.SourceParser.Pointer = parser.entry;
            }
            else
            {
                return;
            }
        }
    }
}