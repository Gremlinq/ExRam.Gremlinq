using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Reflection;

namespace ExRam.Gremlinq.Core
{
    internal abstract class GremlinQueryBase
    {
        private delegate IGremlinQueryBase QueryContinuation(
            GremlinQueryBase existingQuery,
            Traversal? maybeNewTraversal,
            IImmutableDictionary<StepLabel, LabelProjections>? maybeNewLabelProjections);

        private static readonly ConcurrentDictionary<Type, QueryContinuation?> QueryTypes = new();
        private static readonly MethodInfo TryCreateQueryContinuationMethod = typeof(GremlinQueryBase).GetMethod(nameof(TryCreateQueryContinuation), BindingFlags.NonPublic | BindingFlags.Static)!;

        protected GremlinQueryBase(
            IGremlinQueryEnvironment environment,
            Traversal steps,
            IImmutableDictionary<StepLabel, LabelProjections> labelProjections,
            IImmutableDictionary<object, object?> metadata)
        {
            Steps = steps;
            Metadata = metadata;
            Environment = environment;
            LabelProjections = labelProjections;
        }

        public override string ToString() => $"GremlinQuery(Steps.Count: {Steps.Count})";

        protected internal TTargetQuery CloneAs<TTargetQuery>(
            Traversal? maybeNewTraversal = null,
            IImmutableDictionary<StepLabel, LabelProjections>? maybeNewLabelProjections = null)
        {
            var targetQueryType = typeof(TTargetQuery);

            var maybeConstructor = QueryTypes.GetOrAdd(
                targetQueryType,
                static closureType =>
                {
                    if (closureType.IsGenericType && closureType.GetGenericTypeDefinition() == typeof(GremlinQuery<,,>))
                    {
                        return (QueryContinuation?)TryCreateQueryContinuationMethod
                            .MakeGenericMethod(closureType.GetGenericArguments())
                            .Invoke(null, new object?[] { closureType })!;
                    }

                    return (QueryContinuation?)TryCreateQueryContinuationMethod
                        .MakeGenericMethod(GetMatchingQueryTypeArguments(closureType))
                        .Invoke(null, new object?[] { closureType })!;
                });

            return (maybeConstructor is { } constructor)
                ? (TTargetQuery)constructor(this, maybeNewTraversal, maybeNewLabelProjections)
                : throw new NotSupportedException($"Cannot create a query of type {targetQueryType}.");
        }

        private static QueryContinuation? TryCreateQueryContinuation<T1, T2, T3>(Type targetQueryType)
        {
            if (!targetQueryType.IsAssignableFrom(typeof(GremlinQuery<T1, T2, T3>)))
                return null;

            return (existingQuery, maybeNewTraversal, maybeNewLabelProjections) =>
            {
                if (maybeNewTraversal == null && maybeNewLabelProjections == null && targetQueryType.IsInstanceOfType(existingQuery))
                    return (IGremlinQueryBase)existingQuery;

                return new GremlinQuery<T1, T2, T3>(
                    existingQuery.Environment,
                    maybeNewTraversal ?? existingQuery.Steps,
                    maybeNewLabelProjections ?? existingQuery.LabelProjections,
                    existingQuery.Metadata);
            };
        }

        private static Type[] GetMatchingQueryTypeArguments(Type interfaceType)
        {
            var queryDefinitionArguments = typeof(GremlinQuery<,,>).GetGenericArguments();
            var types = new Type[queryDefinitionArguments.Length];

            if (interfaceType.IsGenericType)
            {
                var genericTypeDefinition = interfaceType.GetGenericTypeDefinition();

                var maybeMatchingInterfaceDefinition = typeof(GremlinQuery<,,>)
                    .GetInterfaces()
                    .FirstOrDefault(iface => iface.IsGenericType && iface.GetGenericTypeDefinition() == genericTypeDefinition);

                if (maybeMatchingInterfaceDefinition is { } matchingInterfaceDefinition)
                {
                    var matchingInterfaceDefinitionArguments = matchingInterfaceDefinition.GetGenericArguments();

                    for (var i = 0; i < queryDefinitionArguments.Length; i++)
                    {
                        for (var j = 0; j < matchingInterfaceDefinitionArguments.Length; j++)
                        {
                            if (matchingInterfaceDefinitionArguments[j] == queryDefinitionArguments[i])
                            {
                                types[i] = interfaceType.GetGenericArguments()[j];
                                break;
                            }
                        }
                    }
                }
            }

            for (var i = 0; i < queryDefinitionArguments.Length; i++)
            {
                if (types[i] == null)
                {
                    types[i] = i == 1 && types[0].IsArray
                        ? types[0].GetElementType()!
                        : typeof(object);
                }
            }

            return types;
        }

        protected internal Traversal Steps { get; }
        protected internal IGremlinQueryEnvironment Environment { get; }
        protected internal IImmutableDictionary<object, object?> Metadata { get; }
        protected internal IImmutableDictionary<StepLabel, LabelProjections> LabelProjections { get; }
    }
}
