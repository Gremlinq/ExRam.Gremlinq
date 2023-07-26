using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Models;
using ExRam.Gremlinq.Tests.Entities;

using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Tests.Fixtures
{
    public abstract class GremlinqFixture : IDisposable
    {
        private sealed class EmptyGremlinqTestFixture : GremlinqFixture
        {
            public EmptyGremlinqTestFixture() : base(g.ConfigureEnvironment(_ => _))
            {
            }
        }

        public static readonly GremlinqFixture Empty = new EmptyGremlinqTestFixture();

        protected GremlinqFixture(IGremlinQuerySource source)
        {
            GremlinQuerySource = source;
        }

        public IGremlinQuerySource GremlinQuerySource { get; }

        public void Dispose()
        {
            GremlinQuerySource
                .ConfigureEnvironment(_ => _
                    .UseModel(GraphModel.FromBaseTypes<Vertex, Edge>(lookup => lookup
                        .IncludeAssembliesOfBaseTypes())))
                .V()
                .Drop()
                .ToArrayAsync()
                .AsTask()
                .Wait();
        }
    }
}
