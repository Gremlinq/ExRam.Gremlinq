namespace ExRam.Gremlinq.Core
{
    public interface IStringGremlinQuery : IGremlinQuery<string>
    {
        IStringGremlinQuery Concat(params string[] strings);

        IStringGremlinQuery Concat(params Func<IStringGremlinQuery, IGremlinQueryBase<string>>[] stringTraversals);

        IStringGremlinQuery Substring(int startIndex);

        IStringGremlinQuery Substring(int startIndex, int length);

        IStringGremlinQuery Substring(Range range);
    }
}
