namespace ExRam.Gremlinq.Providers.Neptune
{
    public interface ISigV4AWSSigner : IAWSSigner
    {
        ISigV4AWSSigner ConfigureUri(Func<Uri, Uri> transformation);

        ISigV4AWSSigner ConfigureRegion(Func<string, string> transformation);

        ISigV4AWSSigner ConfigureCacheTime(Func<TimeSpan, TimeSpan> transformation);

        ISigV4AWSSigner WithAccessKeyId(string accessKeyId);

        ISigV4AWSSigner WithSecretAccessKey(string secretAccessKey);
    }
}
