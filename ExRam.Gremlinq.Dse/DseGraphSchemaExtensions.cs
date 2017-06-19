using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using LanguageExt;

namespace ExRam.Gremlinq.Dse
{
    public static class DseGraphSchemaExtensions
    {
        public static DseGraphSchema ToGraphSchema(this IGraphModel model)
        {
            var schema = new DseGraphSchema(model, ImmutableList<(string, string, string)>.Empty);

            model = model.EdgeConnectionClosure();

            return model.Connections
                .Where(x => !x.Item1.GetTypeInfo().IsAbstract && !x.Item2.GetTypeInfo().IsAbstract && !x.Item3.GetTypeInfo().IsAbstract)
                .Aggregate(
                    schema,
                    (closureSchema, connectionTuple) => closureSchema.Connection(
                        model.TryGetLabelOfType(connectionTuple.Item1).IfNone(() => throw new InvalidOperationException(/* TODO: Better exception */)),
                        model.TryGetLabelOfType(connectionTuple.Item2).IfNone(() => throw new InvalidOperationException(/* TODO: Better exception */)),
                        model.TryGetLabelOfType(connectionTuple.Item3).IfNone(() => throw new InvalidOperationException(/* TODO: Better exception */))));
        }

        public static DseGraphSchema Connection(this DseGraphSchema schema, string outVertexLabel, string edgeLabel, string inVertexLabel)
        {
            return new DseGraphSchema(schema.Model, schema.Connections.Add((outVertexLabel, edgeLabel, inVertexLabel)));
        }
    }
}