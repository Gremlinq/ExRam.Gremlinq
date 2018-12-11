namespace ExRam.Gremlinq
{
    public abstract class LogicalStep : Step
    {
        protected LogicalStep(string name, IGremlinQuery[] traversals)
        {
            Name = name;
            Traversals = traversals;
        }

        public string Name { get; }
        public IGremlinQuery[] Traversals { get; }
    }

    public abstract class LogicalStep<TStep> : LogicalStep where TStep : LogicalStep
    {
        protected LogicalStep(string name, IGremlinQuery[] traversals) : base(name, traversals)
        {
        }
    }
}
