using ExRam.Gremlinq.Core.Execution;

using Gremlin.Net.Driver.Exceptions;

using System.Text.Json;
using System.Text.Json.Serialization;

namespace ExRam.Gremlinq.Providers.Neptune
{
    internal static class ExceptionExtensions
    {
        private static readonly JsonSerializerOptions SerializerOptions = new ()
        {
            PropertyNameCaseInsensitive = true,
            Converters =
            {
                new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
            }
        };

        private record struct NeptuneErrorResponse(string? Code, string? DetailedMessage);

        public static NeptuneGremlinQueryExecutionException? TryGetNeptuneGremlinQueryExecutionException(this GremlinQueryExecutionException ex)
        {
            var ret = default(NeptuneGremlinQueryExecutionException?);

            if (ex.InnerException is ResponseException responseException)
            {
                var statusCodeString = responseException.StatusCode.ToString();

                if (responseException.Message.StartsWith(statusCodeString) && responseException.Message.Length > statusCodeString.Length)
                {
                    try
                    {
                        var response = JsonSerializer.Deserialize<NeptuneErrorResponse>(responseException.Message.AsSpan()[(statusCodeString.Length + 1)..], SerializerOptions);

                        if (response.Code is { Length: > 0 } errorCode && NeptuneErrorCode.From(errorCode) is var neptuneErrorCode)
                        {
                            ret = response.DetailedMessage is { Length: > 0 } detailedMessage
                                ? new NeptuneGremlinQueryExecutionException(neptuneErrorCode, ex.ExecutionContext, detailedMessage, ex)
                                : new NeptuneGremlinQueryExecutionException(neptuneErrorCode, ex.ExecutionContext, ex);
                        }
                    }
                    catch (JsonException)
                    {

                    }
                }
            }

            return ret;
        }
    }
}
