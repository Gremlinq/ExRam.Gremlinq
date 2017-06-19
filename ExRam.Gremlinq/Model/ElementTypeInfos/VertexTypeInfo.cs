using System;
using System.Collections.Immutable;
using System.Linq.Expressions;

namespace ExRam.Gremlinq
{
    public sealed class VertexTypeInfo : GraphElementInfo
    {
        public VertexTypeInfo(Type elementType, string label, ImmutableList<Expression> secondaryIndexes) : base(elementType, label)
        {
            SecondaryIndexes = secondaryIndexes;
        }

        public ImmutableList<Expression> SecondaryIndexes { get; }
    }
}