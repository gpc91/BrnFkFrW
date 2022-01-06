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

        internal String InputString { get; set; }

        internal Dictionary<char, IInstruction> Instructions { get; set; }

        protected Interpreter(int memorySize = 30000)
        {
            WorkingMemory = new Memory(memorySize);
        }

        public Parser Parse()
        {
            SourceParser = new Parser(this);
            return SourceParser;
            
        }

        public void Execute(Parser parser)
        {
            try
            {
                char instruction = (char) SourceParser.Read();
                //Console.WriteLine(instruction);
                //Console.WriteLine($"attempting execution of instruction {instruction}");
                Instructions[instruction]?.Execute(parser);
            }
            catch (Exception e)
            {
                
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
        }
        
    }
}