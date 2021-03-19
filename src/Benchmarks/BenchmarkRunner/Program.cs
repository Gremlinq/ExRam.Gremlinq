using static BenchmarkDotNet.Running.BenchmarkRunner;

namespace Benchmarks
{
    public class Program
    {
        private static void Main()
        {
            Run<Benchmarks>();
        }
    }
}
