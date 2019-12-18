namespace ExRam.Gremlinq.Core
{
    public abstract class LogicalStep : Step
    {
        protected LogicalStep(string name, IGremlinQueryBase[] traversals)
        {
            Name = name;
            Traversals = traversals;
        }

        public string Name { get; }
        public IGremlinQueryBase[] Traversals { get; }
    }
}
