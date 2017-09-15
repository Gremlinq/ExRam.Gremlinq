using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Xml;
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
                foreach (var kvp in parameters.OrderByDescending(x => x.Key.Length))
                {
                    var value = kvp.Value;

                    switch (value)
                    {
                        case Enum _:
                            value = (int)value;
                            break;
                        case DateTimeOffset x:
                            value = x.ToString("yyyy-MM-ddTHH\\:mm\\:ss.fffffffzzz");
                            break;
                        case DateTime x:
                            value = x.ToUniversalTime().ToString("yyyy-MM-ddTHH\\:mm\\:ss.fffffffZ");
                            break;
                        case TimeSpan x:
                            value = XmlConvert.ToString(x);
                            break;
                        case byte[] x:
                            value = Convert.ToBase64String(x);
                            break;
                    }

                    if (value is string)
                        value = $"'{value}'";
                    else
                        value = value.ToString().ToLower();

                    query = query.Replace(kvp.Key, (string)value);
                }

                Console.WriteLine(query);

                var documentQuery = this._session.CreateGremlinQuery<JToken>(query);

                return AsyncEnumerable
                    .Repeat(Unit.Default)
                    .TakeWhile(_ => documentQuery.HasMoreResults)
                    .SelectMany(async (_, ct) =>
                    {
                        try
                        {
                            return await documentQuery.ExecuteNextAsync<JToken>(ct).ConfigureAwait(false);
                        }
                        catch (Exception ex)
                        {
                            throw new Exception(query, ex);
                        }
                    })
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
