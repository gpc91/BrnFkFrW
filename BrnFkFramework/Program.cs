using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
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
            BrainfuckInterpreter bfi = new BrainfuckInterpreter();
            bfi.UseLogger(new LoggerConfiguration().WriteTo.Console().MinimumLevel.Debug().CreateLogger());
            bfi.PrintMemory();
            //bfi.Parse().ParseString("+++>++[-<+>]<.#");
            //bfi.Parse().ParseString("-[-[-[-[-[-[-[-[-]>>>]>>>]>>>]>>>]>>>]>>>]>>>]>>>+");
            //bfi.Parse().ParseString("[[+-[+-]>+<-]>++<]"); // Recursion fixer!!!
            //bfi.Parse().ParseString("++>+++<[>[->+<]<-]"); // Double tested recursion
            bfi.Parse().ParseString("++[-[-[-[-[-[-[-[-]>]>]>]>]>]>]>]++.");
            bfi.PrintMemory(limit: 50);
            Console.WriteLine(bfi.WorkingMemory.Pointer);
        }
    }
}