using BenchmarkDotNet.Attributes;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Models;
using ExRam.Gremlinq.Tests.Entities;

using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Benchmarks
{
    // The lookups the serializer makes on the environment for every value: is a type native to the server?
    [MemoryDiagnoser]
    public class EnvironmentBenchmarks
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

        [Benchmark]
        public bool SupportsTypeNatively_int() => _environment.SupportsTypeNatively(typeof(int));

        [Benchmark]
        public bool SupportsTypeNatively_string() => _environment.SupportsTypeNatively(typeof(string));

        [Benchmark]
        public bool SupportsTypeNatively_miss() => _environment.SupportsTypeNatively(typeof(RichVertex));
    }
}
