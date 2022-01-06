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
            BrainfuckExtended bfi = new BrainfuckExtended();
            bfi.UseLogger(new LoggerConfiguration().WriteTo.Console().MinimumLevel.Information().CreateLogger());
            bfi.Parse().ParseString("+++*>++[-<+>]<.#");
        }
    }
}