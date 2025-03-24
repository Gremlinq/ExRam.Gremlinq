namespace ExRam.Gremlinq.Core
{
    public interface IStringGremlinQuery<TString> : IGremlinQueryBaseRec<TString, IStringGremlinQuery<TString>>
    {
        IStringGremlinQuery<TString> Concat(params string[] strings);

        IStringGremlinQuery<TString> Concat(params ReadOnlySpan<string> strings);

        IStringGremlinQuery<TString> Concat(params Func<IStringGremlinQuery<TString>, IGremlinQueryBase<TString>>[] stringTraversals);

        IStringGremlinQuery<TString> Concat(params ReadOnlySpan<Func<IStringGremlinQuery<TString>, IGremlinQueryBase<TString>>> stringTraversals);

        IGremlinQuery<int> Length();

        IStringGremlinQuery<TString> Replace(string oldValue, string newValue);

        IStringGremlinQuery<TString> Reverse();

        IStringGremlinQuery<TString> Substring(int startIndex);

        IStringGremlinQuery<TString> Substring(int startIndex, int length);

        IStringGremlinQuery<TString> Substring(Range range);

        IStringGremlinQuery<TString> ToLower();

        IStringGremlinQuery<TString> ToUpper();

        IStringGremlinQuery<TString> Trim();

        IStringGremlinQuery<TString> TrimStart();

        IStringGremlinQuery<TString> TrimEnd();
    }
}
