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

            /*
            BrainfuckRunner brainfuck = new BrainfuckRunner();
            brainfuck.Debug("+++.@");
            brainfuck.PrintMemory();
            */
            
            new BrainfuckInterpreter().Parse().ParseString("+++>++<.@>.@");
            
            
            // Change all of the below to make the 'root' be the interpreter.
            //Parser parser = new Parser().UseInterpreter(new BrainfuckInterpreter());
            //parser.ParseString(">>>,>>,!<<<<<+++.@>.@+++++.@<.@>.@");
            //parser.Interpreter.PrintMemory();

            //parser.ParseString("+++[.@-].@");

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
    // parser
    internal class Runner
    {
        internal TestParser parser; // interpreter
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
    // 'interpreter'
    internal class TestParser
    {
        internal int ptr = 0;
        internal string str = "+++++>+++[-<+>]";
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
}