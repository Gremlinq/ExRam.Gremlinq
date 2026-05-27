using ExRam.Gremlinq.Tests.Infrastructure;

[assembly: CollectionBehavior(
    CollectionBehavior.CollectionPerAssembly,
    DisableTestParallelization = true,
    MaxParallelThreads = 4)]

[assembly: TestFramework(typeof(GremlinqTestFramework))]
