using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using LanguageExt;

namespace ExRam.Gremlinq.Core
{
    public static class GremlinQuerySerializerBuilder
    {
        private sealed class GremlinQuerySerializerBuilderImpl : IGremlinQuerySerializerBuilder
        {
            private sealed class AssembledGremlinQuerySerializer : IGremlinQuerySerializer
            {
                private sealed class Recurse
                {
                    private readonly Action<object> _recurse;
                    private readonly ISerializedGremlinQueryAssembler _assembler;
                    private readonly IReadOnlyDictionary<Type, AtomSerializationHandler<object>> _dict;

                    public Recurse(IReadOnlyDictionary<Type, AtomSerializationHandler<object>> dict, ISerializedGremlinQueryAssembler assembler)
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
                            var action = _dict
                                .TryGetValue(o.GetType())
                                .IfNone((atom, assembler, baseSerializer, recurse) => assembler.Constant(atom));

                            action(o, _assembler, _ => throw new NotImplementedException(), _recurse);
                        }
                    }
                }

                private readonly IReadOnlyDictionary<Type, AtomSerializationHandler<object>> _dict;
                private readonly ISerializedGremlinQueryAssemblerFactory _assemblerFactory;

                public AssembledGremlinQuerySerializer(IReadOnlyDictionary<Type, AtomSerializationHandler<object>> dict, ISerializedGremlinQueryAssemblerFactory assemblerFactory)
                {
                    _dict = dict;
                    _assemblerFactory = assemblerFactory;
                }

                public object Serialize(IGremlinQuery query)
                {
                    var assembler = _assemblerFactory.Create();

                    new Recurse(_dict, assembler)
                        .RecurseImpl(query);

                    return assembler.Assemble();
                }
            }

            private readonly IImmutableDictionary<Type, AtomSerializationHandler<object>> _dict;
            private readonly ISerializedGremlinQueryAssemblerFactory _assemblerFactory;
            private readonly Lazy<Dictionary<Type, AtomSerializationHandler<object>>> _lazyFastDict;

            public GremlinQuerySerializerBuilderImpl(IImmutableDictionary<Type, AtomSerializationHandler<object>> dict, ISerializedGremlinQueryAssemblerFactory assemblerFactory)
            {
                _dict = dict;
                _assemblerFactory = assemblerFactory;
                _lazyFastDict = new Lazy<Dictionary<Type, AtomSerializationHandler<object>>>(
                    () => dict.ToDictionary(kvp => kvp.Key, kvp => kvp.Value),
                    LazyThreadSafetyMode.None);
            }

            public IGremlinQuerySerializerBuilder OverrideAtomSerializationHandler<TAtom>(AtomSerializationHandler<TAtom> atomSerializationHandler)
            {
                return new GremlinQuerySerializerBuilderImpl(
                    _dict
                        .TryGetValue(typeof(TAtom))
                        .Match(
                            existingAtomSerializer => _dict.SetItem(typeof(TAtom), (atom, assembler, baseSerializer, recurse) => atomSerializationHandler((TAtom)atom, assembler, _ => existingAtomSerializer(_, assembler, baseSerializer, recurse), recurse)),
                            () => _dict.SetItem(typeof(TAtom), (atom, assembler, baseSerializer, recurse) => atomSerializationHandler((TAtom)atom, assembler, _ => throw new NotImplementedException(), recurse))),
                    _assemblerFactory);
            }

            public IGremlinQuerySerializerBuilder ConfigureAssemblerFactory(Func<ISerializedGremlinQueryAssemblerFactory, ISerializedGremlinQueryAssemblerFactory> transformation)
            {
                return new GremlinQuerySerializerBuilderImpl(
                    _dict,
                    transformation(_assemblerFactory));
            }

