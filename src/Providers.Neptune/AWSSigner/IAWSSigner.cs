namespace ExRam.Gremlinq.Providers.Neptune
{
    public interface IAWSSigner
    {
        IAWSSigner ConfigureUri(Func<Uri, Uri> transformation);

        IAWSSigner ConfigureRegion(Func<string, string> transformation);

        IAWSSigner ConfigureCacheTime(Func<TimeSpan, TimeSpan> transformation);

        IAWSSigner WithAccessKeyId(string accessKeyId);

        IAWSSigner WithSecretAccessKey(string secretAccessKey);

        IReadOnlyDictionary<string, string> GetIAMHeaders(DateTimeOffset? time = null);
    }
}
