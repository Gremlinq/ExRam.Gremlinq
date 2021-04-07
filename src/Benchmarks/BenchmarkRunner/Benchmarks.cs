using System.IO;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Tests;
using ExRam.Gremlinq.Tests.Entities;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace Benchmarks
{
    [MemoryDiagnoser]
    public class Benchmarks
    {
        private readonly IGremlinQuerySource _g;

        public Benchmarks()
        {
            _g = g
                .ConfigureEnvironment(env => env.UseModel(GraphModel.FromBaseTypes<Vertex, Edge>(lookup => lookup
                    .IncludeAssembliesOfBaseTypes())));
        }


        [Benchmark]
        public async Task LargeGraphson3Path()
        {
            await _g
                .WithExecutor(GetJson("Large_Graphson3_Paths"))
                .V<Person>()
                .Cast<ExRam.Gremlinq.Core.GraphElements.Path[]>()
                .ToArrayAsync();
        }

        private static string GetJson(string name)
        {
            return new StreamReader(File.OpenRead($"..\\..\\..\\..\\..\\..\\..\\..\\..\\..\\files\\GraphSon\\{name}.json")).ReadToEnd();
        }
    }
}
