namespace ExRam.Gremlinq.Core
{
    public sealed class CyclicPathStep : Step
    {
        public static readonly CyclicPathStep Instance = new ();

        public CyclicPathStep() : base()
        {
        }
    }
}
