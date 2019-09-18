using System;
using ExRam.Gremlinq.Core.Serialization;

namespace ExRam.Gremlinq.Core
{
    public static class GremlinQuerySerializer
    {
        private sealed class InvalidGremlinQuerySerializer : IGremlinQuerySerializer
        {
            public object Serialize(IGremlinQuery query)
            {
                throw new InvalidOperationException($"{nameof(Serialize)} must not be called on {nameof(GremlinQuerySerializer)}.{nameof(Invalid)}. If you are getting this exception while executing a query, configure a proper {nameof(IGremlinQuerySerializer)} on your {nameof(GremlinQuerySource)}.");
            }
        }

        private sealed class VisitorGremlinQuerySerializer<TVisitor> : IGremlinQuerySerializer
            where TVisitor : IGremlinQueryElementVisitor, new()
        {
            public object Serialize(IGremlinQuery query)
            {
                var visitor = new TVisitor();
                visitor.Visit(query);

                return visitor.Build();
            }
        }

        public static readonly IGremlinQuerySerializer Invalid = new InvalidGremlinQuerySerializer();

        public static IGremlinQuerySerializer FromVisitor<TVisitor>()
            where TVisitor : IGremlinQueryElementVisitor, new()
        {
            return new VisitorGremlinQuerySerializer<TVisitor>();
        }
    }
}
