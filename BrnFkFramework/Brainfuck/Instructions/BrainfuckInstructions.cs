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
                //parser.Interpreter.logger?.Fatal(parser.Interpreter.InputString[(int) parser.Interpreter.SourceParser.Pointer].ToString());
                switch (parser.Interpreter.InputString[(int) parser.Interpreter.SourceParser.Pointer])
                {
                    case '!' :
                        parser.Interpreter.logger?.Verbose($"Print instruction with modifier '!' (Int)");
                        Console.WriteLine($"{cell}");
                        parser.Interpreter.SourceParser.Pointer++;
                        return;
                    case '@':
                        parser.Interpreter.logger?.Verbose($"Print instruction with modifier '@' (Ext)");
                        Console.WriteLine($"{(char)cell} ({cell})");
                        parser.Interpreter.SourceParser.Pointer++;
                        return;
                    case '£' :
                        parser.Interpreter.logger?.Verbose($"Print instruction with modifier '£' (Int Ext)");
                        Console.WriteLine($"{cell} ({(int)cell})");
                        parser.Interpreter.SourceParser.Pointer++;
                        return;
                    case '#' :
                        parser.Interpreter.logger?.Verbose($"Print instruction with modifier '#' (Mem)");
                        parser.Interpreter.PrintMemory();
                        parser.Interpreter.SourceParser.Pointer++;
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
                while (parser.isRunning && !parser.Interpreter.SourceParser.EndOfInput && parser.Interpreter.SourceParser.CurrentInstruction != ']')
                {
                    char instr = (char) parser.Interpreter.SourceParser.CurrentInstruction;

                    switch (instr)
                    {
                        case '[':
                            parser.Interpreter.logger?.Fatal($"Found new loop while skipping. Entering...");
                            new Parser(parser.Interpreter).Run();
                            break;
                        case ']':
                            break;
                         default: 
                             parser.Interpreter.SourceParser.Pointer++;
                             break;

                    }

                    if (instr == ']' && parser.Interpreter.WorkingMemory.Cell == 0)
                    {
                        parser.Interpreter.SourceParser.Pointer++;
                    }
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
                parser.Stop();
            }
        }
    }
}