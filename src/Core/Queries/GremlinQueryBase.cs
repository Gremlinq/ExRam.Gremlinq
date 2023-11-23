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
        private static readonly Type[] ImplementedInterfaces = typeof(GremlinQuery<,,>).GetInterfaces();
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

        protected internal TTargetQuery CloneAs<TTargetQuery>(Traversal? maybeNewTraversal = null, IImmutableDictionary<StepLabel, LabelProjections>? maybeNewLabelProjections = null)
        {
            var targetQueryType = typeof(TTargetQuery);

            var maybeConstructor = QueryTypes.GetOrAdd(
                targetQueryType,
                static targetQueryType =>
                {
                    var typeArguments = Array.Empty<Type>();

                    if (targetQueryType.IsGenericType && targetQueryType.GetGenericTypeDefinition() == typeof(GremlinQuery<,,>))
                        typeArguments = targetQueryType.GetGenericArguments();
                    else
                    {
                        var queryDefinitionArguments = typeof(GremlinQuery<,,>).GetGenericArguments();
                        typeArguments = new Type[queryDefinitionArguments.Length];

                        if (targetQueryType.IsGenericType)
                        {
                            var genericTypeDefinition = targetQueryType.GetGenericTypeDefinition();

                            for (var i = 0; i < ImplementedInterfaces.Length; i++)
                            {
                                var implementedInterface = ImplementedInterfaces[i];

                                if (implementedInterface.IsGenericType && implementedInterface.GetGenericTypeDefinition() == genericTypeDefinition)
                                {
                                    var matchingInterfaceDefinitionArguments = implementedInterface.GetGenericArguments();

                                    for (var j = 0; j < queryDefinitionArguments.Length; j++)
                                    {
                                        for (var k = 0; k < matchingInterfaceDefinitionArguments.Length; k++)
                                        {
                                            if (matchingInterfaceDefinitionArguments[k] == queryDefinitionArguments[j])
                                            {
                                                typeArguments[j] = targetQueryType.GetGenericArguments()[k];

                                                break;
                                            }
                                        }
                                    }

                                    break;
                                }
                            }
                        }

                        for (var i = 0; i < queryDefinitionArguments.Length; i++)
                        {
                            if (typeArguments[i] == null)
                            {
                                typeArguments[i] = i == 1 && typeArguments[0].IsArray
                                    ? typeArguments[0].GetElementType()!
                                    : typeof(object);
                            }
                        }
                    }

                    return (QueryContinuation?)TryCreateQueryContinuationMethod
                        .MakeGenericMethod(typeArguments)
                        .Invoke(null, new object?[] { targetQueryType })!;
                });

            return maybeConstructor is { } constructor
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

        protected internal Traversal Steps { get; }
        protected internal IGremlinQueryEnvironment Environment { get; }
        protected internal IImmutableDictionary<object, object?> Metadata { get; }
        protected internal IImmutableDictionary<StepLabel, LabelProjections> LabelProjections { get; }
    }
}
