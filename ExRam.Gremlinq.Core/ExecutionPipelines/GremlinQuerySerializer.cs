using System;
using ExRam.Gremlinq.Core.Serialization;

namespace ExRam.Gremlinq.Core
{
    public static class GremlinQuerySerializer<TSerializedQuery>
    {
        private sealed class InvalidGremlinQuerySerializer : IGremlinQuerySerializer<TSerializedQuery>
        {
            public TSerializedQuery Serialize(IGremlinQuery query)
            {
                throw new InvalidOperationException($"{nameof(Serialize)} must not be called on {nameof(GremlinQuerySerializer<TSerializedQuery>)}.{nameof(Invalid)}. If you are getting this exception while executing a query, configure a proper {nameof(IGremlinQuerySerializer<TSerializedQuery>)} on your {nameof(GremlinQuerySource)}.");
            }
        }

        private sealed class VisitorGremlinQuerySerializer<TVisitor> : IGremlinQuerySerializer<TSerializedQuery>
            where TVisitor : IGremlinQueryElementVisitor<TSerializedQuery>, new()
        {
            public TSerializedQuery Serialize(IGremlinQuery query)
            {
                var visitor = new TVisitor();
                visitor.Visit(query);

                return visitor.Build();
            }
        }

        public static readonly IGremlinQuerySerializer<TSerializedQuery> Invalid = new InvalidGremlinQuerySerializer();

        public static IGremlinQuerySerializer<TSerializedQuery> FromVisitor<TVisitor>()
            where TVisitor : IGremlinQueryElementVisitor<TSerializedQuery>, new()
        {
            return new VisitorGremlinQuerySerializer<TVisitor>();
        }
    }
}
