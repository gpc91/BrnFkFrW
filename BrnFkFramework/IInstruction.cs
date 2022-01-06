namespace BrnFkFramework
{
    internal interface IInstruction
    {
        public void Execute(Parser parser);
    }
}

// fetch decode execute