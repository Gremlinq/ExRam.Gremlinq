using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using LanguageExt;

namespace ExRam.Gremlinq.Core
{
    public static class GremlinQuerySerializer
    {
        private sealed class GremlinQuerySerializerImpl : IGremlinQuerySerializer
        {
            private sealed class Recurse
            {
                private readonly Action<object> _recurse;
                private readonly ISerializedGremlinQueryAssembler _assembler;
                private readonly ConcurrentDictionary<Type, AtomSerializer<object>> _dict;

                public Recurse(ConcurrentDictionary<Type, AtomSerializer<object>> dict, ISerializedGremlinQueryAssembler assembler)
                {
                    _dict = dict;
                    _assembler = assembler;
                    _recurse = RecurseImpl;
                }

                public void RecurseImpl(object o)
                {
                    if (o is IGremlinQuery query)
                    {
                        var steps = query.AsAdmin().Steps.HandleAnonymousQueries();
                        if (query.AsAdmin().Environment.Options.GetValue(GremlinQuerySerializer.WorkaroundTinkerpop2112))
                            steps = steps.WorkaroundTINKERPOP_2112();

                        foreach (var step in steps)
                        {
                            RecurseImpl(step);
                        }
                    }
                    else
                    {
                        var action = GetSerializer(o.GetType());

                        action(o, _assembler, _ => throw new NotImplementedException(), _recurse);
                    }
                }

                private AtomSerializer<object> GetSerializer(Type type)
                {
                    return _dict
                        .GetOrAdd(
                            type,
                            closureType =>
                            {
                                if (closureType.BaseType is Type baseType)
                                    return GetSerializer(baseType);

                                return (atom, assembler, baseSerializer, recurse) => assembler.Constant(atom);
                            });
                }
            }

            private readonly IImmutableDictionary<Type, AtomSerializer<object>> _dict;
            private readonly ISerializedGremlinQueryAssemblerFactory _assemblerFactory;
            private readonly Lazy<ConcurrentDictionary<Type, AtomSerializer<object>>> _lazyFastDict;

            public GremlinQuerySerializerImpl(IImmutableDictionary<Type, AtomSerializer<object>> dict, ISerializedGremlinQueryAssemblerFactory assemblerFactory)
            {
                _dict = dict;
                _assemblerFactory = assemblerFactory;
                _lazyFastDict = new Lazy<ConcurrentDictionary<Type, AtomSerializer<object>>>(
                    () => new ConcurrentDictionary<Type, AtomSerializer<object>>(dict.ToDictionary(kvp => kvp.Key, kvp => kvp.Value)),
                    LazyThreadSafetyMode.PublicationOnly);
            }

            public object Serialize(IGremlinQuery query)
            {
                var assembler = _assemblerFactory.Create();

                new Recurse(_lazyFastDict.Value, assembler)
                    .RecurseImpl(query);

                return assembler.Assemble();
            }

            public IGremlinQuerySerializer OverrideAtomSerializer<TAtom>(AtomSerializer<TAtom> atomSerializer)
            {
                return new GremlinQuerySerializerImpl(
                    _dict
                        .TryGetValue(typeof(TAtom))
                        .Match(
                            existingAtomSerializer => _dict.SetItem(typeof(TAtom), (atom, assembler, baseSerializer, recurse) => atomSerializer((TAtom)atom, assembler, _ => existingAtomSerializer(_, assembler, baseSerializer, recurse), recurse)),
                            () => _dict.SetItem(typeof(TAtom), (atom, assembler, baseSerializer, recurse) => atomSerializer((TAtom)atom, assembler, _ => throw new NotImplementedException(), recurse))),
                    _assemblerFactory);
            }

            public IGremlinQuerySerializer ConfigureAssemblerFactory(Func<ISerializedGremlinQueryAssemblerFactory, ISerializedGremlinQueryAssemblerFactory> transformation)
            {
                return new GremlinQuerySerializerImpl(
                    _dict,
                    transformation(_assemblerFactory));
            }
        }

        private sealed class InvalidGremlinQuerySerializer : IGremlinQuerySerializer
        {
            public IGremlinQuerySerializer OverrideAtomSerializer<TAtom>(AtomSerializer<TAtom> atomSerializer)
            {
                throw new InvalidOperationException($"{nameof(OverrideAtomSerializer)} must not be called on {nameof(GremlinQuerySerializer)}.{nameof(Invalid)}. If you are getting this exception while executing a query, configure a proper {nameof(IGremlinQuerySerializer)} on your {nameof(GremlinQuerySource)}.");
            }

