using System;

namespace ExRam.Gremlinq.Core.Tests
{
    public interface IGremlinqTestFixture
    {
        IGremlinqTestFixture Configure(Func<IGremlinQuerySource, IGremlinQuerySource> transformation);

        IGremlinQuerySource GremlinQuerySource { get; }
    }
}
