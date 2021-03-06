using System;
using System.Collections.Generic;
using System.Text;
using Serilog.Core;

namespace BrnFkFramework
{
    /// <summary>
    /// Holds and manipulates program state and executes program instructions provided to it.
    /// </summary>
    public abstract class Interpreter
    {

        /// <summary>
        /// The source, or root parser used by this interpreter.
        /// </summary>
        public Parser SourceParser { get; set; }
        
        /// <summary>
        /// The Memory class used by this interpreter
        /// </summary>
        internal Memory WorkingMemory { get; set; }

        internal String InputString { get; set; }

        internal Dictionary<char, IInstruction> Instructions { get; set; }

        /// <summary>
        /// Instantiate a new instance of an interpreter with its own parser and memory initialised.
        /// </summary>
        /// <param name="memorySize">Size of the memory array (30,000 default as per brainfuck standard)</param>
        protected Interpreter(int memorySize = 30000)
        {
            WorkingMemory = new Memory(memorySize);
            SourceParser = new Parser(this);
        }
        
        /// <summary>
        /// Execute the next instruction from a specified parser
        /// </summary>
        /// <param name="parser">The parser to get the next token from</param>
        public void Execute(Parser parser)
        {
            try
            {
                SourceParser.LastInstruction = SourceParser.Read();
                logger?.Verbose($"Attempting to execute instruction '{SourceParser.LastInstruction}'");
                Instructions[(char) SourceParser.LastInstruction]?.Execute(parser);
            }
            catch (Exception e)
            {
                logger?.Error($"Unrecognised character {SourceParser.LastInstruction}");
            }
        }

        /// <summary>
        /// Prints a segment of the interpreters working memory.
        /// </summary>
        /// <param name="start">index to read from</param>
        /// <param name="limit">how many blocks of memory to read</param>
        public void PrintMemory(int start = 0, int limit = 8, bool hex = false)
        {
            logger?.Debug("Printing Memory Block.");
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < limit; i++)
            {
                // ptr allows us to wrap the index so that we can always under/overflow correctly and print surrounding
                // the current memory block
                int ptr = (start + i) % WorkingMemory.Block.Length;
                ptr = ptr < 0 ? (WorkingMemory.Block.Length + ptr) : ptr;
                sb.Append($"[\x1b[32m{(hex == true ? String.Format("{0:X2}", WorkingMemory.Block[ptr]) : WorkingMemory.Block[ptr])}\x1b[0m]");
            }
            Console.WriteLine(sb);
            sb.Clear();
        }
        internal Logger logger;
        
        /// <summary>
        /// Specify a logger implementation to use. Serilog recommended.
        /// </summary>
        /// <param name="logger"></param>
        public void UseLogger(Logger logger)
        {
            this.logger = logger;
            this.logger.Information("Logger added.");
        }
        
    }
}