namespace ExRam.Gremlinq.Core.Steps
{
    public sealed class CyclicPathStep : Step
    {
        public static readonly CyclicPathStep Instance = new ();

        public CyclicPathStep() : base()
        {
        }
    }
}
