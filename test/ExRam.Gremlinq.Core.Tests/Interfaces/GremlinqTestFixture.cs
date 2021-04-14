using System;

namespace ExRam.Gremlinq.Core.Tests
{
    public abstract class GremlinqTestFixture : IGremlinqTestFixture
    {
        private sealed class GremlinqTestFixtureImpl : GremlinqTestFixture
        {
            public GremlinqTestFixtureImpl(IGremlinQuerySource source) : base(source)
            {
            }
        }

        protected GremlinqTestFixture(IGremlinQuerySource source)
        {
            GremlinQuerySource = source;
        }

        public IGremlinqTestFixture Configure(Func<IGremlinQuerySource, IGremlinQuerySource> transformation) => new GremlinqTestFixtureImpl(transformation(GremlinQuerySource));

        public IGremlinQuerySource GremlinQuerySource { get; }
    }
}
