using System;
using System.Collections.Generic;
using System.Linq;
using Dse.Graph;
using ExRam.Gremlinq;

namespace Dse
{
    public static class DseSessionExtensions
    {
        private sealed class DseGraphQueryProvider : IGremlinQueryProvider
        {
            private readonly IDseSession _session;

            public DseGraphQueryProvider(IDseSession session)
            {
                this._session = session;
            }

            public IGremlinQuery CreateQuery()
            {
                return GremlinQuery.ForGraph("g", this);    //TODO: Get graph name from _session!!
            }

            public IAsyncEnumerable<T> Execute<T>(IGremlinQuery<T> query)
            {
                if (typeof(T) != typeof(string))
                    throw new NotSupportedException("Only string queries are supported.");

                var executableQuery = query.Serialize(false);

                return this._session
                    .ExecuteGraphAsync(new SimpleGraphStatement(executableQuery.parameters, executableQuery.queryString))
                    .ToAsyncEnumerable()
                    .SelectMany(node => node.ToAsyncEnumerable())
                    .Select(node => (T)(object)node.ToString());
            }

            public IGremlinModel Model => GremlinModel.Empty;
            public IGraphElementNamingStrategy NamingStrategy => GraphElementNamingStrategy.Simple;
        }

        public static IGremlinQueryProvider CreateQueryProvider(this IDseSession session, IGremlinModel model, IGraphElementNamingStrategy namingStrategy)
        {
            return new DseGraphQueryProvider(session)
                .WithModel(model)
                .WithNamingStrategy(namingStrategy)
                .WithJsonSupport();
        }
    }
}
