using static BenchmarkDotNet.Running.BenchmarkRunner;

namespace Benchmarks
{
    public class Program
    {
        static void Main()
        {
            Run<Benchmarks>();
        }
    }
}
