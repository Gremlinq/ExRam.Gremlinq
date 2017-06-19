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
            var schema = new DseGraphSchema(model, ImmutableList<PropertySchemaInfo>.Empty, ImmutableList<(string, string, string)>.Empty);
            var propertyKeys = new Dictionary<string, Type>();

            model = model.EdgeConnectionClosure();

            foreach (var vertexType in model.VertexTypes.Values.Cast<GraphElementInfo>().Concat(model.EdgeTypes.Values))
            {
                foreach (var property in vertexType.ElementType.GetProperties())
                {
                    var propertyType = property.PropertyType;

                    while (true)
                    {
                        if (propertyType.GetTypeInfo().IsEnum)
                            propertyType = Enum.GetUnderlyingType(propertyType);
                        else
                        {
                            var maybeNullableType = Nullable.GetUnderlyingType(propertyType);
                            if (maybeNullableType != null)
                                propertyType = maybeNullableType;
                            else
                                break;
                        }
                    }

                    if (propertyKeys.TryGetValue(property.Name, out var existingType))
                    {
                        if (existingType != propertyType) //TODO: Support any kind of inheritance here?
                            throw new InvalidOperationException($"Property {property.Name} already exists with type {existingType.Name}.");
                    }
                    else
                        propertyKeys.Add(property.Name, propertyType);
                }
            }

            schema = propertyKeys
                .Aggregate(
                    schema,
                    (closureSchema, propertyKvp) => closureSchema.Property(propertyKvp.Key, propertyKvp.Value));

            return model.Connections
                .Where(x => !x.Item1.GetTypeInfo().IsAbstract && !x.Item2.GetTypeInfo().IsAbstract && !x.Item3.GetTypeInfo().IsAbstract)
                .Aggregate(
                    schema,
                    (closureSchema, connectionTuple) => closureSchema.Connection(
                        model.TryGetLabelOfType(connectionTuple.Item1).IfNone(() => throw new InvalidOperationException(/* TODO: Better exception */)),
                        model.TryGetLabelOfType(connectionTuple.Item2).IfNone(() => throw new InvalidOperationException(/* TODO: Better exception */)),
                        model.TryGetLabelOfType(connectionTuple.Item3).IfNone(() => throw new InvalidOperationException(/* TODO: Better exception */))));
        }

        public static DseGraphSchema Property<T>(this DseGraphSchema schema, string name)
        {
            return schema.Property(name, typeof(T));
        }

        public static DseGraphSchema Property(this DseGraphSchema schema, string name, Type type)
        {
            return new DseGraphSchema(schema.Model, schema.PropertySchemaInfos.Add(new PropertySchemaInfo(name, type)), schema.Connections);
        }

        public static DseGraphSchema Connection(this DseGraphSchema schema, string outVertexLabel, string edgeLabel, string inVertexLabel)
        {
            return new DseGraphSchema(schema.Model, schema.PropertySchemaInfos, schema.Connections.Add((outVertexLabel, edgeLabel, inVertexLabel)));
        }
    }
}