using System;
using System.Collections.Immutable;
using System.Linq.Expressions;
using LanguageExt;

namespace ExRam.Gremlinq.Dse
{
    public static class DseGraphModelExtensions
    {
        private sealed class DseGraphModel : IDseGraphModel
        {
            public DseGraphModel(
                IImmutableDictionary<Type, string> vertexLabels,
                IImmutableDictionary<Type, string> edgeTypes,
                IImmutableDictionary<Type, IImmutableSet<(Type, Type)>> connections, 
                IImmutableDictionary<Type, Expression> primaryKeys,
                IImmutableDictionary<Type, IImmutableSet<Expression>> materializedIndexes,
                IImmutableDictionary<Type, IImmutableSet<Expression>> secondaryIndexes,
                IImmutableDictionary<Type, Expression> searchIndexes,
                IImmutableDictionary<Type, IImmutableSet<(Type vertexType, Expression indexExpression, EdgeDirection direction)>> edgeIndexes)
            {
                this.VertexLabels = vertexLabels;
                this.EdgeLabels = edgeTypes;
                this.Connections = connections;
                this.PrimaryKeys = primaryKeys;
                this.MaterializedIndexes = materializedIndexes;
                this.SecondaryIndexes = secondaryIndexes;
                this.SearchIndexes = searchIndexes;
                this.EdgeIndexes = edgeIndexes;
            }

            public Option<string> IdPropertyName => Option<string>.None;

            public IImmutableDictionary<Type, string> VertexLabels { get; }

            public IImmutableDictionary<Type, string> EdgeLabels { get; }

            public IImmutableDictionary<Type, IImmutableSet<(Type, Type)>> Connections { get; }

            public IImmutableDictionary<Type, Expression> PrimaryKeys { get; }

            public IImmutableDictionary<Type, IImmutableSet<Expression>> MaterializedIndexes { get; }

            public IImmutableDictionary<Type, IImmutableSet<Expression>> SecondaryIndexes { get; }

            public IImmutableDictionary<Type, Expression> SearchIndexes { get; }

            public IImmutableDictionary<Type, IImmutableSet<(Type vertexType, Expression indexExpression, EdgeDirection direction)>> EdgeIndexes { get; }
        }

        public static IDseGraphModel ToDseGraphModel(this IGraphModel model)
        {
            return new DseGraphModel(
                model.VertexLabels, 
                model.EdgeLabels, 
                ImmutableDictionary<Type, IImmutableSet<(Type, Type)>>.Empty,
                ImmutableDictionary<Type, Expression>.Empty, 
                ImmutableDictionary<Type, IImmutableSet<Expression>>.Empty, 
                ImmutableDictionary<Type, IImmutableSet<Expression>>.Empty,
                ImmutableDictionary<Type, Expression>.Empty,
                ImmutableDictionary<Type, IImmutableSet<(Type vertexType, Expression indexExpression, EdgeDirection direction)>>.Empty);
        }

        public static IDseGraphModel EdgeConnectionClosure(this IDseGraphModel model)
        {
            foreach (var kvp in model.Connections)
            {
                foreach (var edgeClosure in model.GetDerivedTypes(kvp.Key, true))
                {
                    foreach (var tuple in kvp.Value)
                    {
                        foreach (var outVertexClosure in model.GetDerivedTypes(tuple.Item1, true))
                        {
                            foreach (var inVertexClosure in model.GetDerivedTypes(tuple.Item2, true))
                            {
                                model = model.AddConnection(outVertexClosure, edgeClosure, inVertexClosure);
                            }
                        }
                    }
                }
            }

            return model;
        }

        public static IDseGraphModel AddConnection<TOutVertex, TEdge, TInVertex>(this IDseGraphModel model)
        {
            return model.AddConnection(typeof(TOutVertex), typeof(TEdge), typeof(TInVertex));
        }

