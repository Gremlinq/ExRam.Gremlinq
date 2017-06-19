using System;
using System.Collections.Immutable;

namespace ExRam.Gremlinq
{
    public static class GraphSchema
    {
        internal sealed class GraphSchemaImpl : IGraphSchema
        {
            public GraphSchemaImpl(IGraphModel model, ImmutableList<VertexSchemaInfo> vertexSchemaInfos, ImmutableList<EdgeSchemaInfo> edgeSchemaInfos, ImmutableList<PropertySchemaInfo> propertySchemaInfos, ImmutableList<(string, string, string)> connections)
            {
                this.Model = model;
                this.EdgeSchemaInfos = edgeSchemaInfos;
                this.VertexSchemaInfos = vertexSchemaInfos;
                this.PropertySchemaInfos = propertySchemaInfos;
                this.Connections = connections;
            }

            public IGraphModel Model { get; }
            public ImmutableList<EdgeSchemaInfo> EdgeSchemaInfos { get; }
            public ImmutableList<VertexSchemaInfo> VertexSchemaInfos { get; }
            public ImmutableList<PropertySchemaInfo> PropertySchemaInfos { get; }
            public ImmutableList<(string, string, string)> Connections { get; }
        }

        public static IGraphSchema Property<T>(this IGraphSchema schema, string name)
        {
            return schema.Property(name, typeof(T));
        }

        public static IGraphSchema Property(this IGraphSchema schema, string name, Type type)
        {
            return new GraphSchemaImpl(schema.Model, schema.VertexSchemaInfos, schema.EdgeSchemaInfos, schema.PropertySchemaInfos.Add(new PropertySchemaInfo(name, type)), schema.Connections);
        }

        public static IGraphSchema VertexLabel(this IGraphSchema schema, string label, ImmutableList<string> properties, ImmutableList<string> partitionKeyProperties, ImmutableList<string> indexProperties)
        {
            return new GraphSchemaImpl(schema.Model, schema.VertexSchemaInfos.Add(new VertexSchemaInfo(label, properties, partitionKeyProperties, indexProperties)), schema.EdgeSchemaInfos, schema.PropertySchemaInfos, schema.Connections);
        }

        public static IGraphSchema EdgeLabel(this IGraphSchema schema, string label, ImmutableList<string> properties)
        {
            return new GraphSchemaImpl(schema.Model, schema.VertexSchemaInfos, schema.EdgeSchemaInfos.Add(new EdgeSchemaInfo(label, properties)), schema.PropertySchemaInfos, schema.Connections);
        }

        public static IGraphSchema Connection(this IGraphSchema schema, string outVertexLabel, string edgeLabel, string inVertexLabel)
        {
            return new GraphSchemaImpl(schema.Model, schema.VertexSchemaInfos, schema.EdgeSchemaInfos, schema.PropertySchemaInfos, schema.Connections.Add((outVertexLabel, edgeLabel, inVertexLabel)));
        }
    }
}