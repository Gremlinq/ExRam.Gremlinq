using System;
using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Reflection;
using ExRam.Gremlinq.Core.Projections;

namespace ExRam.Gremlinq.Core
{
    internal abstract class GremlinQueryBase
    {
        private delegate IGremlinQueryBase QueryContinuation(GremlinQueryBase existingQuery, Func<IGremlinQueryEnvironment, IGremlinQueryEnvironment>? maybeEnvironmentTransformation, Func<StepStack, StepStack>? maybeStepStackTransformation, Func<Projection, Projection>? maybeProjectionTransformation, Func<IImmutableDictionary<StepLabel, Projection>, IImmutableDictionary<StepLabel, Projection>>? maybeStepLabelProjectionsTransformation, Func<QueryFlags, QueryFlags>? maybeQueryFlagsTransformation);

        private static readonly ConcurrentDictionary<Type, QueryContinuation?> QueryTypes = new();
        private static readonly MethodInfo CreateFuncMethod = typeof(GremlinQueryBase).GetMethod(nameof(CreateFunc), BindingFlags.NonPublic | BindingFlags.Static)!;

        protected GremlinQueryBase(
            IGremlinQueryEnvironment environment,
            StepStack steps,
            Projection projection,
            IImmutableDictionary<StepLabel, Projection> stepLabelProjections,
            IImmutableDictionary<StepLabel, Projection> sideEffectProjections,
            QueryFlags flags)
        {
            Steps = steps;
            Flags = flags;
            Projection = projection;
            Environment = environment;
            StepLabelProjections = stepLabelProjections;
            SideEffectProjections = sideEffectProjections;
        }

        protected internal TTargetQuery CloneAs<TTargetQuery>(Func<IGremlinQueryEnvironment, IGremlinQueryEnvironment>? maybeEnvironmentTransformation = null, Func<StepStack, StepStack>? maybeStepStackTransformation = null, Func<Projection, Projection>? maybeProjectionTransformation = null, Func<IImmutableDictionary<StepLabel, Projection>, IImmutableDictionary<StepLabel, Projection>>? maybeStepLabelProjectionsTransformation = null, Func<QueryFlags, QueryFlags>? maybeQueryFlagsTransformation = null)
        {
            var targetQueryType = typeof(TTargetQuery);

            var maybeConstructor = QueryTypes.GetOrAdd(
                targetQueryType,
                closureType =>
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
                ? (TTargetQuery)constructor(this, maybeEnvironmentTransformation, maybeStepStackTransformation, maybeProjectionTransformation, maybeStepLabelProjectionsTransformation, maybeQueryFlagsTransformation)
                : throw new NotSupportedException($"Cannot change the query type to {targetQueryType}.");
        }

        private static QueryContinuation? CreateFunc<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>(Type targetQueryType)
        {
            if (!targetQueryType.IsAssignableFrom(typeof(GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>)))
                return null;

            return (existingQuery, maybeEnvironmentTransformation, maybeStepStackTransformation, maybeProjectionTransformation, maybeStepLabelProjectionsTransformation, maybeQueryFlagsTransformation) =>
            {
                var newEnvironment = maybeEnvironmentTransformation?.Invoke(existingQuery.Environment) ?? existingQuery.Environment;
                var newSteps = maybeStepStackTransformation?.Invoke(existingQuery.Steps) ?? existingQuery.Steps;
                var newQueryFlags = maybeQueryFlagsTransformation?.Invoke(existingQuery.Flags) ?? existingQuery.Flags;
                var newProjection = maybeProjectionTransformation?.Invoke(existingQuery.Projection) ?? existingQuery.Projection;
                var newStepLabelProjections = maybeStepLabelProjectionsTransformation?.Invoke(existingQuery.StepLabelProjections) ?? existingQuery.StepLabelProjections;

                if (targetQueryType.IsInstanceOfType(existingQuery) && newQueryFlags == existingQuery.Flags && newEnvironment == existingQuery.Environment && maybeStepStackTransformation == null && newProjection == existingQuery.Projection && newStepLabelProjections == existingQuery.StepLabelProjections)
                    return (IGremlinQueryBase)existingQuery;

                return new GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>(
                    newEnvironment,
                    newSteps,
                    newProjection,
                    newStepLabelProjections,
                    existingQuery.SideEffectProjections,
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

        protected internal StepStack Steps { get; }
        protected internal QueryFlags Flags { get; }
        protected internal Projection Projection { get; }
        protected internal IGremlinQueryEnvironment Environment { get; }
        protected internal IImmutableDictionary<StepLabel, Projection> StepLabelProjections { get; }
        protected internal IImmutableDictionary<StepLabel, Projection> SideEffectProjections { get; }
    }
}