        public static IDseGraphModel PrimaryKey<T>(this IDseGraphModel model, Expression<Func<T, object>> expression)
        {
            var newPrimaryKeys = model.PrimaryKeys.SetItem(typeof(T), expression);

            return newPrimaryKeys != model.PrimaryKeys
                ? new DseGraphModel(
                    model.VertexLabels, 
                    model.EdgeLabels, 
                    model.Connections,
                    newPrimaryKeys,
                    model.MaterializedIndexes, 
                    model.SecondaryIndexes,
                    model.SearchIndexes,
                    model.EdgeIndexes)
                : model;
        }

        public static IDseGraphModel MaterializedIndex<T>(this IDseGraphModel model, Expression<Func<T, object>> indexExpression)
        {
            var newMaterializedIndexes = model.MaterializedIndexes.Add(typeof(T), indexExpression);

            return newMaterializedIndexes != model.MaterializedIndexes
                ? new DseGraphModel(
                    model.VertexLabels,
                    model.EdgeLabels,
                    model.Connections,
                    model.PrimaryKeys,
                    model.MaterializedIndexes.Add(typeof(T), indexExpression),
                    model.SecondaryIndexes,
                    model.SearchIndexes,
                    model.EdgeIndexes)
                : model;
        }

        public static IDseGraphModel SecondaryIndex<T>(this IDseGraphModel model, Expression<Func<T, object>> indexExpression)
        {
            var newSecondaryIndexes = model.SecondaryIndexes.Add(typeof(T), indexExpression);

            return newSecondaryIndexes != model.SecondaryIndexes 
                ? new DseGraphModel(
                    model.VertexLabels,
                    model.EdgeLabels,
                    model.Connections,
                    model.PrimaryKeys,
                    model.MaterializedIndexes,
                    newSecondaryIndexes,
                    model.SearchIndexes,
                    model.EdgeIndexes)
                :  model;
        }

        public static IDseGraphModel SearchIndex<T>(this IDseGraphModel model, Expression<Func<T, object>> indexExpression)
        {
            var newSearchIndexes = model.SearchIndexes.SetItem(typeof(T), indexExpression);

            return newSearchIndexes != model.SearchIndexes
                ? new DseGraphModel(
                    model.VertexLabels,
                    model.EdgeLabels,
                    model.Connections,
                    model.PrimaryKeys,
                    model.MaterializedIndexes,
                    model.SecondaryIndexes,
                    newSearchIndexes,
                    model.EdgeIndexes)
                : model;
        }

        public static IDseGraphModel EdgeIndex<TVertex, TEdge>(this IDseGraphModel model, Expression<Func<TEdge, object>> indexExpression, EdgeDirection direction)
        {
            var newEdgeIndexes = model.EdgeIndexes.Add(typeof(TEdge), (typeof(TVertex), indexExpression, direction));

            return newEdgeIndexes != model.EdgeIndexes
                ? new DseGraphModel(
                    model.VertexLabels,
                    model.EdgeLabels,
                    model.Connections,
                    model.PrimaryKeys,
                    model.MaterializedIndexes,
                    model.SecondaryIndexes,
                    model.SearchIndexes,
                    newEdgeIndexes)
                : model;
        }
        
        private static IDseGraphModel AddConnection(this IDseGraphModel model, Type outVertexType, Type edgeType, Type inVertexType)
        {
            model.VertexLabels
                .TryGetValue(outVertexType)
                .IfNone(() => throw new ArgumentException($"Model does not contain vertex type {outVertexType}."));

            model.VertexLabels
                .TryGetValue(inVertexType)
                .IfNone(() => throw new ArgumentException($"Model does not contain vertex type {inVertexType}."));

            model.EdgeLabels
                .TryGetValue(edgeType)
                .IfNone(() => throw new ArgumentException($"Model does not contain edge type {edgeType}."));

            var newConnections = model.Connections.Add(edgeType, (outVertexType, inVertexType));

            return newConnections != model.Connections
                ? new DseGraphModel(
                    model.VertexLabels, 
                    model.EdgeLabels, 
                    newConnections, 
                    model.PrimaryKeys, 
                    model.MaterializedIndexes, 
                    model.SecondaryIndexes, 
                    model.SearchIndexes,
                    model.EdgeIndexes)
                : model;
        }
    }
}