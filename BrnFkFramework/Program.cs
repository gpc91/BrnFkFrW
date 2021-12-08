using System;
using System.IO;

namespace BrnFkFramework
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            string file = File.ReadAllText("test.b");
            Console.WriteLine(file);
            new Compiler(file, new Data()).Run();
            //new Compiler("+>+>+>+<+<+<+>+>+>+", new Data(4)).Run();
            //new Compiler("+++>++++[-<+>]<.", new Data()).Run();
        }
    }

    internal class Compiler
    {
        private int ip = 0; // instruction pointer
        private Data data;

        private string input;

        private bool isEof => ip >= input.Length;
        
        public Compiler(string input, Data data)
        {
            this.data = data;
            this.input = input;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>return the exit point</returns>
        private int Consume(int entry_point = 0)
        {
            switch (input[ip])
            {
                case '.' : 
                    // force byte
                    if (ip < input.Length-1 && input[ip + 1] == '!')
                    {
                        Console.WriteLine($"[\x1b[33m'.'\x1b[0m, {ip}]\t[\x1b[36m{data.Pointer}\x1b[0m] {data.Value} ::{data.Value}");
                        ip++;
                    }
                    else
                    {
                        Console.WriteLine($"[\x1b[33m'.'\x1b[0m, {ip}]\t[\x1b[36m{data.Pointer}\x1b[0m] {data.Value} ::'{(char)data.Value}'");
                    }
                    //Console.WriteLine(data.Value);
                    break;
                case ',' :
                    //Console.WriteLine($"[\x1b[33m','\x1b[0m, {ip}]\tinput.");
                    data.Value = byte.Parse(Console.ReadLine());
                    break;
                case '+' :
                    Console.WriteLine($"[\x1b[33m'+'\x1b[0m, {ip}]\t[\x1b[36m{data.Pointer}\x1b[0m] {data.Value} -> {data.Value + 1}");
                    data.Add();
                    break;
                case '-' :
                    //Console.WriteLine($"[\x1b[33m'-'\x1b[0m, {ip}]\t[\x1b[36m{data.Pointer}\x1b[0m] {data.Value} -> {data.Value - 1}");
                    data.Sub();
                    break;
                case '>' :
                    //Console.WriteLine($"[\x1b[33m'>'\x1b[0m, {ip}]\t[\x1b[36m{data.Pointer}\x1b[0m] -> [{data.Pointer + 1}]");
                    data.Right();
                    break;
                case '<' :
                    //Console.WriteLine($"[\x1b[33m'<'\x1b[0m, {ip}]\t[\x1b[36m{data.Pointer}\x1b[0m] -> [{data.Pointer - 1}]");
                    data.Left();
                    break;
                case '[' :
                    //Console.WriteLine($"[\x1b[33m'['\x1b[0m, {ip}]\t");
                    if (data.Value > 0)
                    {
                        //Console.WriteLine($"data value is {data.Value}... entering loop...");
                        int ep = ip; // entry point of loop
                        while (!isEof)
                        {
                            ip++;
                            ip = Consume(ep);
                            
                            if (!isEof && input[ip] == ']')
                            {
                                if (data.Value > 0)
                                {
                                    //Console.WriteLine($"setting ip {ip} to {entry_point}");
                                    ip = entry_point;
                                }
                                else
                                {
                                    return ip++;
                                }
                            }
                        }
                    }
                    else
                    {
                        //Console.WriteLine("Data value is 0, no loop.");
                        while (input[ip] != ']')
                        {
                            //Console.WriteLine($"Seeking ']' character. Current: {input[ip]}");
                            ip++;
                        }
                    }
                    break;
                case ']' :
                    //Console.WriteLine($"[\x1b[33m']'\x1b[0m, {ip}]\tM");
                    if (data.Value > 0)
                    {
                        //Console.WriteLine($"data value greater {ip} :: {entry_point}");
                        ip = entry_point;
                    }
                    else
                    { 
                        //Console.WriteLine($"exiting with return of {ip}");
                        return ip++;
                    }
                    break;
                case '#':
                    // implement comments
                    break;
            }
            return ip++;
        }
        
        public void Run()
        {
            while (!isEof)
            {
                Consume();
            }
        }

    }

    internal class Data
    {
        private byte[] data;

        public byte[] Contents => data;
        
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
        
        public byte Add() => Contents[Pointer]++;
        public byte Sub() => Contents[Pointer]--;
        public void Right() => pointer++;
        public void Left() => pointer--;
        
    }
}