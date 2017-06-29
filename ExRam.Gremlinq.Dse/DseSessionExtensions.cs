using System;
using System.Collections.Generic;
using System.Linq;
using Dse.Graph;
using ExRam.Gremlinq;

// ReSharper disable once CheckNamespace
namespace Dse
{
    public static class DseSessionExtensions
    {
        private sealed class DseGraphQueryProvider : IGremlinQueryProvider
        {
            private readonly IDseSession _session;

            public DseGraphQueryProvider(IDseSession session, IGraphModel model)
            {
                this.Model = model;
                this._session = session;
            }

            public IAsyncEnumerable<T> Execute<T>(IGremlinQuery<T> query)
            {
                if (typeof(T) != typeof(string))
                    throw new NotSupportedException("Only string queries are supported.");

                if (query.GraphName == null)
                    query = query.WithGraphName((this._session.Cluster as IDseCluster)?.Configuration.GraphOptions.Source ?? "g");

                var executableQuery = query
                    .Serialize(this.Model, false);

                return this._session
                    .ExecuteGraphAsync(new SimpleGraphStatement(executableQuery
                        .parameters
                        .ToDictionary(
                            kvp => kvp.Key,
                            kvp => kvp.Value is TimeSpan
                                ? Duration.FromTimeSpan((TimeSpan)kvp.Value)
                                : kvp.Value),
                            executableQuery.queryString))
                    .ToAsyncEnumerable()
                    .SelectMany(node => node.ToAsyncEnumerable())
                    .Select(node => (T)(object)node.ToString());
            }

            public IGraphModel Model { get; }
        }

        public static IGremlinQueryProvider CreateQueryProvider(this IDseSession session, IGraphModel model)
        {
            return new DseGraphQueryProvider(session, model)
                //.WithModel(model)
                .WithJsonSupport();
        }
    }
}
