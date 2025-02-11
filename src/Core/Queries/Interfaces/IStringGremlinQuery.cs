namespace ExRam.Gremlinq.Core
{
    public interface IStringGremlinQuery<TString> : IGremlinQueryBaseRec<TString, IStringGremlinQuery<TString>>
    {
        IStringGremlinQuery<TString> Concat(params string[] strings);

        IStringGremlinQuery<TString> Concat(params Func<IStringGremlinQuery<TString>, IGremlinQueryBase<TString>>[] stringTraversals);

        IStringGremlinQuery<TString> Substring(int startIndex);

        IStringGremlinQuery<TString> Substring(int startIndex, int length);

        IStringGremlinQuery<TString> Substring(Range range);
    }
}
