using System;
using System.Diagnostics;
using System.Runtime.Remoting.Messaging;

namespace BrnFkFramework.Brainfuck.Instructions
{
    public class BrainfuckAdd : IInstruction
    {
        public void Execute(Parser parser)
        {
            parser.Interpreter.logger?.Verbose("Cell Add");
            parser.Interpreter.WorkingMemory.Add();
        }
    }
    
    public class BrainfuckSub : IInstruction
    {
        public void Execute(Parser parser)
        {
            parser.Interpreter.logger?.Verbose("Cell Subtract");
            parser.Interpreter.WorkingMemory.Sub();
        }
    }
    public class BrainfuckPrint : IInstruction
    {
        public void Execute(Parser parser)
        {
            // TODO: Make this better. Right now it is lame and doesn't properly handle last characters
            if (parser.Interpreter.SourceParser.EndOfInput)
            {
                parser.Interpreter.logger?.Verbose($"Print instruction");
                Console.Write($"{(char) parser.Interpreter.WorkingMemory.Cell}");
            }
            else
            {
                byte cell = parser.Interpreter.WorkingMemory.Cell;
                switch (parser.Interpreter.InputString[(int) parser.Interpreter.SourceParser.Pointer])
                {
                    case '!' :
                        parser.Interpreter.logger?.Verbose($"Print instruction with Integer modifier '!'");
                        Console.WriteLine($"{cell}");
                        parser.Interpreter.SourceParser.Pointer++;
                        return;
                    case '&' :
                        parser.Interpreter.logger?.Verbose($"Print instruction with Detail modifier '&'");
                        int ptr = parser.Interpreter.WorkingMemory.Pointer;
                        Console.WriteLine($"[{ptr}] {(char)cell} ({(int)cell}) #{String.Format("{0:X2}", cell)}");
                        parser.Interpreter.SourceParser.Pointer++;
                        return;
                    case '@' :
                        parser.Interpreter.logger?.Verbose($"Print instruction with Mem modifier '@'");
                        parser.Interpreter.PrintMemory(start: parser.Interpreter.WorkingMemory.Pointer-3, limit: 7, hex: true);
                        parser.Interpreter.SourceParser.Pointer++;
                        return;
                    case '#' :
                        parser.Interpreter.logger?.Verbose($"Print instruction with Hex modifier '#'");
                        Console.WriteLine("{0:X2}", cell);
                        return;
                    default :
                        parser.Interpreter.logger?.Verbose($"Print instruction");
                        Console.Write($"{(char) cell}");
                        return;
                }    
            }
        }
    }
    public class BrainfuckRead : IInstruction
    {
        public void Execute(Parser parser)
        {
            switch (parser.Interpreter.InputString[(int)parser.Interpreter.SourceParser.Pointer])
            {
                case '*' :
                    parser.Interpreter.logger?.Verbose($"Read instruction (Explicit)");
                    byte _b;
                    if (byte.TryParse(Console.ReadLine(), out _b))
                    {
                        parser.Interpreter.WorkingMemory.Cell = _b;
                        return;
                    }
                    else
                    {
                        parser.Interpreter.logger?.Fatal($"Explicit Read instruction failed.");
                        throw new Exception("Failed to read input.");
                    }
                default:
                    parser.Interpreter.logger?.Verbose($"Read instruction");
                    byte val = (byte) Console.Read();
                    parser.Interpreter.WorkingMemory.Cell = val;
                    //Console.WriteLine(); // clear to next line to correct printing
                    return;
            }
        }
    }

    public class BrainfuckRight : IInstruction
    {
        public void Execute(Parser parser)
        {
            parser.Interpreter.logger?.Verbose("Shift right");
            parser.Interpreter.WorkingMemory.Right();
        }
    }
    public class BrainfuckLeft : IInstruction
    {
        public void Execute(Parser parser)
        {
            parser.Interpreter.logger?.Verbose("Shift left");
            parser.Interpreter.WorkingMemory.Left();
        }
    }

    public class BrainfuckCondLeft : IInstruction
    { 
        public void Execute(Parser parser)
        {
            parser.Interpreter.logger?.Debug("Checking conditional '[' ");
            if (parser.Interpreter.WorkingMemory.Cell > 0)
            {
                parser.Interpreter.logger?.Debug($"Value {parser.Interpreter.WorkingMemory.Cell} at memory cell {parser.Interpreter.WorkingMemory.Pointer} is greater than 0. Condition met.");
                new Parser(parser.Interpreter).Run();
            }
            else
            {
                parser.Interpreter.logger?.Debug($"Condition was not met at memory cell {parser.Interpreter.WorkingMemory.Pointer}, skipping block...");
                while (parser.IsRunning && !parser.Interpreter.SourceParser.EndOfInput && parser.Interpreter.SourceParser.CurrentInstruction != ']')
                {
                    char instr = (char) parser.Interpreter.SourceParser.CurrentInstruction;

                    if (instr == '[')
                    {
                        parser.Interpreter.logger?.Fatal($"Found new conditional. Entering...");
                        new Parser(parser.Interpreter).Run();
                    }

                    parser.Interpreter.SourceParser.Pointer++;
                }
            }
        }
    }
    
    //TODO: The conditional right needs to be more thoroughly fixed so that it no longer breaks out of the root parser
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
                if (parser != parser.Interpreter.SourceParser)
                {
                    parser.Stop();
                }
            }
        }
    }
}