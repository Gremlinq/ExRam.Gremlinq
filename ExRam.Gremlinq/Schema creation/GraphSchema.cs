using System;
using System.Collections.Immutable;

namespace ExRam.Gremlinq
{
    public static class GraphSchema
    {
        private sealed class GraphSchemaImpl : IGraphSchema
        {
            public GraphSchemaImpl(ImmutableList<VertexSchemaInfo> vertexSchemaInfos, ImmutableList<EdgeSchemaInfo> edgeSchemaInfos, ImmutableList<PropertySchemaInfo> propertySchemaInfos, ImmutableList<(string, string, string)> connections)
            {
                this.EdgeSchemaInfos = edgeSchemaInfos;
                this.VertexSchemaInfos = vertexSchemaInfos;
                this.PropertySchemaInfos = propertySchemaInfos;
                this.Connections = connections;
            }

            public ImmutableList<EdgeSchemaInfo> EdgeSchemaInfos { get; }
            public ImmutableList<VertexSchemaInfo> VertexSchemaInfos { get; }
            public ImmutableList<PropertySchemaInfo> PropertySchemaInfos { get; }
            public ImmutableList<(string, string, string)> Connections { get; }
        }

        public static readonly IGraphSchema Empty = new GraphSchemaImpl(ImmutableList<VertexSchemaInfo>.Empty, ImmutableList<EdgeSchemaInfo>.Empty, ImmutableList<PropertySchemaInfo>.Empty, ImmutableList<(string, string, string)>.Empty);

        public static IGraphSchema Property<T>(this IGraphSchema schema, string name)
        {
            return schema.Property(name, typeof(T));
        }

        public static IGraphSchema Property(this IGraphSchema schema, string name, Type type)
        {
            return new GraphSchemaImpl(schema.VertexSchemaInfos, schema.EdgeSchemaInfos, schema.PropertySchemaInfos.Add(new PropertySchemaInfo(name, type)), schema.Connections);
        }

        public static IGraphSchema VertexLabel(this IGraphSchema schema, string label, ImmutableList<string> properties, ImmutableList<string> partitionKeyProperties, ImmutableList<string> indexProperties)
        {
            return new GraphSchemaImpl(schema.VertexSchemaInfos.Add(new VertexSchemaInfo(label, properties, partitionKeyProperties, indexProperties)), schema.EdgeSchemaInfos, schema.PropertySchemaInfos, schema.Connections);
        }

        public static IGraphSchema EdgeLabel(this IGraphSchema schema, string label, ImmutableList<string> properties)
        {
            return new GraphSchemaImpl(schema.VertexSchemaInfos, schema.EdgeSchemaInfos.Add(new EdgeSchemaInfo(label, properties)), schema.PropertySchemaInfos, schema.Connections);
        }

        public static IGraphSchema Connection(this IGraphSchema schema, string outVertexLabel, string edgeLabel, string inVertexLabel)
        {
            return new GraphSchemaImpl(schema.VertexSchemaInfos, schema.EdgeSchemaInfos, schema.PropertySchemaInfos, schema.Connections.Add((outVertexLabel, edgeLabel, inVertexLabel)));
        }
    }
}