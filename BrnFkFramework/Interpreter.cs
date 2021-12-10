using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace BrnFkFramework
{
    /// <summary>
    /// Holds and manipulates program state and executes program instructions provided to it.
    /// </summary>
    public abstract class Interpreter
    {

        public Parser SourceParser { get; set; }
        
        internal Memory WorkingMemory { get; set; }
        
        internal Dictionary<char, IInstruction> Instructions { get; set; }

        protected Interpreter(int memorySize = 30000)
        {
            WorkingMemory = new Memory(memorySize);
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
        
    }
}