            public IGremlinQuerySerializer ConfigureAssemblerFactory(Func<ISerializedGremlinQueryAssemblerFactory, ISerializedGremlinQueryAssemblerFactory> transformation)
            {
                throw new InvalidOperationException($"{nameof(ConfigureAssemblerFactory)} must not be called on {nameof(GremlinQuerySerializer)}.{nameof(Invalid)}. If you are getting this exception while executing a query, configure a proper {nameof(IGremlinQuerySerializer)} on your {nameof(GremlinQuerySource)}.");
            }

            public object Serialize(IGremlinQuery query)
            {
                throw new InvalidOperationException($"{nameof(Serialize)} must not be called on {nameof(GremlinQuerySerializer)}.{nameof(Invalid)}. If you are getting this exception while executing a query, configure a proper {nameof(IGremlinQuerySerializer)} on your {nameof(GremlinQuerySource)}.");
            }
        }

        public static readonly GremlinqOption<bool> WorkaroundTinkerpop2112 = new GremlinqOption<bool>(false);

        public static readonly IGremlinQuerySerializer Invalid = new InvalidGremlinQuerySerializer();

        public static readonly IGremlinQuerySerializer Unit = new GremlinQuerySerializerImpl(ImmutableDictionary<Type, AtomSerializer<object>>.Empty, SerializedGremlinQueryAssemblerFactory.Unit);

        public static readonly IGremlinQuerySerializer Groovy = GremlinQuerySerializer
            .Unit
            .UseDefaultGremlinStepSerializationHandlers()
            .UseGroovy();

        public static IGremlinQuerySerializer UseGroovy(this IGremlinQuerySerializer builder)
        {
            return builder
                .ConfigureAssemblerFactory(_ => SerializedGremlinQueryAssemblerFactory.Groovy);
        }

