using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace BrnFkFramework
{
    /// <summary>
    /// Holds and manipulates program state and executes program instructions provided to it.
    /// </summary>
    public abstract class Interpreter
    {

        public Parser SourceParser { get; set; }

        internal Memory WorkingMemory { get; set; }

        internal StreamReader InputStream { get; }

        internal Dictionary<char, IInstruction> Instructions { get; set; }

        protected Interpreter(int memorySize = 30000)
        {
            WorkingMemory = new Memory(memorySize);
        }

        public Parser Parse()
        {
            return SourceParser = new Parser(this);
        }

        public void Execute(char input)
        {
            try
            {
                Instructions[input]?.Execute(this);
            }
            catch (Exception e)
            {
                
            }
        }

        public void Execute(Parser parser)
        {
            try
            {
                Instructions[(char)this.InputStream.Read()]?.Execute(parser);
            }
        }

        /// <summary>
        /// Prints a segment of the interpreters working memory.
        /// </summary>
        /// <param name="start">index to read from</param>
        /// <param name="limit">how many blocks of memory to read</param>
        public void PrintMemory(int start = 0, int limit = 8)
        {
            StringBuilder sb = new StringBuilder().Append("[");
            for (int i = 0; i < limit; i++)
            {
                sb.Append($"{WorkingMemory.Block[start + i]}, ");
            }
            sb.Remove(sb.Length-2, 2).Append("]");
            Console.WriteLine(sb);
            sb.Clear();
            sb = null;
        }
        
    }
}