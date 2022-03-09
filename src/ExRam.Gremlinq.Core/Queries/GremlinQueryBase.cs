using System;
using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Reflection;
using ExRam.Gremlinq.Core.Projections;

namespace ExRam.Gremlinq.Core
{
    internal abstract class GremlinQueryBase
    {
        private delegate IGremlinQueryBase QueryContinuation(GremlinQueryBase existingQuery, Func<StepStack, StepStack>? stepStackTransformation, Func<Projection, Projection>? projectionTransformation, Func<IImmutableDictionary<StepLabel, Projection>, IImmutableDictionary<StepLabel, Projection>>? stepLabelProjectionsTransformation, Func<QueryFlags, QueryFlags>? queryFlagsTransformation);

        private static readonly ConcurrentDictionary<Type, QueryContinuation?> QueryTypes = new();
        private static readonly MethodInfo CreateFuncMethod = typeof(GremlinQueryBase).GetMethod(nameof(CreateFunc), BindingFlags.NonPublic | BindingFlags.Static)!;

        protected GremlinQueryBase(
            StepStack steps,
            Projection projection,
            IGremlinQueryEnvironment environment,
            IImmutableDictionary<StepLabel, Projection> stepLabelProjections,
            QueryFlags flags)
        {
            Steps = steps;
            Flags = flags;
            Projection = projection;
            Environment = environment;
            StepLabelProjections = stepLabelProjections;
        }

        protected internal TTargetQuery ContinueAs<TTargetQuery>(Func<StepStack, StepStack>? stepStackTransformation = null, Func<Projection, Projection>? projectionTransformation = null, Func<IImmutableDictionary<StepLabel, Projection>, IImmutableDictionary<StepLabel, Projection>>? stepLabelProjectionsTransformation = null, Func<QueryFlags, QueryFlags>? queryFlagsTransformation = null)
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
                ? (TTargetQuery)constructor(this, stepStackTransformation, projectionTransformation, stepLabelProjectionsTransformation, queryFlagsTransformation)
                : throw new NotSupportedException($"Cannot change the query type to {targetQueryType}.");
        }

        private static QueryContinuation? CreateFunc<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>(Type targetQueryType)
        {
            if (!targetQueryType.IsAssignableFrom(typeof(GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>)))
                return null;

            return (existingQuery, stepStackTransformation, projectionTransformation, stepLabelProjectionsTransformation, queryFlagsTransformation) =>
            {
                var newSteps = stepStackTransformation?.Invoke(existingQuery.Steps) ?? existingQuery.Steps;
                var newQueryFlags = queryFlagsTransformation?.Invoke(existingQuery.Flags) ?? existingQuery.Flags;
                var newProjection = projectionTransformation?.Invoke(existingQuery.Projection) ?? existingQuery.Projection;
                var newStepLabelProjections = stepLabelProjectionsTransformation?.Invoke(existingQuery.StepLabelProjections) ?? existingQuery.StepLabelProjections;

                if (targetQueryType.IsInstanceOfType(existingQuery) && newQueryFlags == existingQuery.Flags && stepStackTransformation == null && newProjection == existingQuery.Projection && newStepLabelProjections == existingQuery.StepLabelProjections)
                    return (IGremlinQueryBase)existingQuery;

                return new GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>(
                    existingQuery.Environment,
                    newSteps,
                    newProjection,
                    newStepLabelProjections,
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
    }
}
