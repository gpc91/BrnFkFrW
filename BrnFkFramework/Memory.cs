using System;

namespace BrnFkFramework
{
    public class Memory
    {
        /// <summary>
        /// Get the position of the memory pointer
        /// </summary>
        public int Pointer { get; internal set; } = 0;
        internal byte[] Block;
        /// <summary>
        /// Get the byte at the currently pointed to cell.
        /// </summary>
        public byte Cell
        {
            get { return Block[Pointer]; }
            internal set { Block[Pointer] = value; }
        }

        /// <summary>
        /// Specify the overflow behaviour when the memory pointer under/overflows.
        /// </summary>
        public OverflowBehaviour PointerOverflow { get; internal set; } = OverflowBehaviour.Default;

        /// <summary>
        /// The byte array size
        /// </summary>
        /// <param name="memorySize">(optional) size of array</param>
        public Memory(int memorySize = 30000)
        {
            Block = new byte[memorySize];
        }

        /// <summary>
        /// Increment and return the currently pointed to memory block
        /// </summary>
        /// <returns>value of cell that has been incremented</returns>
        public byte Add() => Block[Pointer]++;
        /// <summary>
        /// Decrement and return the currently pointed to memory block
        /// </summary>
        /// <returns>value of cell that has been decremented</returns>
        public byte Sub() => Block[Pointer]--;
        
        /// <summary>
        /// Advance the memory pointer forward, adhering to any overflow rules specified.
        /// </summary>
        public void Next()
        {
            switch (PointerOverflow)
            {
                case OverflowBehaviour.Block :
                    Pointer += Pointer < Block.Length - 1 ? 1 : 0;
                    return;
                case OverflowBehaviour.Wrap :
                    if (Pointer >= Block.Length-1)
                    {
                        Pointer = 0;
                    }
                    else
                    {
                        Pointer++;
                    }
                    return;
                default :
                    Pointer++;
                    return;
            }
        }
        /// <summary>
        /// Retreats the memory pointer back, adhering to any overflow rules specified.
        /// </summary>
        public void Prev()
        {
            switch (PointerOverflow)
            {
                case OverflowBehaviour.Block :
                    Pointer -= Pointer > 0 ? 1 : 0;
                    return;
                case OverflowBehaviour.Wrap :
                    if (Pointer <= 0)
                    {
                        Pointer = Block.Length - 1;
                    }
                    else
                    {
                        Pointer--;
                    }
                    return;
                default:
                    Pointer--;
                    return;
            }
        }

        /// <summary>
        /// Shift the memory pointer forward. <see cref="Next"/>
        /// </summary>
        public void Right() => Next();
        /// <summary>
        /// Shift the memory pointer backwards. <see cref="Prev"/>
        /// </summary>
        public void Left() => Prev();
        
    }

    /// <summary>
    /// Define the behaviour of any potential overflows when advancing or retreating the pointer.
    /// </summary>
    public enum OverflowBehaviour
    {
        /// <summary>
        /// No under/overflow protection will be used when advancing or retreating the pointer and will throw <see cref="IndexOutOfRangeException"/> if accessed  
        /// </summary>
        Default,
        /// <summary>
        /// When advancing or retreating the memory pointer the pointer will wrap
        /// </summary>
        Wrap,
        /// <summary>
        /// When advancing or retreating the memory pointer it will be blocked from advancing or retreating beyond the bounds of the memory array
        /// </summary>
        Block
    }
    
}