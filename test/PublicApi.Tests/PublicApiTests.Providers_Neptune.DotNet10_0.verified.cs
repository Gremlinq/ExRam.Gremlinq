namespace ExRam.Gremlinq.Providers.Neptune
{
    public static class AWSSigner
    {
        public static readonly ExRam.Gremlinq.Providers.Neptune.IDisabledAWSSigner Disabled;
        public static readonly ExRam.Gremlinq.Providers.Neptune.ISigV4AWSSigner EmptySigV4;
        public static System.Net.Http.Headers.HttpHeaders Sign(this ExRam.Gremlinq.Providers.Neptune.IAWSSigner signer, System.Net.Http.Headers.HttpHeaders headers, System.DateTimeOffset? time = default) { }
        public static System.Net.Http.HttpRequestMessage Sign(this ExRam.Gremlinq.Providers.Neptune.IAWSSigner signer, System.Net.Http.HttpRequestMessage request, System.DateTimeOffset? time = default) { }
        public static ExRam.Gremlinq.Providers.Neptune.ISigV4AWSSigner WithCacheTime(this ExRam.Gremlinq.Providers.Neptune.ISigV4AWSSigner signer, System.TimeSpan cacheTime) { }
        public static ExRam.Gremlinq.Providers.Neptune.ISigV4AWSSigner WithRegion(this ExRam.Gremlinq.Providers.Neptune.ISigV4AWSSigner signer, string region) { }
        public static ExRam.Gremlinq.Providers.Neptune.ISigV4AWSSigner WithUri(this ExRam.Gremlinq.Providers.Neptune.ISigV4AWSSigner signer, System.Uri uri) { }
    }
    public static class ConfigurableGremlinQuerySourceExtensions
    {
        public static ExRam.Gremlinq.Core.IGremlinQuerySource UseNeptune<TVertexBase, TEdgeBase>(this ExRam.Gremlinq.Core.IGremlinQuerySource source, System.Func<ExRam.Gremlinq.Providers.Neptune.INeptuneConfigurator, ExRam.Gremlinq.Core.IGremlinQuerySourceTransformation> configuratorTransformation) { }
    }
    public static class GremlinQuerySourceExtensions
    {
        public static ExRam.Gremlinq.Core.IGremlinQuerySource UseDFE(this ExRam.Gremlinq.Core.IGremlinQuerySource source, bool enabled = true) { }
    }
    public interface IAWSSigner
    {
        System.Collections.Generic.IReadOnlyDictionary<string, string> GetIAMHeaders(System.DateTimeOffset? time = default);
    }
    public interface IDisabledAWSSigner : ExRam.Gremlinq.Providers.Neptune.IAWSSigner
    {
        ExRam.Gremlinq.Providers.Neptune.ISigV4AWSSigner UseSigV4();
    }
    public interface INeptuneConfigurator : ExRam.Gremlinq.Core.IGremlinQuerySourceTransformation, ExRam.Gremlinq.Core.IGremlinqConfigurator<ExRam.Gremlinq.Providers.Neptune.INeptuneConfigurator>, ExRam.Gremlinq.Providers.Core.IProviderConfigurator<ExRam.Gremlinq.Providers.Neptune.INeptuneConfigurator, ExRam.Gremlinq.Providers.Core.IPoolGremlinqClientFactory<ExRam.Gremlinq.Providers.Core.IWebSocketGremlinqClientFactory>> { }
    public interface ISigV4AWSSigner : ExRam.Gremlinq.Providers.Neptune.IAWSSigner
    {
        ExRam.Gremlinq.Providers.Neptune.ISigV4AWSSigner ConfigureCacheTime(System.Func<System.TimeSpan, System.TimeSpan> transformation);
        ExRam.Gremlinq.Providers.Neptune.ISigV4AWSSigner ConfigureRegion(System.Func<string, string> transformation);
        ExRam.Gremlinq.Providers.Neptune.ISigV4AWSSigner ConfigureUri(System.Func<System.Uri, System.Uri> transformation);
        ExRam.Gremlinq.Providers.Neptune.ISigV4AWSSigner WithAccessKeyId(string accessKeyId);
        ExRam.Gremlinq.Providers.Neptune.ISigV4AWSSigner WithSecretAccessKey(string secretAccessKey);
    }
    public static class NeptuneConfiguratorExtensions
    {
        public static ExRam.Gremlinq.Providers.Neptune.INeptuneConfigurator UseElasticSearch(this ExRam.Gremlinq.Providers.Neptune.INeptuneConfigurator configurator, System.Uri elasticSearchEndPoint, ExRam.Gremlinq.Providers.Neptune.NeptuneElasticSearchIndexConfiguration indexConfiguration = 0) { }
        public static TConfigurator UseIAMAuthentication<TConfigurator>(this TConfigurator configurator, ExRam.Gremlinq.Providers.Neptune.IAWSSigner signer)
            where TConfigurator : ExRam.Gremlinq.Providers.Core.IProviderConfigurator<TConfigurator, ExRam.Gremlinq.Providers.Core.IPoolGremlinqClientFactory<ExRam.Gremlinq.Providers.Core.IWebSocketGremlinqClientFactory>> { }
        public static TConfigurator UseIAMAuthentication<TConfigurator>(this TConfigurator configurator, System.Func<ExRam.Gremlinq.Providers.Neptune.IDisabledAWSSigner, ExRam.Gremlinq.Providers.Neptune.IAWSSigner> builderTransformation)
            where TConfigurator : ExRam.Gremlinq.Providers.Core.IProviderConfigurator<TConfigurator, ExRam.Gremlinq.Providers.Core.IPoolGremlinqClientFactory<ExRam.Gremlinq.Providers.Core.IWebSocketGremlinqClientFactory>> { }
    }
    public enum NeptuneElasticSearchIndexConfiguration
    {
        Standard = 0,
        LowercaseKeyword = 1,
    }
    public readonly struct NeptuneErrorCode : System.IEquatable<ExRam.Gremlinq.Providers.Neptune.NeptuneErrorCode>
    {
        public static readonly ExRam.Gremlinq.Providers.Neptune.NeptuneErrorCode AccessDeniedException;
        public static readonly ExRam.Gremlinq.Providers.Neptune.NeptuneErrorCode BadRequestException;
        public static readonly ExRam.Gremlinq.Providers.Neptune.NeptuneErrorCode CancelledByUserException;
        public static readonly ExRam.Gremlinq.Providers.Neptune.NeptuneErrorCode ConcurrentModificationException;
        public static readonly ExRam.Gremlinq.Providers.Neptune.NeptuneErrorCode ConstraintViolationException;
        public static readonly ExRam.Gremlinq.Providers.Neptune.NeptuneErrorCode FailureByQueryException;
        public static readonly ExRam.Gremlinq.Providers.Neptune.NeptuneErrorCode InternalFailureException;
        public static readonly ExRam.Gremlinq.Providers.Neptune.NeptuneErrorCode InvalidNumericDataException;
        public static readonly ExRam.Gremlinq.Providers.Neptune.NeptuneErrorCode InvalidParameterException;
        public static readonly ExRam.Gremlinq.Providers.Neptune.NeptuneErrorCode MalformedQueryException;
        public static readonly ExRam.Gremlinq.Providers.Neptune.NeptuneErrorCode MemoryLimitExceededException;
        public static readonly ExRam.Gremlinq.Providers.Neptune.NeptuneErrorCode MethodNotAllowedException;
        public static readonly ExRam.Gremlinq.Providers.Neptune.NeptuneErrorCode MissingParameterException;
        public static readonly ExRam.Gremlinq.Providers.Neptune.NeptuneErrorCode QueryLimitExceededException;
        public static readonly ExRam.Gremlinq.Providers.Neptune.NeptuneErrorCode QueryLimitException;
        public static readonly ExRam.Gremlinq.Providers.Neptune.NeptuneErrorCode QueryTooLargeException;
        public static readonly ExRam.Gremlinq.Providers.Neptune.NeptuneErrorCode ReadOnlyViolationException;
        public static readonly ExRam.Gremlinq.Providers.Neptune.NeptuneErrorCode ThrottlingException;
        public static readonly ExRam.Gremlinq.Providers.Neptune.NeptuneErrorCode TimeLimitExceededException;
        public static readonly ExRam.Gremlinq.Providers.Neptune.NeptuneErrorCode TooManyRequestsException;
        public static readonly ExRam.Gremlinq.Providers.Neptune.NeptuneErrorCode UnsupportedOperationException;
        public string Code { get; }
        public bool Equals(ExRam.Gremlinq.Providers.Neptune.NeptuneErrorCode other) { }
        public override bool Equals(object? obj) { }
        public override int GetHashCode() { }
        public static ExRam.Gremlinq.Providers.Neptune.NeptuneErrorCode From(string code) { }
        public static bool operator !=(ExRam.Gremlinq.Providers.Neptune.NeptuneErrorCode left, ExRam.Gremlinq.Providers.Neptune.NeptuneErrorCode right) { }
        public static bool operator ==(ExRam.Gremlinq.Providers.Neptune.NeptuneErrorCode left, ExRam.Gremlinq.Providers.Neptune.NeptuneErrorCode right) { }
    }
    public sealed class NeptuneGremlinQueryExecutionException : ExRam.Gremlinq.Core.Execution.GremlinQueryExecutionException
    {
        public NeptuneGremlinQueryExecutionException(ExRam.Gremlinq.Providers.Neptune.NeptuneErrorCode code, ExRam.Gremlinq.Core.Execution.GremlinQueryExecutionContext executionContext, System.Exception innerException) { }
        public NeptuneGremlinQueryExecutionException(ExRam.Gremlinq.Providers.Neptune.NeptuneErrorCode code, ExRam.Gremlinq.Core.Execution.GremlinQueryExecutionContext executionContext, string message, System.Exception innerException) { }
        public ExRam.Gremlinq.Providers.Neptune.NeptuneErrorCode Code { get; }
    }
    public static class SigV4AWSSignerExtensions
    {
        public static ExRam.Gremlinq.Providers.Neptune.ISigV4AWSSigner WithCredentials(this ExRam.Gremlinq.Providers.Neptune.ISigV4AWSSigner signer, Amazon.Runtime.AWSCredentials credentials) { }
        public static ExRam.Gremlinq.Providers.Neptune.ISigV4AWSSigner WithCredentials(this ExRam.Gremlinq.Providers.Neptune.ISigV4AWSSigner signer, Amazon.Runtime.Identity.IIdentityResolver<Amazon.Runtime.AWSCredentials> identityResolver, Amazon.Runtime.IClientConfig? clientConfig = null) { }
        public static ExRam.Gremlinq.Providers.Neptune.ISigV4AWSSigner WithDefaultAWSCredentials(this ExRam.Gremlinq.Providers.Neptune.ISigV4AWSSigner signer, Amazon.Runtime.IClientConfig? clientConfig = null) { }
    }
}