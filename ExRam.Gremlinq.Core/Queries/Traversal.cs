namespace ExRam.Gremlinq.Core
{
    public readonly struct Traversal
    {
        public Traversal(Step[] steps)
        {
            Steps = steps;
        }

        public Step[] Steps { get; }

        public static implicit operator Traversal(Step[] steps)
        {
            return new Traversal(steps);
        }

        public static implicit operator Traversal(Step step)
        {
            return new Traversal(new Step[] { step });
        }
    }
}
