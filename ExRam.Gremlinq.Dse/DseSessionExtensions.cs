using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using Dse.Graph;
using ExRam.Gremlinq;

// ReSharper disable once CheckNamespace
namespace Dse
{
    public static class DseSessionExtensions
    {
        private sealed class DseGraphNativeQueryProvider : INativeGremlinQueryProvider<string>
        {
            private readonly IDseSession _session;

            public DseGraphNativeQueryProvider(IDseSession session)
            {
                this._session = session;

                this.TraversalSource = GremlinQuery.Create((this._session.Cluster as IDseCluster)?.Configuration.GraphOptions.Source ?? "g");
            }

            public IAsyncEnumerable<string> Execute(string query, IDictionary<string, object> parameters)
            {
                return this._session
                    .ExecuteGraphAsync(new SimpleGraphStatement(parameters
                        .ToDictionary(
                            kvp => kvp.Key,
                            kvp => kvp.Value is TimeSpan span
                                ? Duration.FromTimeSpan(span)
                                : kvp.Value),
                        query))
                    .ToAsyncEnumerable()
                    .SelectMany(node => node.ToAsyncEnumerable())
                    .Select(node => node.ToString());
            }

            public IGremlinQuery<Unit> TraversalSource { get; }
        }

        public static INativeGremlinQueryProvider<string> CreateQueryProvider(this IDseSession session)
        {
            return new DseGraphNativeQueryProvider(session);
        }
    }
}
