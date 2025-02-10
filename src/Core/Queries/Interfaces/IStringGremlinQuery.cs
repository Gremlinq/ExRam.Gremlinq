namespace ExRam.Gremlinq.Core
{
    public interface IStringGremlinQuery : IGremlinQuery<string>
    {
        IStringGremlinQuery Concat(params string[] strings);

        IStringGremlinQuery Concat(params Func<IStringGremlinQuery, IGremlinQueryBase<string>>[] stringTraversals);

        IStringGremlinQuery Substring(Index startIndex);

        IStringGremlinQuery Substring(Index startIndex, int length);

        IStringGremlinQuery Substring(Range range);
    }
}
