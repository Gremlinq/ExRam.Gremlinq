﻿using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Models;
using ExRam.Gremlinq.Providers.Core;
using ExRam.Gremlinq.Tests.Entities;

using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Tests.Infrastructure
{
    public class GremlinqFixture : IAsyncLifetime
    {
        private IGremlinQuerySource? _g;

        protected virtual IGremlinQuerySource TransformQuerySource(IGremlinQuerySource g) => g;

        public virtual async Task InitializeAsync()
        {
            _g = TransformQuerySource(g
                .ConfigureEnvironment(env => env
                    .UseModel(GraphModel.FromBaseTypes<Vertex, Edge>())
                    .AddGraphSonBinarySupport()));
        }

        public virtual async Task DisposeAsync()
        {
        }

        public virtual IGremlinQuerySource GetQuerySource() => _g ?? throw new InvalidOperationException();
    }
}
