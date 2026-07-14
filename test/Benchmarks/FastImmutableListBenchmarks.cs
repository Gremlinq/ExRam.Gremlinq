using BenchmarkDotNet.Attributes;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Steps;

namespace ExRam.Gremlinq.Benchmarks
{
    [MemoryDiagnoser]
    public class FastImmutableListBenchmarks
    {
        private FastImmutableList<Step> _steps;
        
        private static readonly Step[] Steps4 = [CountStep.Local, CountStep.Local, CountStep.Local, CountStep.Local];
        private static readonly Step[] Steps8 = [CountStep.Local, CountStep.Local, CountStep.Local, CountStep.Local, CountStep.Local, CountStep.Local, CountStep.Local, CountStep.Local];
        private static readonly Step[] Steps16 = [CountStep.Local, CountStep.Local, CountStep.Local, CountStep.Local, CountStep.Local, CountStep.Local, CountStep.Local, CountStep.Local, CountStep.Local, CountStep.Local, CountStep.Local, CountStep.Local, CountStep.Local, CountStep.Local, CountStep.Local, CountStep.Local];
        private static readonly Step[] Steps32 = [CountStep.Local, CountStep.Local, CountStep.Local, CountStep.Local, CountStep.Local, CountStep.Local, CountStep.Local, CountStep.Local, CountStep.Local, CountStep.Local, CountStep.Local, CountStep.Local, CountStep.Local, CountStep.Local, CountStep.Local, CountStep.Local, CountStep.Local, CountStep.Local, CountStep.Local, CountStep.Local, CountStep.Local, CountStep.Local, CountStep.Local, CountStep.Local, CountStep.Local, CountStep.Local, CountStep.Local, CountStep.Local, CountStep.Local, CountStep.Local, CountStep.Local, CountStep.Local];

        [GlobalSetup]
        public void Setup() => _steps = FastImmutableList<Step>.Empty
            .Push(CountStep.Local, CountStep.Local, CountStep.Local, CountStep.Local, CountStep.Local, CountStep.Local, CountStep.Local, CountStep.Local);

        [Benchmark]
        public object Push_1() => _steps
            .Push(CountStep.Local);

        [Benchmark]
        public object Push_4() => _steps
            .Push(Steps4);

        [Benchmark]
        public object Push_8() => _steps
            .Push(Steps8);
        
        [Benchmark]
        public object Push_16() => _steps
            .Push(Steps16);

        [Benchmark]
        public object Push_32() => _steps
            .Push(Steps32);

        [Benchmark]
        public object Push_4_multiple() => _steps   //8, 16
            .Push(Steps4)                           //12, 16
            .Push(Steps4)                           //16, 16
            .Push(Steps4)                           //20, 32
            .Push(Steps4)                           //24, 32
            .Push(Steps4)                           //28, 32
            .Push(Steps4)                           //32, 32
            .Push(Steps4)                           //36, 64
            .Push(Steps4)                           //40, 64
            .Push(Steps4)                           //44, 64
            .Push(Steps4)                           //48, 64
            .Push(Steps4)                           //52, 64
            .Push(Steps4)                           //56, 64
            .Push(Steps4)                           //60, 64
            .Push(Steps4)                           //64, 64
            .Push(Steps4)                           //68, 128
            .Push(Steps4);                          //72, 128 => 16 + 32 + 64 + 128 = 240
    }
}
