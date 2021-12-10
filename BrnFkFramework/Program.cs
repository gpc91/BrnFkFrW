using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using BrnFkFramework.Brainfuck;

namespace BrnFkFramework
{
    internal class  Program
    {
        public static void Main(string[] args)
        {

            Parser parser = new Parser().UseInterpreter(new BrainfuckInterpreter());
            parser.ParseString(",.");
            
            /*
            string file = File.ReadAllText("test.b");
            new Compiler(file, new Data()).RegisterProcessor().Run();
            */
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