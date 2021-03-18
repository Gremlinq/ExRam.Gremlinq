using BenchmarkDotNet.Attributes;

namespace Benchmarks
{
    [MemoryDiagnoser]
    public class Benchmarks
    {
        [Benchmark]
        public void Benchmark1()
        {
        }
    }
}
