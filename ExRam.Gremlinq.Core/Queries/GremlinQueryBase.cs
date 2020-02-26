using System;
using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;

namespace ExRam.Gremlinq.Core
{
    internal abstract class GremlinQueryBase
    {
        private static readonly ConcurrentDictionary<Type, Func<IImmutableStack<Step>, IGremlinQueryEnvironment, IImmutableDictionary<StepLabel, QuerySemantics>, bool, IGremlinQueryBase>> QueryTypes = new ConcurrentDictionary<Type, Func<IImmutableStack<Step>, IGremlinQueryEnvironment, IImmutableDictionary<StepLabel, QuerySemantics>, bool, IGremlinQueryBase>>();

        private static readonly Type[] SupportedInterfaceDefinitions = typeof(GremlinQuery<,,,,,>)
            .GetInterfaces()
            .Select(iface => iface.IsGenericType ? iface.GetGenericTypeDefinition() : iface)
            .ToArray();

        protected GremlinQueryBase(
            IImmutableStack<Step> steps,
            IGremlinQueryEnvironment environment,
            QuerySemantics semantics,
            IImmutableDictionary<StepLabel, QuerySemantics> stepLabelSemantics,
            bool surfaceVisible)
        {
            Steps = steps;
            Semantics = semantics;
            Environment = environment;
            SurfaceVisible = surfaceVisible;
            StepLabelSemantics = stepLabelSemantics;
        }

        protected TTargetQuery ChangeQueryType<TTargetQuery>()
        {
            var targetQueryType = typeof(TTargetQuery);

            if (targetQueryType.IsAssignableFrom(GetType()) && targetQueryType.IsGenericType && Semantics != QuerySemantics.None)
                return (TTargetQuery)(object)this;

            var genericTypeDef = targetQueryType.IsGenericType
                ? targetQueryType.GetGenericTypeDefinition()
                : targetQueryType;

            if (!SupportedInterfaceDefinitions.Contains(genericTypeDef))
                throw new NotSupportedException($"Cannot change the query type to {targetQueryType}.");

            var constructor = QueryTypes.GetOrAdd(
                targetQueryType,
                closureType =>
                {
                    var semantics = closureType.GetQuerySemantics();
                    var genericType = typeof(GremlinQuery<,,,,,>).MakeGenericType(
                        GetMatchingType(closureType, "TElement", "TVertex", "TEdge", "TProperty", "TArray"),
                        GetMatchingType(closureType, "TOutVertex", "TAdjacentVertex"),
                        GetMatchingType(closureType, "TInVertex"),
                        GetMatchingType(closureType, "TValue"),
                        GetMatchingType(closureType, "TMeta"),
                        GetMatchingType(closureType, "TQuery"));

                    var stepsParameter = Expression.Parameter(typeof(IImmutableStack<Step>));
                    var environmentParameter = Expression.Parameter(typeof(IGremlinQueryEnvironment));
                    var stepLabelSemanticsParameter = Expression.Parameter(typeof(IImmutableDictionary<StepLabel, QuerySemantics>));
                    var surfaceVisibleParameter = Expression.Parameter(typeof(bool));

                    return Expression
                        .Lambda<Func<IImmutableStack<Step>, IGremlinQueryEnvironment,  IImmutableDictionary<StepLabel, QuerySemantics>, bool, IGremlinQueryBase>>(
                            Expression.New(
                                genericType.GetConstructor(new[]
                                {
                                    stepsParameter.Type,
                                    environmentParameter.Type,
                                    typeof(QuerySemantics),
                                    stepLabelSemanticsParameter.Type,
                                    surfaceVisibleParameter.Type
                                }),
                                stepsParameter,
                                environmentParameter,
                                Expression.Constant(semantics, typeof(QuerySemantics)),
                                stepLabelSemanticsParameter,
                                surfaceVisibleParameter),
                            stepsParameter,
                            environmentParameter,
                            stepLabelSemanticsParameter,
                            surfaceVisibleParameter)
                        .Compile();
                });

            return (TTargetQuery)constructor(Steps, Environment, StepLabelSemantics, SurfaceVisible);
        }

        private static Type GetMatchingType(Type interfaceType, params string[] argumentNames)
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

            return typeof(object);
        }

        protected internal bool SurfaceVisible { get; }
        protected internal QuerySemantics Semantics { get; }
        protected internal IImmutableStack<Step> Steps { get; }
        protected internal IGremlinQueryEnvironment Environment { get; }
        protected internal IImmutableDictionary<StepLabel, QuerySemantics> StepLabelSemantics { get; }
    }
}
