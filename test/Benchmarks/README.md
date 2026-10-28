# ExRam.Gremlinq Benchmarks

This project contains performance benchmarks for ExRam.Gremlinq using BenchmarkDotNet.

## What is measured

- `QueryBuildingBenchmarks`: building a query with the fluent API, without serializing it.
- `SerializationBenchmarks`: serializing a built query to `Bytecode`, `RequestMessage` and Groovy.
  Every benchmark but the last serializes the same query instance again and again.
- `TransformerBenchmarks`: one `TryTransform` call on a transformer whose converters exist
  already, so the lookup of the converter chain and its call, not its creation.
- `FastImmutableListBenchmarks`: pushing to the list that holds a traversal's steps.

## Running Benchmarks Locally

The project targets the frameworks of `test/Directory.Build.props`, so `dotnet run` needs one.

To run all benchmarks:

```bash
cd test/Benchmarks
dotnet run -c Release -f net10.0
```

To run specific benchmarks:

```bash
cd test/Benchmarks
dotnet run -c Release -f net10.0 -- --filter '*SerializationBenchmarks*'
```

To export results to JSON:

```bash
cd test/Benchmarks
dotnet run -c Release -f net10.0 -- --exporters json --join
```

## Continuous Benchmarking

`.github/workflows/benchmark.yml` runs the benchmarks on manual workflow dispatch.

Results are stored in the `gh-pages` branch and viewable at:
https://gremlinq.github.io/ExRam.Gremlinq/benchmarks/

## More Information

- [BenchmarkDotNet Documentation](https://benchmarkdotnet.org/)
- [Continuous Benchmarks on a Budget](https://blog.martincostello.com/continuous-benchmarks-on-a-budget/)
- [github-action-benchmark](https://github.com/benchmark-action/github-action-benchmark)