        public static IGremlinQuerySerializer UseDefaultGremlinStepSerializationHandlers(this IGremlinQuerySerializer builder)
        {
            return builder
                .OverrideAtomSerializer<HasNotStep>((step, assembler, overridden, recurse) => assembler.Method("hasNot", step.Key, recurse))
                .OverrideAtomSerializer<ChooseOptionTraversalStep>((step, assembler, overridden, recurse) => assembler.Method("choose", step.Traversal, recurse))
                .OverrideAtomSerializer<OptionTraversalStep>((step, assembler, overridden, recurse) => assembler.Method("option", step.Guard, step.OptionTraversal, recurse))
                .OverrideAtomSerializer<WithoutStrategiesStep>((step, assembler, overridden, recurse) =>
                {
                    assembler.OpenMethod("withoutStrategies");

                    foreach (var className in step.ClassNames)
                    {
                        assembler.StartParameter();
                        assembler.Identifier(className);
                        assembler.EndParameter();
                    }

                    assembler.CloseMethod();
                })
                .OverrideAtomSerializer<HasStep>((step, assembler, overridden, recurse) =>
                {
                    if (step.Value is P p1 && p1.EqualsConstant(false))
                        recurse(NoneStep.Instance);
                    else
                    {
                        var stepName = "has";
                        var argument = step.Value;

                        if (argument is P p2)
                        {
                            if (p2 is P.SingleArgumentP singleArgumentP)
                            {
                                if (singleArgumentP.Argument == null)
                                {
                                    if (p2 is P.Eq)
                                        stepName = "hasNot";
                                    else if (p2 is P.Neq)
                                        argument = null;
                                }

                                if (p2 is P.Eq)
                                    argument = singleArgumentP.Argument;
                            }
                            else if (p2 == P.True)
                                argument = null;
                        }

                        if (argument != null)
                            assembler.Method(stepName, step.Key, argument, recurse);
                        else
                            assembler.Method(stepName, step.Key, recurse);
                    }
                })
                .OverrideAtomSerializer<RepeatStep>((step, assembler, overridden, recurse) => assembler.Method("repeat", step.Traversal, recurse))
                .OverrideAtomSerializer<SideEffectStep>((step, assembler, overridden, recurse) => assembler.Method("sideEffect", step.Traversal, recurse))
                .OverrideAtomSerializer<ToTraversalStep>((step, assembler, overridden, recurse) => assembler.Method("to", step.Traversal, recurse))
                .OverrideAtomSerializer<UnionStep>((step, assembler, overridden, recurse) => assembler.Method("union", step.Traversals, recurse))
                .OverrideAtomSerializer<UntilStep>((step, assembler, overridden, recurse) => assembler.Method("until", step.Traversal, recurse))
                .OverrideAtomSerializer<ValuesStep>((step, assembler, overridden, recurse) => assembler.Method("values", step.Keys, recurse))
                .OverrideAtomSerializer<VerticesStep>((step, assembler, overridden, recurse) => assembler.Method("vertices", step.Traversal, recurse))
                .OverrideAtomSerializer<WhereTraversalStep>((step, assembler, overridden, recurse) => assembler.Method("where", step.Traversal, recurse))
                .OverrideAtomSerializer<WithStrategiesStep>((step, assembler, overridden, recurse) => assembler.Method("withStrategies", step.Traversal, recurse))
                .OverrideAtomSerializer<IdStep>((step, assembler, overridden, recurse) => assembler.Method("id"))
                .OverrideAtomSerializer<BarrierStep>((step, assembler, overridden, recurse) => assembler.Method("barrier"))
                .OverrideAtomSerializer<OrderStep>((step, assembler, overridden, recurse) => assembler.Method("order"))
                .OverrideAtomSerializer<CreateStep>((step, assembler, overridden, recurse) => assembler.Method("create"))
                .OverrideAtomSerializer<UnfoldStep>((step, assembler, overridden, recurse) => assembler.Method("unfold"))
                .OverrideAtomSerializer<IdentityStep>((step, assembler, overridden, recurse) => assembler.Method("identity"))
                .OverrideAtomSerializer<EmitStep>((step, assembler, overridden, recurse) => assembler.Method("emit"))
                .OverrideAtomSerializer<DedupStep>((step, assembler, overridden, recurse) => assembler.Method("dedup"))
                .OverrideAtomSerializer<OutVStep>((step, assembler, overridden, recurse) => assembler.Method("outV"))
                .OverrideAtomSerializer<OtherVStep>((step, assembler, overridden, recurse) => assembler.Method("otherV"))
                .OverrideAtomSerializer<InVStep>((step, assembler, overridden, recurse) => assembler.Method("inV"))
                .OverrideAtomSerializer<BothVStep>((step, assembler, overridden, recurse) => assembler.Method("bothV"))
                .OverrideAtomSerializer<DropStep>((step, assembler, overridden, recurse) => assembler.Method("drop"))
                .OverrideAtomSerializer<FoldStep>((step, assembler, overridden, recurse) => assembler.Method("fold"))
                .OverrideAtomSerializer<ExplainStep>((step, assembler, overridden, recurse) => assembler.Method("explain"))
                .OverrideAtomSerializer<ProfileStep>((step, assembler, overridden, recurse) => assembler.Method("profile"))
                .OverrideAtomSerializer<CountStep>((step, assembler, overridden, recurse) =>
                {
                    if (step.Scope.Equals(Scope.Local))
                        assembler.Method("count", step.Scope, recurse);
                    else
                        assembler.Method("count");
                })
                .OverrideAtomSerializer<BuildStep>((step, assembler, overridden, recurse) => assembler.Method("build"))
                .OverrideAtomSerializer<SumStep>((step, assembler, overridden, recurse) => assembler.Method("sum", step.Scope, recurse))
                .OverrideAtomSerializer<TailStep>((step, assembler, overridden, recurse) =>
                {
                    if (step.Scope.Equals(Scope.Local))
                        assembler.Method("tail", step.Scope, step.Count, recurse);
                    else
                        assembler.Method("tail", step.Count, recurse);
                })
                .OverrideAtomSerializer<SelectStep>((step, assembler, overridden, recurse) => assembler.Method("select", step.StepLabels, recurse))
                .OverrideAtomSerializer<AsStep>((step, assembler, overridden, recurse) => assembler.Method("as", step.StepLabels, recurse))
                .OverrideAtomSerializer<FromLabelStep>((step, assembler, overridden, recurse) => assembler.Method("from", step.StepLabel, recurse))
                .OverrideAtomSerializer<ToLabelStep>((step, assembler, overridden, recurse) => assembler.Method("to", step.StepLabel, recurse))
                .OverrideAtomSerializer<TimesStep>((step, assembler, overridden, recurse) => assembler.Method("times", step.Count, recurse))
                .OverrideAtomSerializer<FilterStep>((step, assembler, overridden, recurse) => assembler.Method("filter", step.Lambda, recurse))
                .OverrideAtomSerializer<AggregateStep>((step, assembler, overridden, recurse) => assembler.Method("aggregate", step.StepLabel, recurse))
                .OverrideAtomSerializer<WherePredicateStep>((step, assembler, overridden, recurse) => assembler.Method("where", step.Predicate, recurse))
                .OverrideAtomSerializer<ByLambdaStep>((step, assembler, overridden, recurse) => assembler.Method("by", step.Lambda, recurse))
                .OverrideAtomSerializer<SkipStep>((step, assembler, overridden, recurse) => assembler.Method("skip", step.Count, recurse))
                .OverrideAtomSerializer<PropertyStep>((step, assembler, overridden, recurse) =>
                {
                    if (T.Id.Equals(step.Key) && !Cardinality.Single.Equals(step.Cardinality.IfNone(Cardinality.Single)))
                        throw new NotSupportedException("Cannot have an id property on non-single cardinality.");

                    if (ReferenceEquals(step.Key, T.Id))
                        assembler.Method("property", step.MetaProperties.Prepend(step.Value).Prepend(step.Key), recurse);
                    else
                    {
                        step.Cardinality.Match(
                            c => assembler.Method("property", step.MetaProperties.Prepend(step.Value).Prepend(step.Key).Prepend(c), recurse),
                            () => assembler.Method("property", step.MetaProperties.Prepend(step.Value).Prepend(step.Key), recurse));
                    }
                })
                .OverrideAtomSerializer<RangeStep>((step, assembler, overridden, recurse) => assembler.Method("range", step.Lower, step.Upper, recurse))
                .OverrideAtomSerializer<ByMemberStep>((step, assembler, overridden, recurse) => assembler.Method("by", step.Key, step.Order, recurse))
                .OverrideAtomSerializer<KeyStep>((step, assembler, overridden, recurse) => assembler.Method("key"))
                .OverrideAtomSerializer<PropertiesStep>((step, assembler, overridden, recurse) => assembler.Method("properties", step.Keys, recurse))
                .OverrideAtomSerializer<VStep>((step, assembler, overridden, recurse) => assembler.Method("V", step.Ids, recurse))
                .OverrideAtomSerializer<EStep>((step, assembler, overridden, recurse) => assembler.Method("E", step.Ids, recurse))
                .OverrideAtomSerializer<InjectStep>((step, assembler, overridden, recurse) => assembler.Method("inject", step.Elements, recurse))
                .OverrideAtomSerializer<P.Eq>((p, assembler, overridden, recurse) => assembler.Method("eq", p.Argument, recurse))
                .OverrideAtomSerializer<P.Between>((p, assembler, overridden, recurse) => assembler.Method("between", p.Lower, p.Upper, recurse))
                .OverrideAtomSerializer<P.Gt>((p, assembler, overridden, recurse) => assembler.Method("gt", p.Argument, recurse))
                .OverrideAtomSerializer<P.Gte>((p, assembler, overridden, recurse) => assembler.Method("gte", p.Argument, recurse))
                .OverrideAtomSerializer<P.Lt>((p, assembler, overridden, recurse) => assembler.Method("lt", p.Argument, recurse))
                .OverrideAtomSerializer<P.Lte>((p, assembler, overridden, recurse) => assembler.Method("lte", p.Argument, recurse))
                .OverrideAtomSerializer<P.Neq>((p, assembler, overridden, recurse) => assembler.Method("neq", p.Argument, recurse))
                .OverrideAtomSerializer<P.Within>((p, assembler, overridden, recurse) => assembler.Method("within", p.Arguments, recurse))
                .OverrideAtomSerializer<P.Without>((p, assembler, overridden, recurse) => assembler.Method("without", p.Arguments, recurse))
                .OverrideAtomSerializer<P.Outside>((p, assembler, overridden, recurse) => assembler.Method("outside", p.Lower, p.Upper, recurse))
                .OverrideAtomSerializer<P.AndP>((p, assembler, overridden, recurse) =>
                {
                    recurse(p.Operand1);
                    assembler.Method("and", p.Operand2, recurse);
                })
                .OverrideAtomSerializer<P.OrP>((p, assembler, overridden, recurse) =>
                {
                    recurse(p.Operand1);
                    assembler.Method("or", p.Operand2, recurse);
                })
                .OverrideAtomSerializer<TextP.StartingWith>((p, assembler, overridden, recurse) => assembler.Method("startingWith", p.Value, recurse))
                .OverrideAtomSerializer<TextP.EndingWith>((p, assembler, overridden, recurse) => assembler.Method("endingWith", p.Value, recurse))
                .OverrideAtomSerializer<TextP.Containing>((p, assembler, overridden, recurse) => assembler.Method("containing", p.Value, recurse))
                .OverrideAtomSerializer<Lambda>((lambda, assembler, overridden, recurse) => assembler.Lambda(lambda.LambdaString))
                .OverrideAtomSerializer<Cardinality>((enumValue, assembler, overridden, recurse) => assembler.Field(enumValue.Name))
                .OverrideAtomSerializer<Order>((enumValue, assembler, overridden, recurse) => assembler.Field(enumValue.Name))
                .OverrideAtomSerializer<Scope>((enumValue, assembler, overridden, recurse) => assembler.Field(enumValue.Name))
                .OverrideAtomSerializer<T>((enumValue, assembler, overridden, recurse) => assembler.Field(enumValue.Name))
                .OverrideAtomSerializer<HasValueStep>((step, assembler, overridden, recurse) =>
                {
                    assembler.Method(
                        "hasValue",
                        step.Argument is P.Eq eq
                            ? eq.Argument
                            : step.Argument,
                        recurse);
                })
                .OverrideAtomSerializer<AddEStep>((step, assembler, overridden, recurse) => assembler.Method("addE", step.Label, recurse))
                .OverrideAtomSerializer<AddVStep>((step, assembler, overridden, recurse) => assembler.Method("addV", step.Label, recurse))
                .OverrideAtomSerializer<AndStep>((step, assembler, overridden, recurse) => assembler.Method("and", step.Traversals.SelectMany(FlattenLogicalTraversals<AndStep>), recurse))
                .OverrideAtomSerializer<ByTraversalStep>((step, assembler, overridden, recurse) => assembler.Method("by", step.Traversal, step.Order, recurse))
                .OverrideAtomSerializer<ChooseTraversalStep>((step, assembler, overridden, recurse) =>
                {
                    step.ElseTraversal.Match(
                        elseTraversal => assembler.Method(
                            "choose",
                            step.IfTraversal,
                            step.ThenTraversal,
                            elseTraversal,
                            recurse),
                        () => assembler.Method(
                            "choose",
                            step.IfTraversal,
                            step.ThenTraversal,
                            recurse));
                })
                .OverrideAtomSerializer<ChoosePredicateStep>((step, assembler, overridden, recurse) =>
                {
                    step.ElseTraversal.Match(
                        elseTraversal => assembler.Method(
                            "choose",
                            step.Predicate,
                            step.ThenTraversal,
                            elseTraversal,
                            recurse),
                        () => assembler.Method(
                            "choose",
                            step.Predicate,
                            step.ThenTraversal,
                            recurse));
                })
                .OverrideAtomSerializer<CoalesceStep>((step, assembler, overridden, recurse) => assembler.Method("coalesce", step.Traversals, recurse))
                .OverrideAtomSerializer<CoinStep>((step, assembler, overridden, recurse) => assembler.Method("coin", step.Probability, recurse))
                .OverrideAtomSerializer<ConstantStep>((step, assembler, overridden, recurse) => assembler.Method("constant", step.Value, recurse))
                .OverrideAtomSerializer<BothStep>((step, assembler, overridden, recurse) => assembler.Method("both", step.Labels, recurse))
                .OverrideAtomSerializer<BothEStep>((step, assembler, overridden, recurse) => assembler.Method("bothE", step.Labels, recurse))
                .OverrideAtomSerializer<InStep>((step, assembler, overridden, recurse) => assembler.Method("in", step.Labels, recurse))
                .OverrideAtomSerializer<InEStep>((step, assembler, overridden, recurse) => assembler.Method("inE", step.Labels, recurse))
                .OverrideAtomSerializer<OutStep>((step, assembler, overridden, recurse) => assembler.Method("out", step.Labels, recurse))
                .OverrideAtomSerializer<OutEStep>((step, assembler, overridden, recurse) => assembler.Method("outE", step.Labels, recurse))
                .OverrideAtomSerializer<HasLabelStep>((step, assembler, overridden, recurse) => assembler.Method("hasLabel", step.Labels, recurse))
                .OverrideAtomSerializer<LabelStep>((step, assembler, overridden, recurse) => assembler.Method("label"))
                .OverrideAtomSerializer<EdgesStep>((step, assembler, overridden, recurse) => assembler.Method("edges", step.Traversal, recurse))
                .OverrideAtomSerializer<FromTraversalStep>((step, assembler, overridden, recurse) => assembler.Method("from", step.Traversal, recurse))
                .OverrideAtomSerializer<IdentifierStep>((step, assembler, overridden, recurse) => assembler.Identifier(step.Identifier))
                .OverrideAtomSerializer<IsStep>((step, assembler, overridden, recurse) =>
                {
                    assembler.Method(
                        "is",
                        step.Argument is P.Eq eq
                            ? eq.Argument
                            : step.Argument,
                        recurse);
                })
                .OverrideAtomSerializer<LimitStep>((step, assembler, overridden, recurse) =>
                {
                    if (step.Scope.Equals(Scope.Local))
                        assembler.Method("limit", step.Scope, step.Count, recurse);
                    else
                        assembler.Method("limit", step.Count, recurse);
                })
                .OverrideAtomSerializer<LocalStep>((step, assembler, overridden, recurse) => assembler.Method("local", step.Traversal, recurse))
                .OverrideAtomSerializer<MapStep>((step, assembler, overridden, recurse) => assembler.Method("map", step.Traversal, recurse))
                .OverrideAtomSerializer<NoneStep>((step, assembler, overridden, recurse) => assembler.Method("none"))
                .OverrideAtomSerializer<FlatMapStep>((step, assembler, overridden, recurse) => assembler.Method("flatMap", step.Traversal, recurse))
                .OverrideAtomSerializer<MatchStep>((step, assembler, overridden, recurse) => assembler.Method("match", step.Traversals, recurse))
                .OverrideAtomSerializer<NotStep>((step, assembler, overridden, recurse) =>
                {
                    var traversalSteps = step.Traversal.AsAdmin().Steps;

                    if (!(traversalSteps.Count != 0 && traversalSteps[traversalSteps.Count - 1] is HasStep hasStep && hasStep.Value is P p && p.EqualsConstant(false)))
                        assembler.Method("not", step.Traversal, recurse);
                })
                .OverrideAtomSerializer<OptionalStep>((step, assembler, overridden, recurse) => assembler.Method("optional", step.Traversal, recurse))
                .OverrideAtomSerializer<OrStep>((step, assembler, overridden, recurse) => assembler.Method("or", step.Traversals.SelectMany(FlattenLogicalTraversals<OrStep>), recurse))
                .OverrideAtomSerializer<ValueStep>((step, assembler, overridden, recurse) => assembler.Method("value"))
                .OverrideAtomSerializer<ValueMapStep>((step, assembler, overridden, recurse) => assembler.Method("valueMap", step.Keys, recurse))
                .OverrideAtomSerializer<ProjectStep.ByTraversalStep>((step, assembler, overridden, recurse) => assembler.Method("by", step.Traversal, recurse))
                .OverrideAtomSerializer<ProjectStep>((step, assembler, overridden, recurse) => assembler.Method("project", step.Projections, recurse));
        }

        private static IEnumerable<IGremlinQuery> FlattenLogicalTraversals<TStep>(IGremlinQuery query) where TStep : LogicalStep
        {
            var steps = query.AsAdmin().Steps;

            if (steps.Count == 2 && steps[1] is TStep otherStep)
            {
                foreach (var subTraversal in otherStep.Traversals)
                {
                    foreach (var flattenedSubTraversal in FlattenLogicalTraversals<TStep>(subTraversal))
                    {
                        yield return flattenedSubTraversal;
                    }
                }
            }
            else
                yield return query;
        }
    }
}
