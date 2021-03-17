using System;
using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;

namespace ExRam.Gremlinq.Core
{
    internal abstract class GremlinQueryBase
    {
        private static readonly ConcurrentDictionary<Type, Func<IImmutableStack<Step>, IGremlinQueryEnvironment, QuerySemantics, IImmutableDictionary<StepLabel, QuerySemantics>, QueryFlags, IGremlinQueryBase>> QueryTypes = new();

        private static readonly Type[] SupportedInterfaceDefinitions = typeof(GremlinQuery<,,,,,>)
            .GetInterfaces()
            .Select(iface => iface.IsGenericType ? iface.GetGenericTypeDefinition() : iface)
            .ToArray();

        protected GremlinQueryBase(
            IImmutableStack<Step> steps,
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

        protected TTargetQuery ChangeQueryType<TTargetQuery>(bool eraseSemantics = true)
        {
            var targetQueryType = typeof(TTargetQuery);

            if (targetQueryType.IsAssignableFrom(GetType()))
            {
                if (targetQueryType == GetType() || targetQueryType.IsGenericType && Semantics != QuerySemantics.Value)
                    return (TTargetQuery)(object)this;
            }

            var genericTypeDef = targetQueryType.IsGenericType
                ? targetQueryType.GetGenericTypeDefinition()
                : targetQueryType;

            if (!SupportedInterfaceDefinitions.Contains(genericTypeDef))
                throw new NotSupportedException($"Cannot change the query type to {targetQueryType}.");

            var constructor = QueryTypes.GetOrAdd(
                targetQueryType,
                closureType =>
                {
                    var semantics = closureType.TryGetQuerySemantics();

                    var elementType = GetMatchingType(closureType, "TElement", "TVertex", "TEdge", "TProperty", "TArray") ?? typeof(object);
                    var outVertexType = GetMatchingType(closureType, "TOutVertex", "TAdjacentVertex");
                    var inVertexType = GetMatchingType(closureType, "TInVertex");
                    var scalarType = GetMatchingType(closureType, "TValue", "TArrayItem");
                    var metaType = GetMatchingType(closureType, "TMeta");
                    var queryType = GetMatchingType(closureType, "TOriginalQuery");

                    var genericType = typeof(GremlinQuery<,,,,,>).MakeGenericType(
                        elementType,
                        outVertexType ?? typeof(object),
                        inVertexType ?? typeof(object),
                        scalarType ?? (elementType.IsArray
                            ? elementType.GetElementType()!
                            : typeof(object)),
                        metaType ?? typeof(object),
                        queryType ?? typeof(object));

                    var stepsParameter = Expression.Parameter(typeof(IImmutableStack<Step>));
                    var environmentParameter = Expression.Parameter(typeof(IGremlinQueryEnvironment));
                    var defaultSemanticsParameter = Expression.Parameter(typeof(QuerySemantics));
                    var stepLabelSemanticsParameter = Expression.Parameter(typeof(IImmutableDictionary<StepLabel, QuerySemantics>));
                    var flagsParameter = Expression.Parameter(typeof(QueryFlags));

                    return Expression
                        .Lambda<Func<IImmutableStack<Step>, IGremlinQueryEnvironment, QuerySemantics, IImmutableDictionary<StepLabel, QuerySemantics>, QueryFlags, IGremlinQueryBase>>(
                            Expression.New(
                                genericType.GetConstructor(new[]
                                {
                                    stepsParameter.Type,
                                    environmentParameter.Type,
                                    defaultSemanticsParameter.Type,
                                    stepLabelSemanticsParameter.Type,
                                    flagsParameter.Type
                                })!,
                                stepsParameter,
                                environmentParameter,
                                semantics != null
                                    ? (Expression)Expression.Constant(semantics.Value, typeof(QuerySemantics))
                                    : defaultSemanticsParameter,
                                stepLabelSemanticsParameter,
                                flagsParameter),
                            stepsParameter,
                            environmentParameter,
                            defaultSemanticsParameter,
                            stepLabelSemanticsParameter,
                            flagsParameter)
                        .Compile();
                });

            return (TTargetQuery)constructor(Steps, Environment, eraseSemantics ? QuerySemantics.Value : Semantics, StepLabelSemantics, Flags);
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

        protected internal QueryFlags Flags { get; }
        protected internal QuerySemantics Semantics { get; }
        protected internal IImmutableStack<Step> Steps { get; }
        protected internal IGremlinQueryEnvironment Environment { get; }
        protected internal IImmutableDictionary<StepLabel, QuerySemantics> StepLabelSemantics { get; }
    }
}
