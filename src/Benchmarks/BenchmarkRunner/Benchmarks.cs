using System;
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


        //[Benchmark]
        public async Task LargeGraphson3Path()
        {
            await _g
                .WithExecutor(GetJson("Large_Graphson3_Paths"))
                .V<Person>()
                .Cast<ExRam.Gremlinq.Core.GraphElements.Path[]>()
                .ToArrayAsync();
        }

        [Benchmark]
        public void Old()
        {
            var key = 5326256;
            var stringKey = string.Empty;

            do
            {
                stringKey = (char)('a' + key % 26) + stringKey;
                key /= 26;
            } while (key > 0);

            stringKey = "_" + stringKey;
        }

        [Benchmark]
        public void New()
        {
            var key = 5326256;
            var digits = key > 0
                ? (int)Math.Ceiling(Math.Log(key + 1, 26)) + 1
                : 2;

            string.Create(
                digits,
                (key, digits),
                (span, tuple) =>
                {
                    var (key, digits) = tuple;

                    span[0] = '_';

                    for (var i = digits - 1; i >= 1; i--)
                    {
                        span[i] = (char)('a' + key % 26);
                        key /= 26;
                    }
                });
        }

        private static string GetJson(string name)
        {
            return new StreamReader(File.OpenRead($"..\\..\\..\\..\\..\\..\\..\\..\\..\\..\\files\\GraphSon\\{name}.json")).ReadToEnd();
        }
    }
}
