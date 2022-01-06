using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using BrnFkFramework.Brainfuck;

namespace BrnFkFramework
{

    internal class Runner
    {
        internal TestParser parser;
        internal int entry = 0;

        internal Runner(TestParser tp = null)
        {
            parser = tp;
            entry = tp.ptr;
            //Console.WriteLine($"New Runner created with entry point {entry}[{tp.str[entry]}]");
            Run();
        }

        internal void Incr()
        {
            if (parser.memval[parser.memptr] + 1 > ushort.MaxValue)
            {
                parser.memval[parser.memptr] = 0;
            }
            else
            {
                parser.memval[parser.memptr]++;
            }
            parser.ptr++;
        }
        
        internal void Run()
        {
            while (parser.ptr < parser.str.Length)
            {
                switch (parser.GetChar)
                {
                    case '+':
                        Incr();
                        parser.PrintMemory();
                        break;
                    case '-':
                        if (parser.memval[parser.memptr] > 0)
                        {
                            parser.memval[parser.memptr]--;  
                            parser.PrintMemory();
                        }
                        parser.ptr++;
                        break;
                    case '.':
                        Console.WriteLine($"{(char)parser.memval[parser.memptr]} [{parser.memval[parser.memptr]}]");
                        parser.ptr++;
                        break;
                    case ',' :
                        if (ushort.TryParse(Console.ReadLine(), out parser.memval[parser.memptr]))
                        {
                            Console.WriteLine($"Parsed input and got {parser.memval[parser.memptr]}");
                            parser.ptr++;
                            parser.PrintMemory();
                            break;
                        }
                        throw new Exception("Failed to parse input.");
                    case '[':
                        //Console.WriteLine("Got [ char...");
                        if (parser.memval[parser.memptr] > 0)
                        {
                            //Console.WriteLine($"value is {parser.val}, entering loop");
                            parser.ptr++;
                            new Runner(parser);    
                        }
                        else
                        {
                            //Console.WriteLine("value was 0, seeking ]");
                            while (parser.ptr < parser.str.Length && parser.GetChar != ']')
                            {
                                //Console.WriteLine($"\tskipped {parser.GetChar}");
                                parser.ptr++;
                            }
                        }
                        break;
                    case ']':
                        if (parser.memval[parser.memptr] == 0)
                        {
                            //Console.WriteLine($"] value is {parser.val}, breaking!");
                            parser.ptr++;
                            return;    
                        }
                        else
                        {
                            //Console.WriteLine($"] value is {parser.val}: jumping back to entry with value {parser.str[entry]}");
                            parser.ptr = entry;
                        }
                        break;
                    case '>' :
                        if (parser.memptr < parser.memval.Length-1)
                        {
                            parser.memptr++;
                            //Console.WriteLine($"moved right to {parser.memptr}");
                        }
                        else
                        {
                            parser.ptr++;
                        }
                        break;
                    case '<' :
                        if (parser.memptr > 0)
                        {
                            parser.memptr--;
                        }
                        else
                        {
                            parser.ptr++;
                        }
                        break;
                    default :
                        parser.ptr++;
                        continue;
                }
                

            }
        }
        
    }
    internal class TestParser
    {
        internal int ptr = 0;
        internal string str = "+++++>+++[-<+>]";
        internal ushort val = 0;
        internal ushort[] memval = new ushort[4];
        internal int memptr = 0;
        internal char GetChar => str[ptr];
        internal void Run()
        {
            new Runner(this);
        }
        
        public void PrintMemory()
        {
            Console.Write("[");
            foreach (ushort val in memval)
            {
                Console.Write($"{val}, ");
            }
            Console.WriteLine("]");
        }
    }
    
