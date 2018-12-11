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
        private sealed class DseGraphNativeGremlinQueryProvider : IGremlinQueryProvider
        {
            private readonly IDseSession _session;

            public DseGraphNativeGremlinQueryProvider(IDseSession session)
            {
                _session = session;
            }

            public IAsyncEnumerable<TElement> Execute<TElement>(IGremlinQuery<TElement> query)
            {
                if (typeof(TElement) != typeof(string))
                    throw new NotSupportedException();

                var serialized = query
                    .Serialize();

                return _session
                    .ExecuteGraphAsync(new SimpleGraphStatement(
                        serialized.parameters
                            .ToDictionary(
                                kvp => kvp.Key,
                                kvp => kvp.Value is TimeSpan span
                                    ? Duration.FromTimeSpan(span)
                                    : kvp.Value),
                        serialized.queryString))
                    .ToAsyncEnumerable()
                    .SelectMany(node => node.ToAsyncEnumerable())
                    .Select(node => (TElement)(object)node.ToString());
            }
        }

        public static IGremlinQueryProvider CreateQueryProvider(this IDseSession session)
        {
            return new DseGraphNativeGremlinQueryProvider(session);
        }
    }
}
