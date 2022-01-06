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
            switch ((char) parser.Interpreter.SourceParser.NextInstruction)
            {
                case '!' :
                    Console.WriteLine($"{cell}");
                    parser.Interpreter.SourceParser.Reader.Read();
                    return;
                case '@':
                    Console.WriteLine($"{(char)cell} ({cell})");
                    parser.Interpreter.SourceParser.Reader.Read();
                    return;
                case '£' :
                    Console.WriteLine($"{cell} ({(int)cell})");
                    parser.Interpreter.SourceParser.Reader.Read();
                    return;
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
            switch ((char) parser.Interpreter.SourceParser.NextInstruction)
            {
                case '!' :
                    byte val = (byte) Console.Read();
                    parser.Interpreter.WorkingMemory.Cell = val;
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
                new Parser(parser.Interpreter);
            }
            else
            {
                while ((char) parser.Interpreter.SourceParser.Stream.Peek() != ']')
                {
                    parser.Interpreter.SourceParser.Stream.Read();
                }
            }
        }
    }
    
    public class BrainfuckCondRight : IInstruction
    {
        public void Execute(Interpreter interpreter)
        {
            if (interpreter.WorkingMemory.Cell > 0)
            {
                //interpreter.SourceParser.Stream.BaseStream.Position = // should be parser entry
            }
        }

        public void Execute(Parser parser)
        {
            if (parser.Interpreter.WorkingMemory.Cell > 0)
            {
                parser.Interpreter.SourceParser.Stream.BaseStream.Position = parser.entry;
                Console.WriteLine($"[ found, jumping back to entry point {parser.entry}");
            }
            else
            {
                parser.Interpreter.SourceParser.Reader.Read();
            }
        }
    }
}