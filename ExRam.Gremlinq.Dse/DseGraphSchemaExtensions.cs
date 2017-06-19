using System;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;
using LanguageExt;

namespace ExRam.Gremlinq.Dse
{
    public static class DseGraphSchemaExtensions
    {
        private sealed class DseGraphModel : IDseGraphModel
        {
            public DseGraphModel(
                IImmutableDictionary<Type, VertexTypeInfo> vertexTypes, 
                IImmutableDictionary<Type, EdgeTypeInfo> edgeTypes, 
                IImmutableList<(Type, Type, Type)> connections, 
                IImmutableDictionary<Type, Expression> primaryKeys,
                IImmutableDictionary<Type, IImmutableList<Expression>> secondaryIndexes)
            {
                this.VertexTypes = vertexTypes;
                this.EdgeTypes = edgeTypes;
                this.Connections = connections;
                this.PrimaryKeys = primaryKeys;
                this.SecondaryIndexes = secondaryIndexes;
            }

            public IImmutableDictionary<Type, VertexTypeInfo> VertexTypes { get; }

            public IImmutableDictionary<Type, EdgeTypeInfo> EdgeTypes { get; }

            public IImmutableList<(Type, Type, Type)> Connections { get; }

            public IImmutableDictionary<Type, Expression> PrimaryKeys { get; }

            public IImmutableDictionary<Type, IImmutableList<Expression>> SecondaryIndexes { get; }
        }

        public static IDseGraphModel ToDseGraphModel(this IGraphModel model)
        {
            return new DseGraphModel(model.VertexTypes, model.EdgeTypes, ImmutableList<(Type, Type, Type)>.Empty, ImmutableDictionary<Type, Expression>.Empty, ImmutableDictionary<Type, IImmutableList<Expression>>.Empty);
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
                            model = model.AddConnection(outVertexClosure.ElementType, edgeClosure.ElementType, inVertexClosure.ElementType);
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
            return new DseGraphModel(model.VertexTypes, model.EdgeTypes, model.Connections, model.PrimaryKeys.SetItem(typeof(T), expression), model.SecondaryIndexes);
        }


        public static IDseGraphModel SecondaryIndex<T>(this IDseGraphModel model, Expression<Func<T, object>> indexExpression)
        {
            return new DseGraphModel(
                model.VertexTypes,
                model.EdgeTypes,
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
            var outVertexInfo = model.VertexTypes
                .TryGetValue(outVertexType)
                .Map(vertexInfo => vertexInfo.ElementType)
                .IfNone(() => throw new ArgumentException($"Model does not contain vertex type {outVertexType}."));

            var inVertexInfo = model.VertexTypes
                .TryGetValue(inVertexType)
                .Map(vertexInfo => vertexInfo.ElementType)
                .IfNone(() => throw new ArgumentException($"Model does not contain vertex type {inVertexType}."));

            var connectionEdgeInfo = model.EdgeTypes
                .TryGetValue(edgeType)
                .Map(edgeInfo => edgeInfo.ElementType)
                .IfNone(() => throw new ArgumentException($"Model does not contain edge type {edgeType}."));

            var tuple = (outVertexInfo, connectionEdgeInfo, inVertexInfo);

            return model.Connections.Contains(tuple)
                ? model
                : new DseGraphModel(model.VertexTypes, model.EdgeTypes, model.Connections.Add(tuple), model.PrimaryKeys, model.SecondaryIndexes);
        }
    }
}