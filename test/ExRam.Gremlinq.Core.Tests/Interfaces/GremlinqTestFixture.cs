namespace ExRam.Gremlinq.Core.Tests
{
    public abstract class GremlinqTestFixture
    {
        protected GremlinqTestFixture(IGremlinQuerySource source)
        {
            GremlinQuerySource = source;
        }

        public IGremlinQuerySource GremlinQuerySource { get; }
    }
}
