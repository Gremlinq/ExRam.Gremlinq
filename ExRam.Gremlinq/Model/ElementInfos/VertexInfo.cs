using System;
using System.Linq.Expressions;
using LanguageExt;

namespace ExRam.Gremlinq
{
    public sealed class VertexInfo : GraphElementInfo
    {
        public VertexInfo(Type elementType, string label, Option<Expression<Func<object, object>>> primaryKey = default(Option<Expression<Func<object, object>>>)) : base(elementType, label)
        {
            PrimaryKey = primaryKey;
        }

        public Option<Expression<Func<object, object>>> PrimaryKey { get; }
    }
}