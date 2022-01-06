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

        public OverflowBehaviour PointerOverflow { get; internal set; } = OverflowBehaviour.Default;

        public Memory(int memorySize = 30000)
        {
            Block = new byte[memorySize];
        }

        public byte Add() => Block[Pointer]++;
        public byte Sub() => Block[Pointer]--;
        
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

        public void Right() => Next();
        public void Left() => Prev();
        
    }

    /// <summary>
    /// Define the behaviour of any potential overflows
    /// Default: Throw an out of bounds exception
    /// Wrap: wrap around from Length-1 -> 0 or 0 -> Length-1
    /// Block: block the pointer moving beyond the bounds of the array.
    /// </summary>
    public enum OverflowBehaviour
    {
        Default,
        Wrap,
        Block
    }
    
}