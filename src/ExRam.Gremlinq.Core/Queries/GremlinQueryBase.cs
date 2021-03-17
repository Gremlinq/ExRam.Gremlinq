using System;
using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

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

        protected TTargetQuery ChangeQueryType<TTargetQuery>(QuerySemantics? forcedSemantics = null)
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
                    var outVertexType = GetMatchingType(closureType, "TOutVertex", "TAdjacentVertex") ?? typeof(object);
                    var inVertexType = GetMatchingType(closureType, "TInVertex") ?? typeof(object);
                    var scalarType = GetMatchingType(closureType, "TValue", "TArrayItem");
                    var metaType = GetMatchingType(closureType, "TMeta") ?? typeof(object);
                    var queryType = GetMatchingType(closureType, "TOriginalQuery") ?? typeof(object);

                    var expression = (Expression<Func<IImmutableStack<Step>, IGremlinQueryEnvironment, QuerySemantics, IImmutableDictionary<StepLabel, QuerySemantics>, QueryFlags, IGremlinQueryBase>>)typeof(GremlinQueryBase)
                        .GetMethod(
                            nameof(GetCreateExpression),
                            BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.InvokeMethod)!
                        .MakeGenericMethod(
                            elementType,
                            outVertexType,
                            inVertexType,
                            scalarType ?? (elementType.IsArray
                                ? elementType.GetElementType()!
                                : typeof(object)),
                            metaType,
                            queryType)
                        .Invoke(null, new object?[] { semantics })!;

                    return expression!
                        .Compile();
                });

            return (TTargetQuery)constructor(Steps, Environment, forcedSemantics ?? QuerySemantics.Value, StepLabelSemantics, Flags);
        }

        private static Expression<Func<IImmutableStack<Step>, IGremlinQueryEnvironment, QuerySemantics, IImmutableDictionary<StepLabel, QuerySemantics>, QueryFlags, IGremlinQueryBase>> GetCreateExpression<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>(QuerySemantics? determinedSemantics)
        {
            return (steps, environment, existingSemantics, stepLabelSemantics, flags) => new GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>(
                steps,
                environment,
                determinedSemantics ?? existingSemantics,
                stepLabelSemantics,
                flags);
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
