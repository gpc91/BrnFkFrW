using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace BrnFkFramework
{

    public class ParseWorker
    {
        private Parser _parser;
        
        // the entry point of this worker (used for recursion)
        private long entry;
        
        internal Interpreter _interpreter;
        
        public ParseWorker(Parser parser)
        {
            _parser = parser;
            _interpreter = parser.Interpreter;
            entry = parser.Stream.BaseStream.Position;
            Run();
        }

        internal void Run()
        {
            Console.WriteLine("Running new worker...");
            while (!_parser.Stream.EndOfStream)
            {
                _parser.Interpreter.Execute((char)_parser.Stream.Read());
            }
        }
    }
    /// <summary>
    /// Reads input and triggers interpreter actions based upon source tokens.
    /// </summary>
    public class Parser
    {
        
        /// <summary>
        /// the interpreter associated with this parser. 
        /// </summary>
        public Interpreter Interpreter;
        private long _entry;
        
        internal StreamReader Stream;
        internal StreamReader Reader => Stream;
        
        

        internal int NextInstruction => Stream != null ? Stream.Peek() : -1;
        
        public Parser(Interpreter interpreter = null)
        {
            Interpreter = interpreter;
            _entry = Interpreter.SourceParser.Stream.BaseStream.Position;
        }

        public void Run()
        {
            while (!Interpreter.SourceParser.Stream.EndOfStream)
            {
                Interpreter.Execute(this);
            }
        }
        
        
        /// <summary>
        /// parse input from the StreamReader object and execute instruction with the provided interpreter.
        /// </summary>
        /// <param name="stream"></param>
        public void Parse(StreamReader sr)
        {
            Console.WriteLine("Starting parse...");
            Stream = sr;
            new Parser(Interpreter);
            //Reader = Stream;
            //Run(Stream);
        }

        public void Run(StreamReader stream)
        {
            while (!stream.EndOfStream)
            {
                CurrentInstruction = stream.Read();
                Interpreter.Execute((char)CurrentInstruction);
            }
        }
        public int CurrentInstruction = -1;
        /// <summary>
        /// parse the input from the given file and execute instructions with provided interpreter.
        /// </summary>
        /// <param name="file"></param>
        public void ParseFile(string file)
        {
            try
            {
                using (FileStream fs = File.OpenRead(file))
                {
                    using (StreamReader sr = new(fs))
                    {
                        Parse(sr);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public void ParseString(string str)
        {
            using (MemoryStream ms = new(Encoding.ASCII.GetBytes(str)))
            {
                using (StreamReader sr = new(ms))
                {
                    Parse(sr);
                }
            }
        }

        public Parser UseInterpreter(Interpreter interpreter)
        {
            Interpreter = interpreter;
            return interpreter.SourceParser = this;
        }
    }
}