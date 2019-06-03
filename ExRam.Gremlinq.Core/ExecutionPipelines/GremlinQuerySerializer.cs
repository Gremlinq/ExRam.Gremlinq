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
                throw new InvalidOperationException();//TODO: message
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