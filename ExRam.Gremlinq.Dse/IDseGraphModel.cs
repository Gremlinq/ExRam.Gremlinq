using System;
using System.Collections.Immutable;
using System.Linq.Expressions;

namespace ExRam.Gremlinq.Dse
{
    public interface IDseGraphModel : IGraphModel
    {
        IImmutableDictionary<Type, IImmutableList<(Type, Type)>> Connections { get; }

        IImmutableDictionary<Type, Expression> PrimaryKeys { get; }

        IImmutableDictionary<Type, IImmutableList<Expression>> MaterializedIndexes { get; }

        IImmutableDictionary<Type, IImmutableList<Expression>> SecondaryIndexes { get; }

        IImmutableDictionary<Type, Expression> SearchIndexes { get; }

    }
}