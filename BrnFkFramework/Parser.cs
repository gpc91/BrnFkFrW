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
            entry = parser.stream.BaseStream.Position;
            Run();
        }

        internal void Run()
        {
            Console.WriteLine("Running new worker...");
            while (!_parser.stream.EndOfStream)
            {
                _parser.Interpreter.Execute((char)_parser.stream.Read());
            }
        }
    }
    /// <summary>
    /// Reads input and triggers interpreter actions based upon source tokens.
    /// </summary>
    public class Parser
    {

        private Parser _main;
        internal StreamReader stream;
        private long _entry;
        internal StreamReader Reader => stream;

        internal int NextInstruction => stream != null ? stream.Peek() : -1;
        
        public Parser(Parser parser = null, long entry = 0)
        {
            _main = parser;
            _entry = entry;
        }
        
        /// <summary>
        /// the interpreter associated with this parser. 
        /// </summary>
        public Interpreter Interpreter;
        
        /// <summary>
        /// parse input from the StreamReader object and execute instruction with the provided interpreter.
        /// </summary>
        /// <param name="stream"></param>
        public void Parse(StreamReader sr)
        {
            Console.WriteLine("Starting parse...");
            stream = sr;
            new ParseWorker(this);
            //Reader = stream;
            //Run(stream);
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