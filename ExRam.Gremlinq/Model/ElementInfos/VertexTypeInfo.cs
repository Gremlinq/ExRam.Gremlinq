using System;
using System.Collections.Immutable;
using System.Linq.Expressions;
using LanguageExt;

namespace ExRam.Gremlinq
{
    public sealed class VertexTypeInfo : GraphElementInfo
    {
        public VertexTypeInfo(Type elementType, string label, ImmutableList<Expression> secondaryIndexes, Option<Expression> primaryKey = default(Option<Expression>)) : base(elementType, label)
        {
            SecondaryIndexes = secondaryIndexes;
            PrimaryKey = primaryKey;
        }

        public Option<Expression> PrimaryKey { get; }
        public ImmutableList<Expression> SecondaryIndexes { get; }
    }
}