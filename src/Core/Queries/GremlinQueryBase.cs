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

                    var t1 = TryGetMatchingType(closureType, "TElement", "TVertex", "TEdge", "TProperty", "TArray") ?? typeof(object);
                    var t2 = TryGetMatchingType(closureType, "TOutVertex", "TAdjacentVertex", "TArrayItem", "TValue") ?? (t1.IsArray ? t1.GetElementType()! : typeof(object));
                    var t3 = TryGetMatchingType(closureType, "TInVertex", "TOriginalQuery", "TMeta") ?? typeof(object);

                    return (QueryContinuation?)TryCreateQueryContinuationMethod
                        .MakeGenericMethod(t1, t2, t3)
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

        private static Type? TryGetMatchingType(Type interfaceType, params string[] argumentNames)
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
        protected internal IGremlinQueryEnvironment Environment { get; }
        protected internal IImmutableDictionary<object, object?> Metadata { get; }
        protected internal IImmutableDictionary<StepLabel, LabelProjections> LabelProjections { get; }
    }
}
