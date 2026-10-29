#pragma warning disable IDE0003
// ReSharper disable ArrangeThisQualifier
using System.Collections.Immutable;
using System.Reflection;

namespace ExRam.Gremlinq.Core
{
    internal static class GremlinQueryFactory
    {
        internal delegate IGremlinQueryBase QueryContinuation(
            IGremlinQueryEnvironment environment,
            Traversal newTraversal,
            IImmutableDictionary<StepLabel, LabelProjections> newLabelProjections,
            IImmutableDictionary<object, object?> newMetadata);

        internal static readonly Type[] QueryGenericTypeDefinitionArguments = typeof(GremlinQuery<,,,>).GetGenericArguments();
        internal static readonly QueryContinuation ObjectQueryContinuation = CreateQueryContinuation<object, object, object, IGremlinQueryBase>();
        internal static readonly Type[] ImplementedInterfaces = typeof(GremlinQuery<,,,>).GetInterfaces().Append(typeof(GremlinQuery<,,,>)).ToArray();
        internal static readonly MethodInfo CreateQueryContinuationMethod = typeof(GremlinQueryFactory).GetMethod(nameof(CreateQueryContinuation), BindingFlags.NonPublic | BindingFlags.Static)!;

        public static TTargetQuery Create<TTargetQuery>(IGremlinQueryEnvironment environment, Traversal newTraversal, IImmutableDictionary<StepLabel, LabelProjections> newLabelProjections, IImmutableDictionary<object, object?> newMetadata) => GremlinQueryFactory<TTargetQuery>.Create(environment, newTraversal, newLabelProjections, newMetadata);

        private static QueryContinuation CreateQueryContinuation<T1, T2, T3, T4>() where T4 : IGremlinQueryBase => (environment, newTraversal, newLabelProjections, newMetadata) => new GremlinQuery<T1, T2, T3, T4>(
            environment,
            newTraversal,
            newLabelProjections,
            newMetadata);
    }

    // The continuation of a target query type, resolved once per type when the class is initialized: the type
    // arguments of GremlinQuery<,,,> are read off the requested interface, which GremlinQuery<,,,> implements.
    internal static class GremlinQueryFactory<TTargetQuery>
    {
        private static readonly GremlinQueryFactory.QueryContinuation? Continuation = Resolve();

        public static TTargetQuery Create(IGremlinQueryEnvironment environment, Traversal newTraversal, IImmutableDictionary<StepLabel, LabelProjections> newLabelProjections, IImmutableDictionary<object, object?> newMetadata) => Continuation is { } queryContinuation && queryContinuation(environment, newTraversal, newLabelProjections, newMetadata) is TTargetQuery newTargetQuery
            ? newTargetQuery
            : throw new NotSupportedException($"Cannot create a query of type {typeof(TTargetQuery)}.");

        private static GremlinQueryFactory.QueryContinuation? Resolve()
        {
            var requestedType = typeof(TTargetQuery);

            if (!requestedType.IsGenericType)
                return GremlinQueryFactory.ObjectQueryContinuation;

            var requestedTypeDefinition = requestedType.GetGenericTypeDefinition();
            var queryTypeArguments = new Type?[GremlinQueryFactory.QueryGenericTypeDefinitionArguments.Length];

            for (var i = 0; i < GremlinQueryFactory.ImplementedInterfaces.Length; i++)
            {
                if (GremlinQueryFactory.ImplementedInterfaces[i] is { IsGenericType: true } queryImplementedInterface && queryImplementedInterface.GetGenericTypeDefinition() == requestedTypeDefinition)
                {
                    var matchingImplementedInterfaceTypeArguments = queryImplementedInterface.GetGenericArguments();

                    for (var j = 0; j < GremlinQueryFactory.QueryGenericTypeDefinitionArguments.Length; j++)
                    {
                        for (var k = 0; k < matchingImplementedInterfaceTypeArguments.Length; k++)
                        {
                            if (matchingImplementedInterfaceTypeArguments[k] == GremlinQueryFactory.QueryGenericTypeDefinitionArguments[j])
                            {
                                queryTypeArguments[j] = requestedType.GetGenericArguments()[k];

                                break;
                            }
                        }

                        queryTypeArguments[j] ??= j == 1 && queryTypeArguments[0]!.IsArray
                            ? queryTypeArguments[0]!.GetElementType()!
                            : GremlinQueryFactory.QueryGenericTypeDefinitionArguments[j].GetGenericParameterConstraints().SingleOrDefault() ?? typeof(object);
                    }

                    return (GremlinQueryFactory.QueryContinuation?)GremlinQueryFactory.CreateQueryContinuationMethod
                        .MakeGenericMethod(queryTypeArguments!)
                        .Invoke(null, null);
                }
            }

            return null;
        }
    }
}
