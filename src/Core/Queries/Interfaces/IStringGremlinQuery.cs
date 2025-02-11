namespace ExRam.Gremlinq.Core
{
    public interface IStringGremlinQuery<TString> : IGremlinQuery<TString>
    {
        IStringGremlinQuery<TString> Concat(params string[] strings);

        IStringGremlinQuery<TString> Concat(params Func<IStringGremlinQuery<TString>, IGremlinQueryBase<TString>>[] stringTraversals);

        IStringGremlinQuery<TString> Substring(int startIndex);

        IStringGremlinQuery<TString> Substring(int startIndex, int length);

        IStringGremlinQuery<TString> Substring(Range range);
    }
}
