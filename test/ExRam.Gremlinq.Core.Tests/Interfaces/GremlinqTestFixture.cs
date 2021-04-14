namespace ExRam.Gremlinq.Core.Tests
{
    public abstract class GremlinqTestFixture : IGremlinqTestFixture
    {
        protected GremlinqTestFixture(IGremlinQuerySource source)
        {
            GremlinQuerySource = source;
        }

        public IGremlinQuerySource GremlinQuerySource { get; }
    }
}
