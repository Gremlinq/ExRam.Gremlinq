using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using LanguageExt;

namespace ExRam.Gremlinq
{
    public static class GraphModel
    {
        private sealed class GraphModelImpl : IGraphModel
        {
            public GraphModelImpl(IImmutableDictionary<Type, VertexTypeInfo> vertexTypes, IImmutableDictionary<Type, EdgeTypeInfo> edgeTypes, IImmutableList<(Type, Type, Type)> connections)
            {
                this.VertexTypes = vertexTypes;
                this.EdgeTypes = edgeTypes;
                this.Connections = connections;
            }

            public IImmutableDictionary<Type, VertexTypeInfo> VertexTypes { get; }

            public IImmutableDictionary<Type, EdgeTypeInfo> EdgeTypes { get; }

            public IImmutableList<(Type, Type, Type)> Connections { get; }
        }

        private sealed class VertexTypeInfoBuilder<T> : IVertexTypeInfoBuilder<T>
        {
            private readonly VertexTypeInfo _typeInfo;

            public VertexTypeInfoBuilder(VertexTypeInfo typeInfo)
            {
                this._typeInfo = typeInfo;
            }

            public VertexTypeInfo Build()
            {
                return this._typeInfo;
            }

            public IVertexTypeInfoBuilder<T> Label(string label)
            {
                return new VertexTypeInfoBuilder<T>(new VertexTypeInfo(this._typeInfo.ElementType, label, this._typeInfo.SecondaryIndexes, this._typeInfo.PrimaryKey));
            }

            public IVertexTypeInfoBuilder<T> SecondaryIndex(Expression<Func<T, object>> indexExpression)
            {
                return new VertexTypeInfoBuilder<T>(new VertexTypeInfo(this._typeInfo.ElementType, this._typeInfo.Label, this._typeInfo.SecondaryIndexes.Add(indexExpression), this._typeInfo.PrimaryKey));
            }

            public IVertexTypeInfoBuilder<T> PrimaryKey(Expression<Func<T, object>> expression)
            {
                return new VertexTypeInfoBuilder<T>(new VertexTypeInfo(this._typeInfo.ElementType, this._typeInfo.Label, this._typeInfo.SecondaryIndexes, expression));
            }
        }

        private sealed class EdgeTypeInfoBuilder<T> : IEdgeTypeInfoBuilder<T>
        {
            private readonly EdgeTypeInfo _typeInfo;

            public EdgeTypeInfoBuilder(EdgeTypeInfo typeInfo)
            {
                this._typeInfo = typeInfo;
            }

            public EdgeTypeInfo Build()
            {
                return this._typeInfo;
            }

            public IEdgeTypeInfoBuilder<T> Label(string label)
            {
                return new EdgeTypeInfoBuilder<T>(new EdgeTypeInfo(this._typeInfo.ElementType, label));
            }
        }

        public static readonly IGraphModel Empty = new GraphModelImpl(ImmutableDictionary<Type, VertexTypeInfo>.Empty, ImmutableDictionary<Type, EdgeTypeInfo>.Empty, ImmutableList<(Type, Type, Type)>.Empty);

        public static IGraphModel FromAssembly<TVertex, TEdge>(Assembly assembly, IGraphElementNamingStrategy namingStrategy)
        {
            return FromAssembly(assembly, typeof(TVertex), typeof(TEdge), namingStrategy);
        }

        public static IGraphModel FromAssembly(Assembly assembly, Type vertexBaseType, Type edgeBaseType, IGraphElementNamingStrategy namingStrategy)
        {
            if (vertexBaseType.IsAssignableFrom(edgeBaseType))
                throw new ArgumentException($"{vertexBaseType} may not be in the inheritance hierarchy of {edgeBaseType}.");

            if (edgeBaseType.IsAssignableFrom(vertexBaseType))
                throw new ArgumentException($"{edgeBaseType} may not be in the inheritance hierarchy of {vertexBaseType}.");

            return new GraphModelImpl(
                assembly
                    .DefinedTypes
                    .Where(typeInfo => vertexBaseType.IsAssignableFrom(typeInfo.AsType()))
                    .ToImmutableDictionary(
                        type => type.AsType(),
                        type => new VertexTypeInfo(type.AsType(), namingStrategy.GetLabelForType(type.AsType()), ImmutableList<Expression>.Empty)),
                assembly
                    .DefinedTypes
                    .Where(typeInfo => edgeBaseType.IsAssignableFrom(typeInfo.AsType()))
                    .ToImmutableDictionary(
                        type => type.AsType(),
                        type => new EdgeTypeInfo(type.AsType(), namingStrategy.GetLabelForType(type.AsType()))),
                ImmutableList<(Type, Type, Type)>.Empty);
        }

        public static IGraphModel EdgeType<T>(this IGraphModel model, Func<IEdgeTypeInfoBuilder<T>, IEdgeTypeInfoBuilder<T>> builderAction)
        {
            var type = typeof(T);

            var edgeInfo = model.EdgeTypes
                .TryGetValue(type)
                .IfNone(new EdgeTypeInfo(type, null));

            return model.VertexTypes.Keys
                .Where(vertexType => vertexType.IsAssignableFrom(type) || type.IsAssignableFrom(vertexType))
                .Select(_ => (Option<Type>)_)
                .FirstOrDefault()
                .Match(
                    contraditingVertexType => throw new ArgumentException($"Proposed edge type is inheritance hierarchy of vertex type {contraditingVertexType}."),
                    () => new GraphModelImpl(model.VertexTypes, model.EdgeTypes.SetItem(type, builderAction(new EdgeTypeInfoBuilder<T>(edgeInfo)).Build()), model.Connections));
        }

