using System;
using System.Linq.Expressions;
using LanguageExt;

namespace ExRam.Gremlinq
{
    public sealed class VertexInfo : GraphElementInfo
    {
        public VertexInfo(Type elementType, string label, Option<Expression> primaryKey = default(Option<Expression>)) : base(elementType, label)
        {
            PrimaryKey = primaryKey;
        }

        public Option<Expression> PrimaryKey { get; }
    }
}