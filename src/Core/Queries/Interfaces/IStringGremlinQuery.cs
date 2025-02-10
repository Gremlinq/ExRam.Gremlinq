namespace ExRam.Gremlinq.Core
{
    public interface IStringGremlinQuery : IGremlinQuery<string>
    {
        IStringGremlinQuery Concat(params string[] strings);
    }
}
