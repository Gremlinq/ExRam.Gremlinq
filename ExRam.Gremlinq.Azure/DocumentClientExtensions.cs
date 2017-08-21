using System;
using System.Collections.Generic;
using System.Reactive;
using ExRam.Gremlinq;
using Microsoft.Azure.Graphs;

namespace Microsoft.Azure.Documents.Client
{
    public static class DocumentClientExtensions
    {
        private sealed class DocumentClientGremlinQueryProvider : INativeGremlinQueryProvider
        {
            private readonly DocumentClient _client;

            public DocumentClientGremlinQueryProvider(DocumentClient client)
            {
                this._client = client;
            }

            public IGremlinQuery<Unit> TraversalSource { get; }

            public IAsyncEnumerable<string> Execute(string query, IDictionary<string, object> parameters)
            {
                this._client.CreateGremlinQuery()
            }
        }

        public static INativeGremlinQueryProvider CreateQueryProvider(this DocumentClient client)
        {
            return new DocumentClientGremlinQueryProvider(client);
        }
    }
}
