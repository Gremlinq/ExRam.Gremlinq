using System;
using System.Collections.Immutable;
using System.Linq;
using LanguageExt;

namespace ExRam.Gremlinq.Dse
{
    public static class DseGraphSchemaExtensions
    {
        public static DseGraphModel ToGraphSchema(this IGraphModel model)
        {
            return new DseGraphModel(model.VertexTypes, model.EdgeTypes, ImmutableList<(Type, Type, Type)>.Empty);
        }

        public static DseGraphModel EdgeConnectionClosure(this DseGraphModel model)
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

        public static DseGraphModel AddConnection<TOutVertex, TEdge, TInVertex>(this DseGraphModel model)
        {
            return model.AddConnection(typeof(TOutVertex), typeof(TEdge), typeof(TInVertex));
        }

        private static DseGraphModel AddConnection(this DseGraphModel model, Type outVertexType, Type edgeType, Type inVertexType)
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
                : new DseGraphModel(model.VertexTypes, model.EdgeTypes, model.Connections.Add(tuple));
        }
    }
}