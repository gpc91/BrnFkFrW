using System;
using System.IO;
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
        
        /// <summary>
        /// parse input from the StreamReader object and execute instruction with the provided interpreter.
        /// </summary>
        /// <param name="stream"></param>
        public void Parse(StreamReader stream)
        {
            Reader = stream;
            while (!stream.EndOfStream)
            {
                char c = (char) stream.Read();
                NextInstruction = stream.Peek();
                Interpreter.Execute(c);
                //Console.Write(c);
                
                // do stuff
            }
        }

        public int NextInstruction = -1;
        
        public StreamReader Reader { get; private set; }
        
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