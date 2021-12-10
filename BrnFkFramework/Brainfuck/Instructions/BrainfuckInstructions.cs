using System;

namespace BrnFkFramework.Brainfuck.Instructions
{
    public class BrainfuckAdd : IInstruction
    {
        public void Execute(Interpreter interpreter)
        {
            interpreter.WorkingMemory.Add();
        }
    }
    
    public class BrainfuckSub : IInstruction
    {
        public void Execute(Interpreter interpreter)
        {
            interpreter.WorkingMemory.Sub();
        }
    }
    public class BrainfuckPrint : IInstruction
    {
        public void Execute(Interpreter interpreter)
        {
            byte cell = interpreter.WorkingMemory.Cell;
            switch ((char) interpreter.SourceParser.NextInstruction)
            {
                case '!' :
                    Console.WriteLine($"{cell}");
                    interpreter.SourceParser.Reader.BaseStream.Position++;
                    return;
                case '@' :
                    Console.WriteLine($"{cell} ({(int)cell})");
                    interpreter.SourceParser.Reader.BaseStream.Position++;
                    return;
                case '£' :
                    Console.WriteLine($"{cell} ({(int)cell})");
                    interpreter.SourceParser.Reader.BaseStream.Position++;
                    return;
                default :
                    Console.Write($"{(char)cell}");
                    return;
            }
        }   
    }
    public class BrainfuckRead : IInstruction
    {
        public void Execute(Interpreter interpreter)
        {
            // TODO: make sure what we input is correct...
            interpreter.WorkingMemory.Cell = (byte) Console.Read(); // this isn't compeltely working
        }
    }

    public class BrainfuckRight : IInstruction
    {
        public void Execute(Interpreter interpreter)
        {
            interpreter.WorkingMemory.Right();
        }
    }
    public class BrainfuckLeft : IInstruction
    {
        public void Execute(Interpreter interpreter)
        {
            interpreter.WorkingMemory.Left();
        }
    }
}