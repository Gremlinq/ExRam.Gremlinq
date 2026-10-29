using System.Collections.Immutable;

using BenchmarkDotNet.Attributes;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Models;
using ExRam.Gremlinq.Tests.Entities;

using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Benchmarks
{
    // The creation of a query of a requested type, as BuildAs does for every Cast, OfType or As: the
    // resolution of the type's continuation and its call.
    [MemoryDiagnoser]
    public class QueryFactoryBenchmarks
    {
        private IGremlinQueryEnvironment _environment = null!;

        [GlobalSetup]
        public void Setup()
        {
            _environment = g
                .ConfigureEnvironment(env => env
                    .UseModel(GraphModel.FromBaseTypes<Vertex, Edge>()))
                .AsAdmin()
                .Environment;
        }

        // A generic target type, whose continuation is resolved from its type arguments.
        [Benchmark]
        public IVertexGremlinQuery<RichVertex> Create_vertex_query() => GremlinQueryFactory.Create<IVertexGremlinQuery<RichVertex>>(_environment, Traversal.Empty, ImmutableDictionary<StepLabel, LabelProjections>.Empty, ImmutableDictionary<object, object?>.Empty);

        // A non-generic target type, which gets the object query's continuation.
        [Benchmark]
        public IGremlinQueryBase Create_base_query() => GremlinQueryFactory.Create<IGremlinQueryBase>(_environment, Traversal.Empty, ImmutableDictionary<StepLabel, LabelProjections>.Empty, ImmutableDictionary<object, object?>.Empty);
    }
}
