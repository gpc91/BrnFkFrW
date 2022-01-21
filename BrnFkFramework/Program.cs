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
            bfi.UseLogger(new LoggerConfiguration().WriteTo.Console().MinimumLevel.Fatal().CreateLogger());
            bfi.SourceParser.ParseFile("hello_world.b");
        }
    }
}