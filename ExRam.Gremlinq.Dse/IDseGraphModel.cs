using System;
using System.Collections.Immutable;
using System.Linq.Expressions;

namespace ExRam.Gremlinq.Dse
{
    public interface IDseGraphModel : IGraphModel
    {
        IImmutableDictionary<Type, IImmutableSet<(Type, Type)>> Connections { get; }

        IImmutableDictionary<Type, Expression> PrimaryKeys { get; }

        IImmutableDictionary<Type, IImmutableSet<Expression>> MaterializedIndexes { get; }

        IImmutableDictionary<Type, IImmutableSet<Expression>> SecondaryIndexes { get; }

        IImmutableDictionary<Type, Expression> SearchIndexes { get; }

        IImmutableDictionary<Type, IImmutableSet<(Expression, EdgeDirection)>> EdgeIndexes { get; }
    }
}