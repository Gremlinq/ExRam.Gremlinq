namespace ExRam.Gremlinq.Core.Tests
{
    public interface IIntegrationTestFixture
    {
        IGremlinQuerySource GremlinQuerySource { get; }
    }
}
