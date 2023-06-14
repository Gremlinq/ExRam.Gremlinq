namespace ExRam.Gremlinq.Core
{
    public static class ConfigurableGremlinQuerySourceExtensions
    {
        public static ExRam.Gremlinq.Core.IGremlinQuerySource UseNeptune(this ExRam.Gremlinq.Core.IConfigurableGremlinQuerySource source, System.Func<ExRam.Gremlinq.Providers.Neptune.INeptuneConfigurator, ExRam.Gremlinq.Core.IGremlinQuerySourceTransformation> configuratorTransformation) { }
    }
}
namespace ExRam.Gremlinq.Providers.Neptune
{
    public interface INeptuneConfigurator : ExRam.Gremlinq.Core.IGremlinQuerySourceTransformation, ExRam.Gremlinq.Core.IGremlinqConfigurator<ExRam.Gremlinq.Providers.Neptune.INeptuneConfigurator>, ExRam.Gremlinq.Providers.Core.IProviderConfigurator<ExRam.Gremlinq.Providers.Neptune.INeptuneConfigurator>, ExRam.Gremlinq.Providers.Core.IWebSocketProviderConfigurator<ExRam.Gremlinq.Providers.Neptune.INeptuneConfigurator> { }
    public static class NeptuneConfiguratorExtensions
    {
        public static ExRam.Gremlinq.Providers.Neptune.INeptuneConfigurator UseElasticSearch(this ExRam.Gremlinq.Providers.Neptune.INeptuneConfigurator configurator, System.Uri elasticSearchEndPoint, ExRam.Gremlinq.Providers.Neptune.NeptuneElasticSearchIndexConfiguration indexConfiguration = 0) { }
    }
    public enum NeptuneElasticSearchIndexConfiguration
    {
        Standard = 0,
        LowercaseKeyword = 1,
    }
    public enum NeptuneErrorCode
    {
        AccessDeniedException = 0,
        BadRequestException = 1,
        CancelledByUserException = 2,
        ConcurrentModificationException = 3,
        ConstraintViolationException = 4,
        InternalFailureException = 5,
        InvalidNumericDataException = 6,
        InvalidParameterException = 7,
        MalformedQueryException = 8,
        MemoryLimitExceededException = 9,
        MethodNotAllowedException = 10,
        MissingParameterException = 11,
        QueryLimitExceededException = 12,
        QueryLimitException = 13,
        QueryTooLargeException = 14,
        ReadOnlyViolationException = 15,
        ThrottlingException = 16,
        TimeLimitExceededException = 17,
        TooManyRequestsException = 18,
        UnsupportedOperationException = 19,
    }
    public sealed class NeptuneResponseException : System.Exception
    {
        public NeptuneResponseException(ExRam.Gremlinq.Providers.Neptune.NeptuneErrorCode code, string detailedMessage, System.Exception innerException) { }
        public ExRam.Gremlinq.Providers.Neptune.NeptuneErrorCode Code { get; }
    }
}