```

BenchmarkDotNet v0.14.0, Ubuntu 24.04.3 LTS (Noble Numbat)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.102
  [Host]   : .NET 10.0.2 (10.0.225.61305), X64 RyuJIT AVX2
  ShortRun : .NET 10.0.2 (10.0.225.61305), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3  

```
| Method            | Mean     | Error    | StdDev  | Gen0   | Allocated |
|------------------ |---------:|---------:|--------:|-------:|----------:|
| SimpleVertexQuery | 239.2 ns | 14.59 ns | 0.80 ns | 0.0186 |     312 B |