    internal class  Program
    {
        public static void Main(string[] args)
        {
            Parser parser = new Parser().UseInterpreter(new BrainfuckInterpreter());
            parser.ParseString(">>>,>>,!<<<<<+++.@>.@+++++.@<.@>.@");
            parser.Interpreter.PrintMemory();

            //parser.ParseString("+++[.@-].@");
  
        }
    }
/*
    internal class Compiler
    {
        private int ip = 0; // instruction pointer
        internal int InstructionPointer{
            get { return ip; }
            set { ip = value; }
        }
        private Data data;
        internal Data Memory => data;

        // TODO: Maybe seperate the source and it's accessors? 
        private string input;
        internal string Input => input;

        internal char CurrentCharacter => input[ip];
        // It is important to check if the next character is not out of bound of the source. If it is, return white space (for now).
        internal char NextCharacter => (ip >= input.Length-1) ? ' ' : input[ip + 1];

        private bool isEof => ip >= input.Length;
        internal bool IsEOF => isEof;
        
        public Compiler(string input, Data data)
        {
            this.data = data;
            this.input = input;
        }

        internal void Consume()
        {
            Processor.Process(input[ip]);
        }

        public Compiler RegisterProcessor()
        {
            Processor.Compiler(this);
            return this;
        }
        
        public void Run()
        {
            while (!IsEOF)
            {
                Consume();
            }
        }

    }
/*
    internal static class Processor
    {
        private static Compiler _compiler;
        
        private static Dictionary<char, IInstruction> Commands = new Dictionary<char, IInstruction>()
        {
            {'.', new PrintCommand()},
            {'^', new TestCommand()},
            {',', new ReadCommand()},
            {'+', new MemoryAddCommand()},
            {'-', new MemorySubtractCommand()},
            {'>', new MemoryShiftRightCommand()},
            {'<', new MemoryShiftLeftCommand()},
            {'[', new ConditionalLeftCommand()},
            {']', new ConditionalRightCommand()},
            //{' ', new WhitespaceCommand()},
            {'#', new CommentCommand()},
            //{'\n', new WhitespaceCommand()}
        };
        
        public static void Process(char input)
        {
            try
            {
                // TODO: Try/Catch for unrecognised chars
                Commands[input]?.Execute(_compiler);
                _compiler.InstructionPointer++;
            }
            catch (Exception e)
            {
                //Console.WriteLine(e);
                _compiler.InstructionPointer++;
                //throw;
            }
        }

        public static void Compiler(Compiler compiler)
        {
            _compiler = compiler;
        }
        
        
    }
    internal class PrintCommand : IInstruction
    {
        public void Execute(Compiler compiler)
        {
            //if (compiler.InstructionPointer < compiler.Input.Length-1)
            {
                switch (compiler.NextCharacter)
                {
                    case '!' :                      // force byte
                        Console.WriteLine($"{compiler.Memory.Value}");
                        compiler.InstructionPointer++;
                        return;
                    case '@' :                      // force byte debug
                        Console.WriteLine($"{(char)compiler.Memory.Value} ({compiler.Memory.Value})");
                        compiler.InstructionPointer++;
                        return;
                    case '£' :
                        Console.Write($"{(char)compiler.Memory.Value} ({compiler.Memory.Value})");
                        compiler.InstructionPointer++;
                        return;
                    default :
                        Console.Write($"{(char)compiler.Memory.Value}");
                        //Console.WriteLine($"[\x1b[33m'.'\x1b[0m, {ip}]\t[\x1b[36m{data.Pointer}\x1b[0m] {data.Value} ::'{(char)data.Value}'");
                        return;
                }
                //Console.WriteLine($"[\x1b[33m'.'\x1b[0m, {ip}]\t[\x1b[36m{data.Pointer}\x1b[0m] {data.Value} ::{data.Value}");
            }
        }
    }

    internal class ReadCommand : IInstruction
    {
        public void Execute(Compiler compiler)
        {
            compiler.Memory.Value = byte.Parse(Console.ReadLine());
        }
    }

    internal class WhitespaceCommand : IInstruction
    {
        public void Execute(Compiler compiler)
        {
            while (!compiler.IsEOF)
            {
                if (compiler.NextCharacter != ' ')
                {
                    return;
                }
                else
                {
                    compiler.InstructionPointer++;
                }
            }
        }
    }

    internal class CommentCommand : IInstruction
    {
        public void Execute(Compiler compiler)
        {
            while (!compiler.IsEOF)
            {
                if (compiler.NextCharacter == '#' || compiler.NextCharacter == '\n')
                {
                    compiler.InstructionPointer++;
                    return;
                }
                else
                {
                    compiler.InstructionPointer++;
                }
            }
        }
    }

    internal class MemoryAddCommand : IInstruction
    {
        public void Execute(Compiler compiler)
        {
            compiler.Memory.Add();
        }
    }

    internal class MemorySubtractCommand : IInstruction
    {
        public void Execute(Compiler compiler)
        {
            compiler.Memory.Sub();
        }
    }

    internal class MemoryShiftRightCommand : IInstruction
    {
        public void Execute(Compiler compiler)
        {
            compiler.Memory.Right();
        }
    }

    internal class MemoryShiftLeftCommand : IInstruction
    {
        public void Execute(Compiler compiler)
        {
            compiler.Memory.Left();
        }
    }
    
    internal class ConditionalLeftCommand : IInstruction
    {
        public void Execute(Compiler compiler)
        {
            // TODO: skip behaviour - if the value at cell is 0 proceed along source until we hit '[' or ']'?
            int entry = ++compiler.InstructionPointer;
            while (!compiler.IsEOF)
            {
                compiler.Consume();
                if (compiler.CurrentCharacter == ']')
                {
                    if (compiler.Memory.Value > 0)
                    {
                        compiler.InstructionPointer = entry;
                    }
                    else
                    {
                        // we now need to break out of the loop as the pointer needs to be advanced by the processor
                        return;
                    }
                }
            }
        }
    }
    
    internal class ConditionalRightCommand : IInstruction
    {
        public void Execute(Compiler compiler)
        {
            return;
        }
    }

    internal class TestCommand : IInstruction
    {
        public void Execute(Compiler compiler)
        {
            Console.WriteLine("This is a test command!");
        }
    }
    */
    
    internal class Data
    {
        private byte[] data;

        private byte[] Block => data;
        
        public byte Value
        {
            get { return data[pointer]; }
            set { data[pointer] = value; }
        }
        
        private int pointer = 0;
        public int Pointer => pointer;

        public Data(int memorySize = 30000)
        {
            data = new byte[memorySize];
        }
        
        public byte Add() => Block[Pointer]++;
        public byte Sub() => Block[Pointer]--;
        public void Right() => pointer++;
        public void Left() => pointer--;
        
    }
}