using BenchmarkDotNet.Attributes;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Models;
using ExRam.Gremlinq.Tests.Entities;

using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Benchmarks
{
    [MemoryDiagnoser]
    public class QueryBuildingBenchmarks
    {
        private IGremlinQuerySource _g = null!;

        [GlobalSetup]
        public void Setup() => _g = g
            .ConfigureEnvironment(env => env
            .UseModel(GraphModel.FromBaseTypes<Vertex, Edge>()));

        [Benchmark]
        public object ComplexTraversalQuery() => _g
            .V<Person>()
            .Where(p => p.Age > 25)
            .Out<WorksFor>()
            .OfType<Company>()
            .Where(c => c.Name!.Value.StartsWith("Tech"));
    }
}
