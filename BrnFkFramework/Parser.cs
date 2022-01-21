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

        internal bool IsRunning = false;
        
        internal int Read()
        {
            if (Pointer < Interpreter.InputString.Length)
            {
                return Interpreter.InputString[(int)Pointer++];
            }
            throw new Exception("Read Error");
        }

        internal bool EndOfInput => Pointer >= Interpreter.InputString.Length;
        
        internal int NextInstruction
        {
            get
            {
                if (!Interpreter.SourceParser.EndOfInput)
                {
                    return Interpreter.InputString[(int) Interpreter.SourceParser.Pointer + 1];
                }
                else
                {
                    throw new Exception("End of Stream");
                }
                return Pointer < Interpreter.InputString.Length ? Interpreter.InputString[(int) Pointer + 1] : -1;
            }
        }

        internal int CurrentInstruction => Interpreter.InputString[(int) Interpreter.SourceParser.Pointer];
        internal int LastInstruction { get; set; }

        public Parser(Interpreter interpreter = null)
        {
            Interpreter = interpreter;
            entry = Interpreter?.SourceParser?.Pointer != null ? Interpreter.SourceParser.Pointer : 0;
            Interpreter?.logger?.Debug($"Created Parser with entry point at {entry}");
           
        }

        public void Run()
        {
            IsRunning = true;
            while (IsRunning && !Interpreter.SourceParser.EndOfInput)
            {
                Interpreter.Execute(this);
            }
        }
        
        public void Stop()
        {
            IsRunning = false;
        }

        /// <summary>
        /// parse the input from the given file and execute instructions with provided interpreter.
        /// </summary>
        /// <param name="file">Brainfuck file to be parsed.</param>
        public void ParseFile(string file)
        {
            Interpreter.logger?.Debug($"Attempting to parse file: {file}");
            Interpreter.InputString = File.ReadAllText(file);
            Run();
        }

        /// <summary>
        /// Parse the input from the provided string and execute instructions with the provided interpreter.
        /// </summary>
        /// <param name="str">Brainfuck string to be parsed.</param>
        public void ParseString(string str)
        {
            Interpreter.logger?.Debug("Attempting to parse string.");
            Interpreter.InputString = str;
            Run();
        }
    }
}