using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using ExRam.Gremlinq;
using Newtonsoft.Json.Linq;

namespace Microsoft.Azure.Documents.Client
{
    public static class CosmosDbSessionExtensions
    {
        private sealed class DocumentClientGremlinQueryProvider : INativeGremlinQueryProvider
        {
            private readonly ICosmosDbSession _session;

            public DocumentClientGremlinQueryProvider(ICosmosDbSession session)
            {
                this._session = session;
                this.TraversalSource = GremlinQuery.Create("g");
            }

            public IAsyncEnumerable<string> Execute(string query, IDictionary<string, object> parameters)
            {
                foreach(var kvp in parameters)
                {
                    var value = kvp.Value;

                    if (value is string)
                        value = $"'{value}'";
                    else
                        value = value.ToString().ToLower();

                    query = query.Replace(kvp.Key, (string)value);
                }

                var documentQuery = this._session.CreateGremlinQuery<JToken>(query);

                return AsyncEnumerable
                    .Repeat(Unit.Default)
                    .TakeWhile(_ => documentQuery.HasMoreResults)
                    .SelectMany((_, ct) => documentQuery.ExecuteNextAsync<JToken>(ct))
                    .SelectMany(x => x.ToAsyncEnumerable())
                    .Select(x => x.ToString());
            }

            public IGremlinQuery<Unit> TraversalSource { get; }
        }

        public static INativeGremlinQueryProvider CreateQueryProvider(this ICosmosDbSession session)
        {
            return new DocumentClientGremlinQueryProvider(session);
        }
    }
}
