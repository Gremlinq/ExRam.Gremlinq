# ExRam.Gremlinq Benchmarks

This project contains performance benchmarks for ExRam.Gremlinq using BenchmarkDotNet.

## Running Benchmarks Locally

To run all benchmarks:

```bash
cd test/Benchmarks
dotnet run -c Release
```

To run specific benchmarks:

```bash
cd test/Benchmarks
dotnet run -c Release -- --filter '*SimpleVertexQuery'
```

To export results to JSON:

```bash
cd test/Benchmarks
dotnet run -c Release -- --exporters json --join
```

## Continuous Benchmarking

Benchmarks are automatically run on:
- Push to the `13.x` branch
- Pull requests to the `13.x` branch
- Manual workflow dispatch

Results are stored in the `gh-pages` branch and viewable at:
https://gremlinq.github.io/ExRam.Gremlinq/benchmarks/

## Performance Alerts

The CI workflow will:
- Comment on PRs if performance regresses by more than 20%
- Store historical benchmark data for trend analysis
- Generate comparison charts between commits

## More Information

- [BenchmarkDotNet Documentation](https://benchmarkdotnet.org/)
- [Continuous Benchmarks on a Budget](https://blog.martincostello.com/continuous-benchmarks-on-a-budget/)
- [github-action-benchmark](https://github.com/benchmark-action/github-action-benchmark)
