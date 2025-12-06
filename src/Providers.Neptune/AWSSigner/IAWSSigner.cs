namespace ExRam.Gremlinq.Providers.Neptune
{
    public interface IAWSSigner
    {
        IReadOnlyDictionary<string, string> GetIAMHeaders(DateTimeOffset? time = null);
    }
}
