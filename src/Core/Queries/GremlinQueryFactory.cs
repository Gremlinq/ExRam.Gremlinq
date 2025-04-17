#pragma warning disable IDE0003
// ReSharper disable ArrangeThisQualifier
using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Reflection;

namespace ExRam.Gremlinq.Core
{
    internal static class GremlinQueryFactory
    {
        private delegate IGremlinQueryBase QueryContinuation(
            IGremlinQueryEnvironment environment,
            Traversal newTraversal,
            IImmutableDictionary<StepLabel, LabelProjections> newLabelProjections,
            IImmutableDictionary<object, object?> newMetadata);

        private static readonly ConcurrentDictionary<Type, QueryContinuation> QueryContinuations = new();
        private static readonly Type[] QueryGenericTypeDefinitionArguments = typeof(GremlinQuery<,,,>).GetGenericArguments();
        private static readonly QueryContinuation ObjectQueryContinuation = CreateQueryContinuation<object, object, object, IGremlinQueryBase>();
        private static readonly Type[] ImplementedInterfaces = typeof(GremlinQuery<,,,>).GetInterfaces().Append(typeof(GremlinQuery<,,,>)).ToArray();
        private static readonly MethodInfo TryCreateQueryContinuationMethod = typeof(GremlinQueryFactory).GetMethod(nameof(CreateQueryContinuation), BindingFlags.NonPublic | BindingFlags.Static)!;

        public static TTargetQuery CloneAs<TTargetQuery>(IGremlinQueryEnvironment environment, Traversal newTraversal, IImmutableDictionary<StepLabel, LabelProjections> newLabelProjections, IImmutableDictionary<object, object?> newMetadata)
        {
            var queryFactory = typeof(TTargetQuery).IsGenericType
                ? QueryContinuations.GetOrAdd(
                    typeof(TTargetQuery),
                    static requestedType =>
                    {
                        var requestedTypeDefinition = requestedType.GetGenericTypeDefinition();
                        var queryTypeArguments = new Type?[QueryGenericTypeDefinitionArguments.Length];

                        for (var i = 0; i < ImplementedInterfaces.Length; i++)
                        {
                            if (ImplementedInterfaces[i] is { IsGenericType: true } queryImplementedInterface && queryImplementedInterface.GetGenericTypeDefinition() == requestedTypeDefinition)
                            {
                                var matchingImplementedInterfaceTypeArguments = queryImplementedInterface.GetGenericArguments();

                                for (var j = 0; j < QueryGenericTypeDefinitionArguments.Length; j++)
                                {
                                    for (var k = 0; k < matchingImplementedInterfaceTypeArguments.Length; k++)
                                    {
                                        if (matchingImplementedInterfaceTypeArguments[k] == QueryGenericTypeDefinitionArguments[j])
                                        {
                                            queryTypeArguments[j] = requestedType.GetGenericArguments()[k];

                                            break;
                                        }
                                    }

                                    queryTypeArguments[j] ??= j == 1 && queryTypeArguments[0]!.IsArray
                                        ? queryTypeArguments[0]!.GetElementType()!
                                        : QueryGenericTypeDefinitionArguments[j].GetGenericParameterConstraints().SingleOrDefault() ?? typeof(object);
                                }


                                return (QueryContinuation?)TryCreateQueryContinuationMethod
                                    .MakeGenericMethod(queryTypeArguments!)
                                    .Invoke(null, null)!;
                            }
                        }

                        throw new NotSupportedException();
                    })
                : ObjectQueryContinuation;

            return queryFactory(environment, newTraversal, newLabelProjections, newMetadata) is TTargetQuery newTargetQuery
                ? newTargetQuery
                : throw new NotSupportedException($"Cannot create a query of type {typeof(TTargetQuery)}.");
        }

        private static QueryContinuation CreateQueryContinuation<T1, T2, T3, T4>() where T4 : IGremlinQueryBase => (environment, newTraversal, newLabelProjections, newMetadata) => new GremlinQuery<T1, T2, T3, T4>(
            environment,
            newTraversal,
            newLabelProjections,
            newMetadata);
    }
}
