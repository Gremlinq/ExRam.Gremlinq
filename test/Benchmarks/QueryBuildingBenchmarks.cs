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
        private int _age = 25;
        private IGremlinQuerySource _g = null!;

        [GlobalSetup]
        public void Setup() => _g = g
            .ConfigureEnvironment(env => env
            .UseModel(GraphModel.FromBaseTypes<Vertex, Edge>()));

        [Benchmark]
        public object ComplexTraversalQuery() => _g
            .V<RichVertex>()
            .Where(p => p.Age > 25)
            .Out<RichEdge>()
            .OfType<SiblingVertex>()
            .Where(c => c.Name!.Value.StartsWith("Tech"));

        [Benchmark]
        public object Where_with_closure_constant()
        {
            var age = _age;

            return _g
                .V<RichVertex>()
                .Where(x => x.Age > age);
        }

        // The constant is an expression that GetValue cannot read directly.
        [Benchmark]
        public object Where_with_arithmetic_constant() => _g
            .V<RichVertex>()
            .Where(x => x.Age > _age + 1);

        [Benchmark]
        public object Project() => _g
            .V<RichVertex>()
            .Project(__ => __.ToTuple()
                .By(__ => __.Id())
                .By(__ => __.Label())
                .By(__ => __.Values(x => x.Age)));

        [Benchmark]
        public object Choose() => _g
            .V()
            .Id()
            .Choose(
                x => x == (object)42,
                _ => _.Constant(true),
                _ => _.Constant(false));

        [Benchmark]
        public object AddV_object() => _g
            .AddV(new RichVertex { Age = 30, RegistrationDate = DateTimeOffset.UnixEpoch });
    }
}
