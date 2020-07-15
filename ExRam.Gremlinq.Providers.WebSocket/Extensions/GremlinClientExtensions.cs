using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Gremlin.Net.Driver;
using Gremlin.Net.Driver.Messages;

namespace ExRam.Gremlinq.Core
{
    public static class GremlinClientExtensions
    {
        private sealed class RequestInterceptingGremlinClient : IGremlinClient
        {
            private readonly IGremlinClient _baseClient;
            private readonly Func<RequestMessage, Task<RequestMessage>> _transformation;

            public RequestInterceptingGremlinClient(IGremlinClient baseClient, Func<RequestMessage, Task<RequestMessage>> transformation)
            {
                _baseClient = baseClient;
                _transformation = transformation;
            }

            public async Task<ResultSet<TResult>> SubmitAsync<TResult>(RequestMessage requestMessage)
            {
                return await _baseClient.SubmitAsync<TResult>(await _transformation(requestMessage));
            }

            public void Dispose()
            {
                _baseClient.Dispose();
            }
        }

        private sealed class ObserveResultStatusAttributesGremlinClient : IGremlinClient
        {
            private readonly IGremlinClient _baseClient;
            private readonly Action<RequestMessage, IReadOnlyDictionary<string, object>> _observer;

            public ObserveResultStatusAttributesGremlinClient(IGremlinClient baseClient, Action<RequestMessage, IReadOnlyDictionary<string, object>> observer)
            {
                _observer = observer;
                _baseClient = baseClient;
            }

            public async Task<ResultSet<TResult>> SubmitAsync<TResult>(RequestMessage requestMessage)
            {
                var resultSet = await _baseClient.SubmitAsync<TResult>(requestMessage);

                _observer(requestMessage, resultSet.StatusAttributes);

                return resultSet;
            }

            public void Dispose()
            {
                _baseClient.Dispose();
            }
        }

        public static IGremlinClient TransformRequest(this IGremlinClient client, Func<RequestMessage, Task<RequestMessage>> transformation) => new RequestInterceptingGremlinClient(client, transformation);

        public static IGremlinClient ObserveResultStatusAttributes(this IGremlinClient client, Action<RequestMessage, IReadOnlyDictionary<string, object>> observer) => new ObserveResultStatusAttributesGremlinClient(client, observer);
    }
}
