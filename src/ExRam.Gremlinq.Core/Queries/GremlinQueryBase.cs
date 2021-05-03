using System;
using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;

namespace ExRam.Gremlinq.Core
{
    internal abstract class GremlinQueryBase
    {
        private static readonly MethodInfo CreateFuncMethod = typeof(GremlinQueryBase).GetMethod(nameof(CreateFunc), BindingFlags.NonPublic | BindingFlags.Static)!;
        private static readonly ConcurrentDictionary<Type, Func<GremlinQueryBase, QuerySemantics?, IGremlinQueryBase>> QueryTypes = new();

        protected GremlinQueryBase(
            StepStack steps,
            IGremlinQueryEnvironment environment,
            QuerySemantics semantics,
            IImmutableDictionary<StepLabel, QuerySemantics> stepLabelSemantics,
            QueryFlags flags)
        {
            Steps = steps;
            Semantics = semantics;
            Environment = environment;
            Flags = flags;
            StepLabelSemantics = stepLabelSemantics;
        }

        protected TTargetQuery ChangeQueryType<TTargetQuery>(QuerySemantics? forcedSemantics = null)
        {
            var targetQueryType = typeof(TTargetQuery);

            var constructor = QueryTypes.GetOrAdd(
                targetQueryType,
                closureType =>
                {
                    var semantics = closureType.TryGetQuerySemantics() ?? QuerySemantics.Value;

                    var elementType = GetMatchingType(closureType, "TElement", "TVertex", "TEdge", "TProperty", "TArray") ?? typeof(object);
                    var outVertexType = GetMatchingType(closureType, "TOutVertex", "TAdjacentVertex") ?? typeof(object);
                    var inVertexType = GetMatchingType(closureType, "TInVertex") ?? typeof(object);
                    var scalarType = GetMatchingType(closureType, "TValue", "TArrayItem");
                    var metaType = GetMatchingType(closureType, "TMeta") ?? typeof(object);
                    var queryType = GetMatchingType(closureType, "TOriginalQuery") ?? typeof(object);

                    return (Func<GremlinQueryBase, QuerySemantics?, IGremlinQueryBase>)CreateFuncMethod
                        .MakeGenericMethod(
                            elementType,
                            outVertexType,
                            inVertexType,
                            scalarType ?? (elementType.IsArray
                                ? elementType.GetElementType()!
                                : typeof(object)),
                            metaType,
                            queryType)
                        .Invoke(null, new object?[] { closureType, semantics })!;
                });

            return (TTargetQuery)constructor(this, forcedSemantics);
        }

        private static Func<GremlinQueryBase, QuerySemantics?, IGremlinQueryBase> CreateFunc<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>(Type targetQueryType, QuerySemantics determinedSemantics)
        {
            var genericTypeDef = targetQueryType.IsGenericType
                ? targetQueryType.GetGenericTypeDefinition()
                : targetQueryType;

            return (existingQuery, forcedSemantics) =>
            {
                var actualSemantics = forcedSemantics ?? determinedSemantics;

                if (targetQueryType.IsAssignableFrom(existingQuery.GetType()) && actualSemantics == existingQuery.Semantics)
                    return (IGremlinQueryBase)existingQuery;

                if (!targetQueryType.IsAssignableFrom(typeof(GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>)))
                    throw new NotSupportedException($"Cannot change the query type to {targetQueryType}.");

                return new GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>(
                    existingQuery.Steps,
                    existingQuery.Environment,
                    actualSemantics,
                    existingQuery.StepLabelSemantics,
                    existingQuery.Flags);
            };
        }

        private static Type? GetMatchingType(Type interfaceType, params string[] argumentNames)
        {
            if (interfaceType.IsGenericType)
            {
                var genericArguments = interfaceType.GetGenericArguments();
                var genericTypeDefinitionArguments = interfaceType.GetGenericTypeDefinition().GetGenericArguments();

                foreach (var argumentName in argumentNames)
                {
                    for (var i = 0; i < genericTypeDefinitionArguments.Length; i++)
                    {
                        if (genericTypeDefinitionArguments[i].ToString() == argumentName)
                            return genericArguments[i];
                    }
                }
            }

            return default;
        }

        protected internal StepStack Steps { get; }
        protected internal QueryFlags Flags { get; }
        protected internal QuerySemantics Semantics { get; }
        protected internal IGremlinQueryEnvironment Environment { get; }
        protected internal IImmutableDictionary<StepLabel, QuerySemantics> StepLabelSemantics { get; }
    }
}
