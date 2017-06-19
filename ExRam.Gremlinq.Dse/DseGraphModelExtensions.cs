using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using System.Threading;
using Dse;
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

        private static readonly IReadOnlyDictionary<Type, string> NativeTypeSteps = new Dictionary<Type, string>
        {
            { typeof(long), "Bigint" },
            { typeof(byte[]), "Blob" },
            { typeof(bool), "Boolean" },
            { typeof(decimal), "Decimal" },
            { typeof(double), "Double" },
            { typeof(TimeSpan), "Duration" },
            { typeof(IPAddress), "Inet" },
            { typeof(int), "Int" },
            //{ typeof(?), new GremlinStep("Linestring") },
            //{ typeof(?), new GremlinStep("Point") },
            //{ typeof(?), new GremlinStep("Polygon") },
            { typeof(short), "Smallint" },
            { typeof(string), "Text" },
            { typeof(DateTime), "Timestamp" },
            { typeof(Guid), "Uuid" }
            //{ typeof(?), new GremlinStep("Varint") },
        };

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

        public static IEnumerable<IGremlinQuery<string>> CreateSchemaQueries(this IDseGraphModel model, IGremlinQueryProvider queryProvider)
        {
            model = model.EdgeConnectionClosure();

            var propertyKeys = new Dictionary<string, Type>();

            foreach (var type in model.VertexLabels.Keys.Concat(model.EdgeLabels.Keys))
            {
                foreach (var property in type.GetProperties())
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

            return propertyKeys
                .Select(propertyInfoKvp => GremlinQuery
                    .Create("schema", queryProvider)
                    .AddStep<string>("propertyKey", propertyInfoKvp.Key)
                    .AddStep<string>(NativeTypeSteps
                        .TryGetValue(propertyInfoKvp.Value)
                        .IfNone(() => throw new InvalidOperationException($"No native type found for {propertyInfoKvp.Value}.")))
                    .AddStep<string>("single")
                    .AddStep<string>("ifNotExists")
                    .AddStep<string>("create"))
                .Concat(model.VertexLabels
                    .Where(vertexKvp => !vertexKvp.Key.GetTypeInfo().IsAbstract)
                    .Select(vertexKvp => vertexKvp.Key
                        .TryGetPartitionKeyExpression(model)
                        .Map(keyExpression => ((keyExpression as LambdaExpression)?.Body as MemberExpression)?.Member.Name)
                        .AsEnumerable()
                        .Aggregate(
                            GremlinQuery
                                .Create("schema", queryProvider)
                                .AddStep<string>("vertexLabel", vertexKvp.Value),
                            (closureQuery, property) => closureQuery.AddStep<string>("partitionKey", property))
                        .ConditionalAddStep(
                            vertexKvp.Key.GetProperties().Any(),
                            query => query.AddStep<string>(
                                "properties",
                                vertexKvp.Key
                                    .GetProperties()
                                    .Select(x => x.Name)
                                    .ToImmutableList<object>()))
                        .AddStep<string>("create")))
                .Concat(model.VertexLabels
                    .Select(vertexKvp => (
                        SchemaInfo: vertexKvp,
                        IndexProperties: model
                            .GetElementInfoHierarchy(vertexKvp.Key)
                            .SelectMany(x => model.SecondaryIndexes
                                .TryGetValue(x)
                                .AsEnumerable()
                                .SelectMany(y => y))
                            .Select(indexExpression => ((indexExpression as LambdaExpression)?.Body.StripConvert() as MemberExpression)?.Member.Name)
                            .ToImmutableList()))
                    .Where(tuple => !tuple.IndexProperties.IsEmpty)
                    .Select(tuple => tuple.IndexProperties
                        .Aggregate(
                            GremlinQuery
                                .Create("schema", queryProvider)
                                .AddStep<string>("vertexLabel", tuple.SchemaInfo.Value)
                                .AddStep<string>("index", Guid.NewGuid().ToString("N"))
                                .AddStep<string>("secondary"),
                            (closureQuery, indexProperty) => closureQuery.AddStep<string>("by", indexProperty))
                        .AddStep<string>("add"))
                    .Concat(model.EdgeLabels
                        .Where(edgeKvp => !edgeKvp.Key.GetTypeInfo().IsAbstract)
                        .Select(edgeKvp => model.Connections
                            .Where(tuple => tuple.Item2 == edgeKvp.Key)
                            .Where(x => !x.Item1.GetTypeInfo().IsAbstract && !x.Item2.GetTypeInfo().IsAbstract && !x.Item3.GetTypeInfo().IsAbstract)
                            .Aggregate(
                                GremlinQuery
                                    .Create("schema", queryProvider)
                                    .AddStep<string>("edgeLabel", edgeKvp.Value)
                                    .AddStep<string>("single")
                                    .ConditionalAddStep(
                                        edgeKvp.Key
                                            .GetProperties()
                                            .Any(),
                                        query => query.AddStep<string>(
                                            "properties",
                                            edgeKvp.Key
                                                .GetProperties()
                                                .Select(property => property.Name)
                                                .ToImmutableList<object>())),
                                (closureQuery, tuple) => closureQuery.AddStep<string>(
                                    "connection",
                                    model.VertexLabels.TryGetValue(tuple.Item1).IfNone(() => throw new InvalidOperationException(/* TODO: Message */ )),
                                    model.VertexLabels.TryGetValue(tuple.Item3).IfNone(() => throw new InvalidOperationException(/* TODO: Message */ ))))
                            .AddStep<string>("ifNotExists")
                            .AddStep<string>("create"))));
        }

        private static IGremlinQuery<TSource> ConditionalAddStep<TSource>(this IGremlinQuery<TSource> query, bool condition, Func<IGremlinQuery<TSource>, IGremlinQuery<TSource>> addStepFunction)
        {
            if (condition)
                return addStepFunction(query);

            return query;
        }

        private static Option<Expression> TryGetPartitionKeyExpression(this Type vertexTypeInfo, IDseGraphModel model)
        {
            return model.PrimaryKeys.TryGetValue(vertexTypeInfo)
                .Match(
                    _ => (Option<Expression>)_,
                    () =>
                    {
                        var baseType = vertexTypeInfo.GetTypeInfo().BaseType;

                        if (baseType != null && model.VertexLabels.ContainsKey(baseType))
                            return baseType.TryGetPartitionKeyExpression(model);

                        return Option<Expression>.None;
                    });
        }

        private static IEnumerable<Type> GetElementInfoHierarchy(this IGraphModel model, Type type)
        {
            do
            {
                yield return type;
                var baseType = type.GetTypeInfo().BaseType;

                type = null;

                if (model.VertexLabels.ContainsKey(baseType) || model.EdgeLabels.ContainsKey(baseType))
                    type = baseType;
            } while (type != null);
        }
    }
}