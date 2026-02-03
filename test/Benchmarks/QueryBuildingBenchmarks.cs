using BenchmarkDotNet.Attributes;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.GraphElements;
using ExRam.Gremlinq.Core.Models;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Benchmarks;

// Model classes for benchmarks
public abstract class Element
{
    public object? Id { get; set; }
    public string? Label { get; set; }
}

public abstract class Vertex : Element
{
}

public abstract class Edge : Element
{
}

public class Person : Vertex
{
    public string? Name { get; set; }
    public int Age { get; set; }
}

public class Company : Vertex
{
    public string? Name { get; set; }
    public DateTime FoundingDate { get; set; }
}

public class WorksAt : Edge
{
    public string? Role { get; set; }
    public DateTime From { get; set; }
}

[MemoryDiagnoser]
public class QueryBuildingBenchmarks
{
    private IGremlinQuerySource _g = null!;

    [GlobalSetup]
    public void Setup()
    {
        _g = g.ConfigureEnvironment(env => env
            .UseModel(GraphModel.FromBaseTypes<Vertex, Edge>()));
    }

    [Benchmark]
    public object SimpleVertexQuery()
    {
        return _g
            .V<Person>();
    }

    [Benchmark]
    public object FilteredVertexQuery()
    {
        return _g
            .V<Person>()
            .Where(p => p.Age > 25);
    }

    [Benchmark]
    public object ComplexTraversalQuery()
    {
        return _g
            .V<Person>()
            .Where(p => p.Age > 25)
            .Out<WorksAt>()
            .OfType<Company>()
            .Where(c => c.Name!.StartsWith("Tech"));
    }

    [Benchmark]
    public object ProjectionQuery()
    {
        return _g
            .V<Person>()
            .Values(p => p.Name!);
    }

    [Benchmark]
    public object CountQuery()
    {
        return _g
            .V<Person>()
            .Count();
    }
}
