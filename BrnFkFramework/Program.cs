using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using BrnFkFramework.Brainfuck;
using BrnFkFramework.Definitions;
using Serilog;

namespace BrnFkFramework
{
    internal class Program
    {
        public static void Main(string[] args)
        {

            int value = 0;
            
            Dictionary<char, Func<bool>> d = new Dictionary<char, Func<bool>>()
            {
                {
                    '+', () =>
                    {
                        Console.WriteLine("adderino");
                        value++;
                        return true;
                    }
                },
                {
                    '-', () =>
                    {
                        Console.WriteLine("subberini");
                        value--;
                        return true;
                    }
                },
                {
                    '.', () =>
                    {
                        Console.WriteLine(value);
                        return true;
                    }
                }
            };

            d['.']();
            d['+']();
            d['.']();
            d['-']();

            //d.Add('.', () => { Console.WriteLine("DOT"); return true;});

            d['.']();

            Environment.Exit(1);
            
            BrainfuckInterpreter bfi = new BrainfuckInterpreter();
            bfi.UseLogger(new LoggerConfiguration().WriteTo.Console().MinimumLevel.Debug().CreateLogger());
            bfi.Parse().ParseString("+++>++[-<+>].#");
        }
    }
}