using System;

namespace BrnFkFramework
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            //new Compiler("++[-.]", new Data()).Run();
            new Compiler("++[-.]>++++[-.]", new Data()).Run();
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
                    Console.WriteLine($"[\x1b[33m'.'\x1b[0m, {ip}]\t[\x1b[36m{data.Pointer}\x1b[0m] {data.Value}");
                    //ip++;
                    Console.WriteLine($"shifted to {ip}");
                    //Console.WriteLine(data.Value);
                    break;
                case ',' :
                    Console.WriteLine($"[\x1b[33m','\x1b[0m, {ip}]\tinput.");
                    data.Value = byte.Parse(Console.ReadLine());
                    //ip++;
                    break;
                case '+' :
                    Console.WriteLine($"[\x1b[33m'+'\x1b[0m, {ip}]\t[\x1b[36m{data.Pointer}\x1b[0m] {data.Value} -> {data.Value + 1}");
                    data.Add();
                    //ip++;
                    break;
                case '-' :
                    Console.WriteLine($"[\x1b[33m'-'\x1b[0m, {ip}]\t[\x1b[36m{data.Pointer}\x1b[0m] {data.Value} -> {data.Value - 1}");
                    data.Sub();
                    //ip++;
                    break;
                case '>' :
                    Console.WriteLine($"[\x1b[33m'>'\x1b[0m, {ip}]\t[\x1b[36m{data.Pointer}\x1b[0m] -> [{data.Pointer + 1}]");
                    data.Right();
                    //ip++;
                    break;
                case '<' :
                    Console.WriteLine($"[\x1b[33m'<'\x1b[0m, {ip}]\t[\x1b[36m{data.Pointer}\x1b[0m] -> [{data.Pointer - 1}]");
                    data.Left();
                    //ip++;
                    break;
                case '[' :
                    Console.WriteLine($"[\x1b[33m'['\x1b[0m, {ip}]\t");
                    if (data.Value > 0)
                    {
                        Console.WriteLine($"data value is {data.Value}... entering loop...");
                        //entry_point = ip;
                        int ep = ip; // entry point of loop
                        int xp = ep; // exit point of loop
                        //while (!isEof && input[ip] != ']')
                        while (!isEof)
                        {
                            ip++;
                            ip = Consume(ep);
                            if (!isEof && input[ip] == ']')
                            {
                                if (data.Value > 0)
                                {
                                    Console.WriteLine($"setting ip {ip} to {entry_point}");
                                    ip = entry_point;
                                }
                                else
                                {
                                    //Console.WriteLine("waaaaaa");
                                    return ip++;
                                }
                            }
                            //ip = entry_point;
                            //Console.WriteLine($"+++{ip}");
                        }
                        //ip = xp;
                    }
                    else
                    {
                        Console.WriteLine("Data value is 0, no loop.");
                        while (input[ip] != ']')
                        {
                            Console.WriteLine($"Seeking ']' character. Current: {input[ip]}");
                            ip++;
                        }
                    }
                    break;
                case ']' :
                    Console.WriteLine($"[\x1b[33m']'\x1b[0m, {ip}]\tM");
                    if (data.Value > 0)
                    {
                        Console.WriteLine($"data value greater {ip} :: {entry_point}");
                        ip = entry_point;
                    }
                    else
                    {
                        //ip++;
                        Console.WriteLine($"exiting with return of {ip}");
                        return ip++;
                    }
                    break;
            }
            //return ip ++;
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
        private byte[] data = new byte[30000];

        public byte[] Contents => data;

        public byte Get
        {
            get { return data[pointer]; }
        }

        public byte Value
        {
            get { return data[pointer]; }
            set { data[pointer] = value != null ? value : data[pointer]; }
        }

        private int pointer = 0;
        public int Pointer => pointer;

        public byte Add() => Contents[Pointer]++;
        public byte Sub() => Contents[Pointer]--;
        public void Right() => pointer++;
        public void Left() => pointer--;
        
    }
}