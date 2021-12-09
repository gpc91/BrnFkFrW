using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace BrnFkFramework
{
    internal class  Program
    {
        public static void Main(string[] args)
        {
            string file = File.ReadAllText("test.b");
            Console.WriteLine(file);
            new Compiler(file, new Data()).RegisterProcessor().Run();
            
            //new Compiler("+>+>+>+<+<+<+>+>+>+", new Data(4)).Run();
            //new Compiler("+++>++++[-<+>]<.", new Data()).Run();
        }
    }

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
            while (!isEof)
            {
                Consume();
            }
        }

    }

    internal static class Processor
    {
        private static Compiler _compiler;
        
        private static Dictionary<char, ICommand> Commands = new Dictionary<char, ICommand>()
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
            {' ', new WhitespaceCommand()},
            {'#', new CommentCommand()}
        };
        
        public static void Process(char input)
        {
            Commands[input]?.Execute(_compiler);
            _compiler.InstructionPointer++;
        }

        public static void Compiler(Compiler compiler)
        {
            _compiler = compiler;
        }
        
        
    }

    internal interface ICommand
    {
        public void Execute(Compiler compiler);
    }

    internal class PrintCommand : ICommand
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
                    default :
                        Console.Write($"{(char)compiler.Memory.Value}");
                        //Console.WriteLine($"[\x1b[33m'.'\x1b[0m, {ip}]\t[\x1b[36m{data.Pointer}\x1b[0m] {data.Value} ::'{(char)data.Value}'");
                        return;
                }
                return;
                //Console.WriteLine($"[\x1b[33m'.'\x1b[0m, {ip}]\t[\x1b[36m{data.Pointer}\x1b[0m] {data.Value} ::{data.Value}");
            }
        }
    }

    internal class ReadCommand : ICommand
    {
        public void Execute(Compiler compiler)
        {
            compiler.Memory.Value = byte.Parse(Console.ReadLine());
        }
    }

    internal class WhitespaceCommand : ICommand
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

    internal class CommentCommand : ICommand
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

    internal class MemoryAddCommand : ICommand
    {
        public void Execute(Compiler compiler)
        {
            compiler.Memory.Add();
        }
    }

    internal class MemorySubtractCommand : ICommand
    {
        public void Execute(Compiler compiler)
        {
            compiler.Memory.Sub();
        }
    }

    internal class MemoryShiftRightCommand : ICommand
    {
        public void Execute(Compiler compiler)
        {
            compiler.Memory.Right();
        }
    }

    internal class MemoryShiftLeftCommand : ICommand
    {
        public void Execute(Compiler compiler)
        {
            compiler.Memory.Left();
        }
    }
    
    internal class ConditionalLeftCommand : ICommand
    {
        public void Execute(Compiler compiler)
        {
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
    
    internal class ConditionalRightCommand : ICommand
    {
        public void Execute(Compiler compiler)
        {
            return;
        }
    }

    internal class TestCommand : ICommand
    {
        public void Execute(Compiler compiler)
        {
            Console.WriteLine("This is a test command!");
        }
    }
    
    
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