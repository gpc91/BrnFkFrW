using BrnFkFramework.Brainfuck;

namespace BrnFkFramework
{
    public class BrainfuckRunner
    {
        private Interpreter _interpreter;

        public void Debug(string input)
        {
            _interpreter = new BrainfuckInterpreter();
        }
        public void ParseString(string input)
        {
            _interpreter = new BrainfuckInterpreter();
        }

        public void ParseFile()
        {
            
        }

        public void PrintMemory()
        {
            _interpreter.PrintMemory(); 
        }
    }
}