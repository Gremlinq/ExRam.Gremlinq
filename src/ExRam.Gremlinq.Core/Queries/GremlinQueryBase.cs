using System;
using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Reflection;
using ExRam.Gremlinq.Core.Projections;

namespace ExRam.Gremlinq.Core
{
    internal abstract class GremlinQueryBase
    {
        private delegate IGremlinQueryBase QueryContinuation(
            GremlinQueryBase existingQuery,
            Traversal? maybeNewTraversal,
            IImmutableDictionary<StepLabel, Projection>? maybeNewStepLabelProjections,
            IImmutableDictionary<StepLabel, Projection>? maybeNewSideEffectLabelProjections,
            QueryFlags? maybeNewQueryFlags);

        private static readonly ConcurrentDictionary<Type, QueryContinuation?> QueryTypes = new();
        private static readonly MethodInfo CreateFuncMethod = typeof(GremlinQueryBase).GetMethod(nameof(CreateFunc), BindingFlags.NonPublic | BindingFlags.Static)!;

        protected GremlinQueryBase(
            IGremlinQueryEnvironment environment,
            Traversal steps,
            IImmutableDictionary<StepLabel, Projection> stepLabelProjections,
            IImmutableDictionary<StepLabel, Projection> sideEffectProjections,
            QueryFlags flags)
        {
            Steps = steps;
            Flags = flags;
            Environment = environment;
            StepLabelProjections = stepLabelProjections;
            SideEffectLabelProjections = sideEffectProjections;
        }

        public override string ToString() => $"GremlinQuery(Steps.Count: {Steps.Count})";

        protected internal TTargetQuery CloneAs<TTargetQuery>(
            Traversal? maybeNewTraversal = null,
            IImmutableDictionary<StepLabel, Projection>? maybeNewStepLabelProjections = null,
            IImmutableDictionary<StepLabel, Projection>? maybeNewSideEffectLabelProjections = null,
            QueryFlags? maybeNewQueryFlags = null)
        {
            var targetQueryType = typeof(TTargetQuery);

            var maybeConstructor = QueryTypes.GetOrAdd(
                targetQueryType,
                static closureType =>
                {
                    if (closureType.IsGenericType && closureType.GetGenericTypeDefinition() == typeof(GremlinQuery<,,,,,>))
                    {
                        return (QueryContinuation?)CreateFuncMethod
                            .MakeGenericMethod(
                                closureType.GetGenericArguments())
                            .Invoke(null, new object?[] { closureType })!;
                    }

                    var elementType = GetMatchingType(closureType, "TElement", "TVertex", "TEdge", "TProperty", "TArray") ?? typeof(object);
                    var outVertexType = GetMatchingType(closureType, "TOutVertex", "TAdjacentVertex") ?? typeof(object);
                    var inVertexType = GetMatchingType(closureType, "TInVertex") ?? typeof(object);
                    var scalarType = GetMatchingType(closureType, "TValue", "TArrayItem");
                    var metaType = GetMatchingType(closureType, "TMeta") ?? typeof(object);
                    var queryType = GetMatchingType(closureType, "TOriginalQuery") ?? typeof(object);

                    return (QueryContinuation?)CreateFuncMethod
                        .MakeGenericMethod(
                            elementType,
                            outVertexType,
                            inVertexType,
                            scalarType ?? (elementType.IsArray
                                ? elementType.GetElementType()!
                                : typeof(object)),
                            metaType,
                            queryType)
                        .Invoke(null, new object?[] { closureType })!;
                });

            return (maybeConstructor is { } constructor)
                ? (TTargetQuery)constructor(this, maybeNewTraversal, maybeNewStepLabelProjections, maybeNewSideEffectLabelProjections, maybeNewQueryFlags)
                : throw new NotSupportedException($"Cannot change the query type to {targetQueryType}.");
        }

        private static QueryContinuation? CreateFunc<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>(Type targetQueryType)
        {
            if (!targetQueryType.IsAssignableFrom(typeof(GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>)))
                return null;

            return (existingQuery, maybeNewTraversal, maybeNewStepLabelProjections, maybeNewSideEffectLabelProjections, maybeNewQueryFlags) =>
            {
                var newTraversal = maybeNewTraversal ?? existingQuery.Steps;
                var newQueryFlags = maybeNewQueryFlags ?? existingQuery.Flags;
                var newStepLabelProjections = maybeNewStepLabelProjections ?? existingQuery.StepLabelProjections;
                var newSideEffectLabelProjections = maybeNewSideEffectLabelProjections ?? existingQuery.SideEffectLabelProjections;

                if (targetQueryType.IsInstanceOfType(existingQuery) && newQueryFlags == existingQuery.Flags && maybeNewTraversal == null && newStepLabelProjections == existingQuery.StepLabelProjections && newSideEffectLabelProjections == existingQuery.SideEffectLabelProjections)
                    return (IGremlinQueryBase)existingQuery;

                return new GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>(
                    existingQuery.Environment,
                    newTraversal,
                    newStepLabelProjections,
                    newSideEffectLabelProjections,
                    newQueryFlags);
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

        protected internal Traversal Steps { get; }
        protected internal QueryFlags Flags { get; }
        protected internal IGremlinQueryEnvironment Environment { get; }
        protected internal IImmutableDictionary<StepLabel, Projection> StepLabelProjections { get; }
        protected internal IImmutableDictionary<StepLabel, Projection> SideEffectLabelProjections { get; }
    }
}
