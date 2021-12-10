namespace BrnFkFramework
{
    internal interface IInstruction
    {
        public void Execute(Interpreter interpreter);
    }
}

// fetch decode execute