            public IGremlinQuerySerializer Build()
            {
                return new AssembledGremlinQuerySerializer(_lazyFastDict.Value, _assemblerFactory);
            }
        }

        public static readonly IGremlinQuerySerializerBuilder Invalid = new GremlinQuerySerializerBuilderImpl(ImmutableDictionary<Type, AtomSerializationHandler<object>>.Empty, SerializedGremlinQueryAssemblerFactory.Invalid);

        public static IGremlinQuerySerializerBuilder UseGroovy(this IGremlinQuerySerializerBuilder builder)
        {
            return builder
                .ConfigureAssemblerFactory(_ => SerializedGremlinQueryAssemblerFactory.Groovy);
        }

        public static IGremlinQuerySerializerBuilder UseDefaultGremlinStepSerializationHandlers(this IGremlinQuerySerializerBuilder builder)
        {
            return builder
                .OverrideAtomSerializationHandler<HasNotStep>((step, assembler, overridden, recurse) => assembler.Method("hasNot", step.Key, recurse))
                .OverrideAtomSerializationHandler<ChooseOptionTraversalStep>((step, assembler, overridden, recurse) => assembler.Method("choose", step.Traversal, recurse))
                .OverrideAtomSerializationHandler<OptionTraversalStep>((step, assembler, overridden, recurse) => assembler.Method("option", step.Guard, step.OptionTraversal, recurse))
                .OverrideAtomSerializationHandler<WithoutStrategiesStep>((step, assembler, overridden, recurse) =>
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
                .OverrideAtomSerializationHandler<HasStep>((step, assembler, overridden, recurse) =>
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
                .OverrideAtomSerializationHandler<RepeatStep>((step, assembler, overridden, recurse) => assembler.Method("repeat", step.Traversal, recurse))
                .OverrideAtomSerializationHandler<SideEffectStep>((step, assembler, overridden, recurse) => assembler.Method("sideEffect", step.Traversal, recurse))
                .OverrideAtomSerializationHandler<ToTraversalStep>((step, assembler, overridden, recurse) => assembler.Method("to", step.Traversal, recurse))
                .OverrideAtomSerializationHandler<UnionStep>((step, assembler, overridden, recurse) => assembler.Method("union", step.Traversals, recurse))
                .OverrideAtomSerializationHandler<UntilStep>((step, assembler, overridden, recurse) => assembler.Method("until", step.Traversal, recurse))
                .OverrideAtomSerializationHandler<ValuesStep>((step, assembler, overridden, recurse) => assembler.Method("values", step.Keys, recurse))
                .OverrideAtomSerializationHandler<VerticesStep>((step, assembler, overridden, recurse) => assembler.Method("vertices", step.Traversal, recurse))
                .OverrideAtomSerializationHandler<WhereTraversalStep>((step, assembler, overridden, recurse) => assembler.Method("where", step.Traversal, recurse))
                .OverrideAtomSerializationHandler<WithStrategiesStep>((step, assembler, overridden, recurse) => assembler.Method("withStrategies", step.Traversal, recurse))
                .OverrideAtomSerializationHandler<IdStep>((step, assembler, overridden, recurse) => assembler.Method("id"))
                .OverrideAtomSerializationHandler<BarrierStep>((step, assembler, overridden, recurse) => assembler.Method("barrier"))
                .OverrideAtomSerializationHandler<OrderStep>((step, assembler, overridden, recurse) => assembler.Method("order"))
                .OverrideAtomSerializationHandler<CreateStep>((step, assembler, overridden, recurse) => assembler.Method("create"))
                .OverrideAtomSerializationHandler<UnfoldStep>((step, assembler, overridden, recurse) => assembler.Method("unfold"))
                .OverrideAtomSerializationHandler<IdentityStep>((step, assembler, overridden, recurse) => assembler.Method("identity"))
                .OverrideAtomSerializationHandler<EmitStep>((step, assembler, overridden, recurse) => assembler.Method("emit"))
                .OverrideAtomSerializationHandler<DedupStep>((step, assembler, overridden, recurse) => assembler.Method("dedup"))
                .OverrideAtomSerializationHandler<OutVStep>((step, assembler, overridden, recurse) => assembler.Method("outV"))
                .OverrideAtomSerializationHandler<OtherVStep>((step, assembler, overridden, recurse) => assembler.Method("otherV"))
                .OverrideAtomSerializationHandler<InVStep>((step, assembler, overridden, recurse) => assembler.Method("inV"))
                .OverrideAtomSerializationHandler<BothVStep>((step, assembler, overridden, recurse) => assembler.Method("bothV"))
                .OverrideAtomSerializationHandler<DropStep>((step, assembler, overridden, recurse) => assembler.Method("drop"))
                .OverrideAtomSerializationHandler<FoldStep>((step, assembler, overridden, recurse) => assembler.Method("fold"))
                .OverrideAtomSerializationHandler<ExplainStep>((step, assembler, overridden, recurse) => assembler.Method("explain"))
                .OverrideAtomSerializationHandler<ProfileStep>((step, assembler, overridden, recurse) => assembler.Method("profile"))
                .OverrideAtomSerializationHandler<CountStep>((step, assembler, overridden, recurse) =>
                {
                    if (step.Scope.Equals(Scope.Local))
                        assembler.Method("count", step.Scope, recurse);
                    else
                        assembler.Method("count");
                })
                .OverrideAtomSerializationHandler<BuildStep>((step, assembler, overridden, recurse) => assembler.Method("build"))
                .OverrideAtomSerializationHandler<SumStep>((step, assembler, overridden, recurse) => assembler.Method("sum", step.Scope, recurse))
                .OverrideAtomSerializationHandler<TailStep>((step, assembler, overridden, recurse) =>
                {
                    if (step.Scope.Equals(Scope.Local))
                        assembler.Method("tail", step.Scope, step.Count, recurse);
                    else
                        assembler.Method("tail", step.Count, recurse);
                })
                .OverrideAtomSerializationHandler<SelectStep>((step, assembler, overridden, recurse) => assembler.Method("select", step.StepLabels, recurse))
                .OverrideAtomSerializationHandler<AsStep>((step, assembler, overridden, recurse) => assembler.Method("as", step.StepLabels, recurse))
                .OverrideAtomSerializationHandler<FromLabelStep>((step, assembler, overridden, recurse) => assembler.Method("from", step.StepLabel, recurse))
                .OverrideAtomSerializationHandler<ToLabelStep>((step, assembler, overridden, recurse) => assembler.Method("to", step.StepLabel, recurse))
                .OverrideAtomSerializationHandler<TimesStep>((step, assembler, overridden, recurse) => assembler.Method("times", step.Count, recurse))
                .OverrideAtomSerializationHandler<FilterStep>((step, assembler, overridden, recurse) => assembler.Method("filter", step.Lambda, recurse))
                .OverrideAtomSerializationHandler<AggregateStep>((step, assembler, overridden, recurse) => assembler.Method("aggregate", step.StepLabel, recurse))
                .OverrideAtomSerializationHandler<WherePredicateStep>((step, assembler, overridden, recurse) => assembler.Method("where", step.Predicate, recurse))
                .OverrideAtomSerializationHandler<ByLambdaStep>((step, assembler, overridden, recurse) => assembler.Method("by", step.Lambda, recurse))
                .OverrideAtomSerializationHandler<SkipStep>((step, assembler, overridden, recurse) => assembler.Method("skip", step.Count, recurse))
                .OverrideAtomSerializationHandler<PropertyStep>((step, assembler, overridden, recurse) =>
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
                .OverrideAtomSerializationHandler<RangeStep>((step, assembler, overridden, recurse) => assembler.Method("range", step.Lower, step.Upper, recurse))
                .OverrideAtomSerializationHandler<ByMemberStep>((step, assembler, overridden, recurse) => assembler.Method("by", step.Key, step.Order, recurse))
                .OverrideAtomSerializationHandler<KeyStep>((step, assembler, overridden, recurse) => assembler.Method("key"))
                .OverrideAtomSerializationHandler<PropertiesStep>((step, assembler, overridden, recurse) => assembler.Method("properties", step.Keys, recurse))
                .OverrideAtomSerializationHandler<VStep>((step, assembler, overridden, recurse) => assembler.Method("V", step.Ids, recurse))
                .OverrideAtomSerializationHandler<EStep>((step, assembler, overridden, recurse) => assembler.Method("E", step.Ids, recurse))
                .OverrideAtomSerializationHandler<InjectStep>((step, assembler, overridden, recurse) => assembler.Method("inject", step.Elements, recurse))
                .OverrideAtomSerializationHandler<P.Eq>((p, assembler, overridden, recurse) => assembler.Method("eq", p.Argument, recurse))
                .OverrideAtomSerializationHandler<P.Between>((p, assembler, overridden, recurse) => assembler.Method("between", p.Lower, p.Upper, recurse))
                .OverrideAtomSerializationHandler<P.Gt>((p, assembler, overridden, recurse) => assembler.Method("gt", p.Argument, recurse))
                .OverrideAtomSerializationHandler<P.Gte>((p, assembler, overridden, recurse) => assembler.Method("gte", p.Argument, recurse))
                .OverrideAtomSerializationHandler<P.Lt>((p, assembler, overridden, recurse) => assembler.Method("lt", p.Argument, recurse))
                .OverrideAtomSerializationHandler<P.Lte>((p, assembler, overridden, recurse) => assembler.Method("lte", p.Argument, recurse))
                .OverrideAtomSerializationHandler<P.Neq>((p, assembler, overridden, recurse) => assembler.Method("neq", p.Argument, recurse))
                .OverrideAtomSerializationHandler<P.Within>((p, assembler, overridden, recurse) => assembler.Method("within", p.Arguments, recurse))
                .OverrideAtomSerializationHandler<P.Without>((p, assembler, overridden, recurse) => assembler.Method("without", p.Arguments, recurse))
                .OverrideAtomSerializationHandler<P.Outside>((p, assembler, overridden, recurse) => assembler.Method("outside", p.Lower, p.Upper, recurse))
                .OverrideAtomSerializationHandler<P.AndP>((p, assembler, overridden, recurse) =>
                {
                    recurse(p.Operand1);
                    assembler.Method("and", p.Operand2, recurse);
                })
                .OverrideAtomSerializationHandler<P.OrP>((p, assembler, overridden, recurse) =>
                {
                    recurse(p.Operand1);
                    assembler.Method("or", p.Operand2, recurse);
                })
                .OverrideAtomSerializationHandler<TextP.StartingWith>((p, assembler, overridden, recurse) => assembler.Method("startingWith", p.Value, recurse))
                .OverrideAtomSerializationHandler<TextP.EndingWith>((p, assembler, overridden, recurse) => assembler.Method("endingWith", p.Value, recurse))
                .OverrideAtomSerializationHandler<TextP.Containing>((p, assembler, overridden, recurse) => assembler.Method("containing", p.Value, recurse))
                .OverrideAtomSerializationHandler<Lambda>((lambda, assembler, overridden, recurse) => assembler.Lambda(lambda.LambdaString))
                .OverrideAtomSerializationHandler<Cardinality>((enumValue, assembler, overridden, recurse) => assembler.Field(enumValue.Name))
                .OverrideAtomSerializationHandler<Order>((enumValue, assembler, overridden, recurse) => assembler.Field(enumValue.Name))
                .OverrideAtomSerializationHandler<Scope>((enumValue, assembler, overridden, recurse) => assembler.Field(enumValue.Name))
                .OverrideAtomSerializationHandler<T>((enumValue, assembler, overridden, recurse) => assembler.Field(enumValue.Name))
                .OverrideAtomSerializationHandler<HasValueStep>((step, assembler, overridden, recurse) =>
                {
                    assembler.Method(
                        "hasValue",
                        step.Argument is P.Eq eq
                            ? eq.Argument
                            : step.Argument,
                        recurse);
                })
                .OverrideAtomSerializationHandler<AddEStep>((step, assembler, overridden, recurse) => assembler.Method("addE", step.Label, recurse))
                .OverrideAtomSerializationHandler<AddVStep>((step, assembler, overridden, recurse) => assembler.Method("addV", step.Label, recurse))
                .OverrideAtomSerializationHandler<AndStep>((step, assembler, overridden, recurse) => assembler.Method("and", step.Traversals.SelectMany(FlattenLogicalTraversals<AndStep>), recurse))
                .OverrideAtomSerializationHandler<ByTraversalStep>((step, assembler, overridden, recurse) => assembler.Method("by", step.Traversal, step.Order, recurse))
                .OverrideAtomSerializationHandler<ChooseTraversalStep>((step, assembler, overridden, recurse) =>
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
                .OverrideAtomSerializationHandler<ChoosePredicateStep>((step, assembler, overridden, recurse) =>
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
                .OverrideAtomSerializationHandler<CoalesceStep>((step, assembler, overridden, recurse) => assembler.Method("coalesce", step.Traversals, recurse))
                .OverrideAtomSerializationHandler<CoinStep>((step, assembler, overridden, recurse) => assembler.Method("coin", step.Probability, recurse))
                .OverrideAtomSerializationHandler<ConstantStep>((step, assembler, overridden, recurse) => assembler.Method("constant", step.Value, recurse))
                .OverrideAtomSerializationHandler<BothStep>((step, assembler, overridden, recurse) => assembler.Method("both", step.Labels, recurse))
                .OverrideAtomSerializationHandler<BothEStep>((step, assembler, overridden, recurse) => assembler.Method("bothE", step.Labels, recurse))
                .OverrideAtomSerializationHandler<InStep>((step, assembler, overridden, recurse) => assembler.Method("in", step.Labels, recurse))
                .OverrideAtomSerializationHandler<InEStep>((step, assembler, overridden, recurse) => assembler.Method("inE", step.Labels, recurse))
                .OverrideAtomSerializationHandler<OutStep>((step, assembler, overridden, recurse) => assembler.Method("out", step.Labels, recurse))
                .OverrideAtomSerializationHandler<OutEStep>((step, assembler, overridden, recurse) => assembler.Method("outE", step.Labels, recurse))
                .OverrideAtomSerializationHandler<HasLabelStep>((step, assembler, overridden, recurse) => assembler.Method("hasLabel", step.Labels, recurse))
                .OverrideAtomSerializationHandler<LabelStep>((step, assembler, overridden, recurse) => assembler.Method("label"))
                .OverrideAtomSerializationHandler<EdgesStep>((step, assembler, overridden, recurse) => assembler.Method("edges", step.Traversal, recurse))
                .OverrideAtomSerializationHandler<FromTraversalStep>((step, assembler, overridden, recurse) => assembler.Method("from", step.Traversal, recurse))
                .OverrideAtomSerializationHandler<IdentifierStep>((step, assembler, overridden, recurse) => assembler.Identifier(step.Identifier))
                .OverrideAtomSerializationHandler<IsStep>((step, assembler, overridden, recurse) =>
                {
                    assembler.Method(
                        "is",
                        step.Argument is P.Eq eq
                            ? eq.Argument
                            : step.Argument,
                        recurse);
                })
                .OverrideAtomSerializationHandler<LimitStep>((step, assembler, overridden, recurse) =>
                {
                    if (step.Scope.Equals(Scope.Local))
                        assembler.Method("limit", step.Scope, step.Count, recurse);
                    else
                        assembler.Method("limit", step.Count, recurse);
                })
                .OverrideAtomSerializationHandler<LocalStep>((step, assembler, overridden, recurse) => assembler.Method("local", step.Traversal, recurse))
                .OverrideAtomSerializationHandler<MapStep>((step, assembler, overridden, recurse) => assembler.Method("map", step.Traversal, recurse))
                .OverrideAtomSerializationHandler<NoneStep>((step, assembler, overridden, recurse) => assembler.Method("none"))
                .OverrideAtomSerializationHandler<FlatMapStep>((step, assembler, overridden, recurse) => assembler.Method("flatMap", step.Traversal, recurse))
                .OverrideAtomSerializationHandler<MatchStep>((step, assembler, overridden, recurse) => assembler.Method("match", step.Traversals, recurse))
                .OverrideAtomSerializationHandler<NotStep>((step, assembler, overridden, recurse) =>
                {
                    var traversalSteps = step.Traversal.AsAdmin().Steps;

                    if (!(traversalSteps.Count != 0 && traversalSteps[traversalSteps.Count - 1] is HasStep hasStep && hasStep.Value is P p && p.EqualsConstant(false)))
                        assembler.Method("not", step.Traversal, recurse);
                })
                .OverrideAtomSerializationHandler<OptionalStep>((step, assembler, overridden, recurse) => assembler.Method("optional", step.Traversal, recurse))
                .OverrideAtomSerializationHandler<OrStep>((step, assembler, overridden, recurse) => assembler.Method("or", step.Traversals.SelectMany(FlattenLogicalTraversals<OrStep>), recurse))
                .OverrideAtomSerializationHandler<ValueStep>((step, assembler, overridden, recurse) => assembler.Method("value"))
                .OverrideAtomSerializationHandler<ValueMapStep>((step, assembler, overridden, recurse) => assembler.Method("valueMap", step.Keys, recurse))
                .OverrideAtomSerializationHandler<ProjectStep.ByTraversalStep>((step, assembler, overridden, recurse) => assembler.Method("by", step.Traversal, recurse))
                .OverrideAtomSerializationHandler<ProjectStep>((step, assembler, overridden, recurse) => assembler.Method("project", step.Projections, recurse));
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
