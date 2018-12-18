namespace ExRam.Gremlinq.Core
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
}
