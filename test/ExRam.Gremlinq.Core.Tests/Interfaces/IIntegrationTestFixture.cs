using System;

namespace ExRam.Gremlinq.Core.Tests
{
    public abstract class IntegrationTestFixture : IIntegrationTestFixture
    {
        private sealed class IntegrationTestFixtureImpl : IntegrationTestFixture
        {
            public IntegrationTestFixtureImpl(IGremlinQuerySource source) : base(source)
            {
            }
        }

        protected IntegrationTestFixture(IGremlinQuerySource source)
        {
            GremlinQuerySource = source;
        }

        public IIntegrationTestFixture Configure(Func<IGremlinQuerySource, IGremlinQuerySource> transformation) => new IntegrationTestFixtureImpl(transformation(GremlinQuerySource));

        public IGremlinQuerySource GremlinQuerySource { get; }
    }

    public interface IIntegrationTestFixture
    {
        IIntegrationTestFixture Configure(Func<IGremlinQuerySource, IGremlinQuerySource> transformation);

        IGremlinQuerySource GremlinQuerySource { get; }
    }
}
