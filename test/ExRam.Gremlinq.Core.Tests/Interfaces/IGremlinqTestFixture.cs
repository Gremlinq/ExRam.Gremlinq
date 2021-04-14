namespace ExRam.Gremlinq.Core.Tests
{
    public interface IGremlinqTestFixture
    {
        IGremlinQuerySource GremlinQuerySource { get; }
    }
}
