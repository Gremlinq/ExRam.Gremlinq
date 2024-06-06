namespace ExRam.Gremlinq.Tests.Entities;

public abstract class Element
{
    public object? Id { get; set; }

    public string? Label { get; set; }

    public string? PartitionKey { get; set; }

    public string? _Etag { get; set; }
}
