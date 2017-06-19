using System;
using System.Collections.Immutable;
using System.Linq;
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
                IImmutableList<(Type, Type, Type)> connections, 
                IImmutableDictionary<Type, Expression> primaryKeys,
                IImmutableDictionary<Type, IImmutableList<Expression>> secondaryIndexes)
            {
                this.VertexLabels = vertexLabels;
                this.EdgeLabels = edgeTypes;
                this.Connections = connections;
                this.PrimaryKeys = primaryKeys;
                this.SecondaryIndexes = secondaryIndexes;
            }

            public IImmutableDictionary<Type, string> VertexLabels { get; }

            public IImmutableDictionary<Type, string> EdgeLabels { get; }

            public IImmutableList<(Type, Type, Type)> Connections { get; }

            public IImmutableDictionary<Type, Expression> PrimaryKeys { get; }

            public IImmutableDictionary<Type, IImmutableList<Expression>> SecondaryIndexes { get; }
        }

        public static IDseGraphModel ToDseGraphModel(this IGraphModel model)
        {
            return new DseGraphModel(model.VertexLabels, model.EdgeLabels, ImmutableList<(Type, Type, Type)>.Empty, ImmutableDictionary<Type, Expression>.Empty, ImmutableDictionary<Type, IImmutableList<Expression>>.Empty);
        }

        public static IDseGraphModel EdgeConnectionClosure(this IDseGraphModel model)
        {
            foreach (var connection in model.Connections)
            {
                foreach (var outVertexClosure in model.GetDerivedElementInfos(connection.Item1, true))
                {
                    foreach (var edgeClosure in model.GetDerivedElementInfos(connection.Item2, true))
                    {
                        foreach (var inVertexClosure in model.GetDerivedElementInfos(connection.Item3, true))
                        {
                            model = model.AddConnection(outVertexClosure, edgeClosure, inVertexClosure);
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
            return new DseGraphModel(model.VertexLabels, model.EdgeLabels, model.Connections, model.PrimaryKeys.SetItem(typeof(T), expression), model.SecondaryIndexes);
        }

        public static IDseGraphModel SecondaryIndex<T>(this IDseGraphModel model, Expression<Func<T, object>> indexExpression)
        {
            return new DseGraphModel(
                model.VertexLabels,
                model.EdgeLabels,
                model.Connections,
                model.PrimaryKeys,
                model.SecondaryIndexes.SetItem(
                    typeof(T),
                    model.SecondaryIndexes
                        .TryGetValue(typeof(T))
                        .Match(
                            list => list.Add(indexExpression),
                            () => ImmutableList.Create<Expression>(indexExpression))));
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

            var tuple = (outVertexType, edgeType, inVertexType);

            return model.Connections.Contains(tuple)
                ? model
                : new DseGraphModel(model.VertexLabels, model.EdgeLabels, model.Connections.Add(tuple), model.PrimaryKeys, model.SecondaryIndexes);
        }
    }
}