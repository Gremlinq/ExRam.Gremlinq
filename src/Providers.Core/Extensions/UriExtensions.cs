namespace ExRam.Gremlinq.Providers.Core
{
    public static class UriExtensions
    {
        public static Uri EnsurePath(this Uri uri)
        {
            return uri is { AbsolutePath: null or "" or "/" }
                ? new UriBuilder(uri) { Path = "gremlin" }.Uri
                : uri;
        }
    }
}
