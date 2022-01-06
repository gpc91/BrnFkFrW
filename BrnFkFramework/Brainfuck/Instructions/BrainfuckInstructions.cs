using System;

namespace BrnFkFramework.Brainfuck.Instructions
{
    public class BrainfuckAdd : IInstruction
    {
        public void Execute(Interpreter interpreter)
        {
            interpreter.WorkingMemory.Add();
        }
        public void Execute(Parser parser)
        {
            parser.Interpreter.WorkingMemory.Add();
        }
    }
    
    public class BrainfuckSub : IInstruction
    {
        public void Execute(Interpreter interpreter)
        {
            interpreter.WorkingMemory.Sub();
        }
        public void Execute(Parser parser)
        {
            parser.Interpreter.WorkingMemory.Sub();
        }
    }
    public class BrainfuckPrint : IInstruction
    {
        public void Execute(Interpreter interpreter)
        {
            byte cell = interpreter.WorkingMemory.Cell;
            switch ((char) interpreter.SourceParser.NextInstruction)
            {
                case '!' :
                    Console.WriteLine($"{cell}");
                    interpreter.SourceParser.Reader.BaseStream.Position++;
                    return;
                case '@' :
                    Console.WriteLine($"{(char)cell} ({cell})");
                    interpreter.SourceParser.Reader.BaseStream.Position++;
                    return;
                case '£' :
                    Console.WriteLine($"{cell} ({(int)cell})");
                    interpreter.SourceParser.Reader.BaseStream.Position++;
                    return;
                default :
                    Console.Write($"{(char)cell}");
                    return;
            }
        } 
        public void Execute(Parser parser)
        {
            byte cell = parser.Interpreter.WorkingMemory.Cell;
            switch ((char) parser.Interpreter.SourceParser.NextInstruction)
            {
                case '!' :
                    Console.WriteLine($"{cell}");
                    parser.Interpreter.SourceParser.Reader.BaseStream.Position++;
            }
        }
    }
    public class BrainfuckRead : IInstruction
    {
        public void Execute(Interpreter interpreter)
        {
            switch ((char) interpreter.SourceParser.NextInstruction)
            {
                case '!':
                    byte val = (byte) Console.Read();
                    interpreter.WorkingMemory.Cell = val;
                    return;
                default:

                    byte _b;
                    if (byte.TryParse(Console.ReadLine(), out _b))
                    {
                        interpreter.WorkingMemory.Cell = _b;
                        return;
                    }

                    throw new Exception("Failed to read input.");
       
            }
        }
        public void Execute(Parser parser)
        {
            
        }
    }

    public class BrainfuckRight : IInstruction
    {
        public void Execute(Interpreter interpreter)
        {
            interpreter.WorkingMemory.Right();
        }
        public void Execute(Parser parser)
        {
            
        }
    }
    public class BrainfuckLeft : IInstruction
    {
        public void Execute(Interpreter interpreter)
        {
            interpreter.WorkingMemory.Left();
        }
        public void Execute(Parser parser)
        {
            
        }
    }

    public class BrainfuckCondLeft : IInstruction
    {
        public void Execute(Interpreter interpreter)
        {
            if (interpreter.WorkingMemory.Cell > 0)
            {
                new ParseWorker(interpreter.SourceParser);
            }
            else
            {
                while ((char)interpreter.SourceParser.Stream.Peek() != ']')
                {
                    interpreter.SourceParser.Stream.Read();
                }
            }
        }
        
        public void Execute(Parser parser)
        {
            
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
            
        }
    }
}