namespace ExRam.Gremlinq.Core
{
    public interface IStringGremlinQuery : IGremlinQuery<string>
    {
        IStringGremlinQuery Concat(params string[] strings);

        IStringGremlinQuery Concat(params Func<IStringGremlinQuery, IGremlinQueryBase<string>>[] stringTraversals);
    }
}