        public static IGraphModel VertexType<T>(this IGraphModel model, Func<IVertexTypeInfoBuilder<T>, IVertexTypeInfoBuilder<T>> builderAction)
        {
            var type = typeof(T);

            var vertexInfo = model.VertexTypes
                .TryGetValue(type)
                .IfNone(new VertexTypeInfo(type, null, ImmutableList<Expression>.Empty));

            return model.EdgeTypes.Keys
                .Where(edgeType => edgeType.IsAssignableFrom(type) || type.IsAssignableFrom(edgeType))
                .Select(_ => (Option<Type>)_)
                .FirstOrDefault()
                .Match(
                    contraditingEdgeType => throw new ArgumentException($"Proposed vertex type is inheritance hierarchy of edge type {contraditingEdgeType}."),
                    () => new GraphModelImpl(model.VertexTypes.SetItem(type, builderAction(new VertexTypeInfoBuilder<T>(vertexInfo)).Build()), model.EdgeTypes, model.Connections));
        }

        public static IGraphModel AddConnection<TOutVertex, TEdge, TInVertex>(this IGraphModel model)
        {
            return model.AddConnection(typeof(TOutVertex), typeof(TEdge), typeof(TInVertex));
        }

        public static IGraphModel EdgeConnectionClosure(this IGraphModel model)
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

        public static IGraphSchema ToGraphSchema(this IGraphModel model)
        {
            var schema = GraphSchema.Empty;
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

            schema = model.VertexTypes.Values
                .Where(x => !x.ElementType.GetTypeInfo().IsAbstract)
                .Aggregate(
                    schema,
                    (closureSchema, vertexType) => closureSchema.VertexLabel(
                        vertexType.Label,
                        vertexType.ElementType
                            .GetProperties()
                            .Select(property => property.Name)
                            .ToImmutableList(),
                        vertexType
                            .TryGetPartitionKeyExpression(model)
                            .Map(keyExpression => ((keyExpression as LambdaExpression)?.Body as MemberExpression)?.Member
                                .Name)
                            .AsEnumerable()
                            .ToImmutableList(),
                        vertexType
                            .SecondaryIndexes
                            .Select(indexExpression => ((indexExpression as LambdaExpression)?.Body.StripConvert() as MemberExpression)?.Member.Name)
                            .ToImmutableList()));

            schema = model.EdgeTypes.Values
                .Where(x => !x.ElementType.GetTypeInfo().IsAbstract)
                .Aggregate(
                    schema,
                    (closureSchema, edgeType) => closureSchema.EdgeLabel(
                        edgeType.Label,
                        edgeType.ElementType.GetProperties().Select(property => property.Name).ToImmutableList()));

            return model.Connections
                .Where(x => !x.Item1.GetTypeInfo().IsAbstract && !x.Item2.GetTypeInfo().IsAbstract && !x.Item3.GetTypeInfo().IsAbstract)
                .Aggregate(
                    schema,
                    (closureSchema, connectionTuple) => closureSchema.Connection(
                        model.TryGetLabelOfType(connectionTuple.Item1).IfNone(() => throw new InvalidOperationException(/* TODO: Better exception */)),
                        model.TryGetLabelOfType(connectionTuple.Item2).IfNone(() => throw new InvalidOperationException(/* TODO: Better exception */)),
                        model.TryGetLabelOfType(connectionTuple.Item3).IfNone(() => throw new InvalidOperationException(/* TODO: Better exception */))));
        }
        
        public static Option<string> TryGetLabelOfType(this IGraphModel model, Type type)
        {
            return model.VertexTypes
                .TryGetValue(type)
                .Match(
                    _ => _,
                    () => model.EdgeTypes
                        .TryGetValue(type)
                        .Map(_ => (GraphElementInfo) _))
                .Map(_ => _.Label);
        }

        internal static Option<Type> TryGetVertexTypeOfLabel(this IGraphModel model, string label)
        {
            return model.VertexTypes.Values
                .TryGetElementTypeOfLabel(label);
        }

        internal static Option<Type> TryGetEdgeTypeOfLabel(this IGraphModel model, string label)
        {
            return model.EdgeTypes.Values
                .TryGetElementTypeOfLabel(label);
        }

        internal static Option<Type> TryGetElementTypeOfLabel(this IEnumerable<GraphElementInfo> elementInfos, string label)
        {
            return elementInfos
                .Where(elementInfo => elementInfo.Label.Equals(label, StringComparison.OrdinalIgnoreCase))
                .Select(elementInfo => elementInfo.ElementType)
                .FirstOrDefault();
        }

        internal static IEnumerable<GraphElementInfo> GetDerivedElementInfos(this IGraphModel model, Type type, bool includeType)
        {
            return model.VertexTypes.Values
                .Cast<GraphElementInfo>()
                .Concat(model.EdgeTypes.Values)
                .Where(elementInfo => (includeType || elementInfo.ElementType != type) && type.IsAssignableFrom(elementInfo.ElementType));
        }

        private static IGraphModel AddConnection(this IGraphModel model, Type outVertexType, Type edgeType, Type inVertexType)
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
                : new GraphModelImpl(model.VertexTypes, model.EdgeTypes, model.Connections.Add(tuple));
        }

        private static Option<Expression> TryGetPartitionKeyExpression(this VertexTypeInfo vertexTypeInfo, IGraphModel model)
        {
            return vertexTypeInfo.PrimaryKey
                .Match(
                    _ => (Option<Expression>)_,
                    () =>
                    {
                        var baseType = vertexTypeInfo.ElementType.GetTypeInfo().BaseType;

                        if (baseType != null)
                        {
                            return model.VertexTypes
                                .TryGetValue(baseType)
                                .Bind(baseVertexInfo => baseVertexInfo.TryGetPartitionKeyExpression(model));
                        }

                        return Option<Expression>.None;
                    });
        }
    }
}