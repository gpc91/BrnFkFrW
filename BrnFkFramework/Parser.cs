using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace BrnFkFramework
{
    /// <summary>
    /// Reads input and triggers interpreter actions based upon source tokens.
    /// </summary>
    public class Parser
    {
        
        /// <summary>
        /// the interpreter associated with this parser. 
        /// </summary>
        public Interpreter Interpreter;
        internal long entry;
        internal long Pointer = 0;

        internal StreamReader Stream;
        internal StreamReader Reader => Stream;

        internal int Read()
        {
            if (Pointer < Interpreter.InputString.Length)
            {
                return Interpreter.InputString[(int)Pointer++];
            }
            throw new Exception("Read Error");
        }

        internal bool EndOfInput => Pointer >= Interpreter.InputString.Length - 1;
        

        internal int NextInstruction => Pointer < Interpreter.InputString.Length ? Interpreter.InputString[(int)Pointer+1] : -1;
        
        public Parser(Interpreter interpreter = null)
        {
            Interpreter = interpreter;
            entry = Interpreter?.SourceParser?.Pointer != null ? Interpreter.SourceParser.Pointer : 0;
            Interpreter?.logger?.Debug($"Created Parser with entry point at {entry}");
           
        }

        public void Run()
        {
            while (!Interpreter.SourceParser.EndOfInput)
            {
                Interpreter.Execute(this);
            }
        }

        /// <summary>
        /// parse the input from the given file and execute instructions with provided interpreter.
        /// </summary>
        /// <param name="file"></param>
        public void ParseFile(string file)
        {
            /*
            try
            {
                using (FileStream fs = File.OpenRead(file))
                {
                    using (StreamReader sr = new(fs))
                    {
                        Stream = sr;
                        Run();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            */
            Interpreter.logger?.Debug($"Attempting to parse file: {file}");
            Interpreter.InputString = File.ReadAllText(file);
            Run();
        }

        public void ParseString(string str)
        {
            /*
            using (MemoryStream ms = new(Encoding.ASCII.GetBytes(str)))
            {
                using (StreamReader sr = new(ms))
                {
                    Stream = sr;
                    Console.WriteLine("Running String parse...");
                    Run();
                }
            }
            */
            Interpreter.logger?.Debug("Attempting to parse string.");
            Interpreter.InputString = str;
            Run();
        }
    }
}