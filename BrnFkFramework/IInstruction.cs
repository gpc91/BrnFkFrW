namespace BrnFkFramework
{
    internal interface IInstruction
    {
        public void Execute(Interpreter interpreter);
        public void Execute(Parser parser);
    }
}

// fetch decode execute