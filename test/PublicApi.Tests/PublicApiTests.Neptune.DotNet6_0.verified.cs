namespace ExRam.Gremlinq.Core
{
    public static class ConfigurableGremlinQuerySourceExtensions
    {
        public static ExRam.Gremlinq.Core.IGremlinQuerySource UseNeptune<TVertexBase, TEdgeBase>(this ExRam.Gremlinq.Core.IConfigurableGremlinQuerySource source, System.Func<ExRam.Gremlinq.Providers.Neptune.INeptuneConfigurator, ExRam.Gremlinq.Core.IGremlinQuerySourceTransformation> configuratorTransformation) { }
    }
}
namespace ExRam.Gremlinq.Providers.Neptune
{
    public interface INeptuneConfigurator : ExRam.Gremlinq.Core.IGremlinQuerySourceTransformation, ExRam.Gremlinq.Core.IGremlinqConfigurator<ExRam.Gremlinq.Providers.Neptune.INeptuneConfigurator>, ExRam.Gremlinq.Providers.Core.IWebSocketProviderConfigurator<ExRam.Gremlinq.Providers.Neptune.INeptuneConfigurator> { }
    public static class NeptuneConfiguratorExtensions
    {
        public static ExRam.Gremlinq.Providers.Neptune.INeptuneConfigurator UseElasticSearch(this ExRam.Gremlinq.Providers.Neptune.INeptuneConfigurator configurator, System.Uri elasticSearchEndPoint, ExRam.Gremlinq.Providers.Neptune.NeptuneElasticSearchIndexConfiguration indexConfiguration = 0) { }
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
}