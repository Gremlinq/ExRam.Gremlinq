#nullable enable
using System;
using System.Collections.Generic;
using System.Dynamic;
using BenchmarkDotNet.Attributes;

namespace Benchmarks
{
    [MemoryDiagnoser]
    public class DictionaryAddBenchmarks
    {
        private readonly IDictionary<string, object?> _expandoObject = new ExpandoObject();
        private readonly IDictionary<string, object?> _dict = new Dictionary<string, object?>();

        [Benchmark]
        public void ExpandoObject()
        {
            var key = (DateTime.Now.Ticks % 100).ToString();

            _expandoObject[key] = key;
        }

        [Benchmark]
        public void Dictionary()
        {
            var key = (DateTime.Now.Ticks % 100).ToString();

            _dict[key] = key;
        }
    }
}
