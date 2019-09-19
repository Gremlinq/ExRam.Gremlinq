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
                private readonly IReadOnlyDictionary<Type, AtomSerializer<object>> _dict;
                private readonly ISerializedGremlinQueryAssemblerFactory _assemblerFactory;

                public AssembledGremlinQuerySerializer(Dictionary<Type, AtomSerializer<object>> dict, ISerializedGremlinQueryAssemblerFactory assemblerFactory)
                {
                    _dict = dict;
                    _assemblerFactory = assemblerFactory;
                }

                public object Serialize(IGremlinQuery query)
                {
                    var assembler = _assemblerFactory.Create();

                    Recurse(query, assembler);

                    return assembler.Assemble();
                }

                private void Recurse(object o, ISerializedGremlinQueryAssembler assembler)
                {
                    if (o is IGremlinQuery query)
                    {
                        var steps = query.AsAdmin().Steps.HandleAnonymousQueries();
                        if (query.AsAdmin().Environment.Options.GetValue(GremlinQuerySerializer.WorkaroundTinkerpop2112))
                            steps = steps.WorkaroundTINKERPOP_2112();

                        foreach (var step in steps)
                        {
                            Recurse(step, assembler);
                        }
                    }
                    else
                    {
                        _dict
                            .TryGetValue(o.GetType())
                            .Match(
                                d => d(o, assembler, _ => throw new NotImplementedException(), _ => Recurse(_, assembler)),
                                () => assembler.Constant(o));
                    }
                }
            }

            private readonly IImmutableDictionary<Type, AtomSerializer<object>> _dict;
            private readonly ISerializedGremlinQueryAssemblerFactory _assemblerFactory;
            private readonly Lazy<Dictionary<Type, AtomSerializer<object>>> _lazyFastDict;

            public GremlinQuerySerializerBuilderImpl(IImmutableDictionary<Type, AtomSerializer<object>> dict, ISerializedGremlinQueryAssemblerFactory assemblerFactory)
            {
                _dict = dict;
                _assemblerFactory = assemblerFactory;
                _lazyFastDict = new Lazy<Dictionary<Type, AtomSerializer<object>>>(
                    () => dict.ToDictionary(kvp => kvp.Key, kvp => kvp.Value),
                    LazyThreadSafetyMode.None);
            }

            public IGremlinQuerySerializerBuilder OverrideAtom<TAtom>(AtomSerializer<TAtom> atomSerializer)
            {
                return new GremlinQuerySerializerBuilderImpl(
                    _dict
                        .TryGetValue(typeof(TAtom))
                        .Match(
                            existingAtomSerializer => _dict.SetItem(typeof(TAtom), (atom, assembler, baseSerializer, recurse) => atomSerializer((TAtom)atom, assembler, _ => existingAtomSerializer(_, assembler, baseSerializer, recurse), recurse)),
                            () => _dict.SetItem(typeof(TAtom), (atom, assembler, baseSerializer, recurse) => atomSerializer((TAtom)atom, assembler, _ => throw new NotImplementedException(), recurse))),
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

        public static readonly IGremlinQuerySerializerBuilder Invalid = new GremlinQuerySerializerBuilderImpl(ImmutableDictionary<Type, AtomSerializer<object>>.Empty, SerializedGremlinQueryAssemblerFactory.Invalid);

        public static IGremlinQuerySerializerBuilder AddGroovy(this IGremlinQuerySerializerBuilder builder)
        {
            return builder
                .ConfigureAssemblerFactory(_ => SerializedGremlinQueryAssemblerFactory.Groovy);
        }

        public static IGremlinQuerySerializerBuilder AddGremlinSteps(this IGremlinQuerySerializerBuilder builder)
        {
            return builder
                .OverrideAtom<HasNotStep>((step, assembler, overridden, recurse) => assembler.Method("hasNot", step.Key, recurse))
                .OverrideAtom<ChooseOptionTraversalStep>((step, assembler, overridden, recurse) => assembler.Method("choose", step.Traversal, recurse))
                .OverrideAtom<OptionTraversalStep>((step, assembler, overridden, recurse) => assembler.Method("option", step.Guard, step.OptionTraversal, recurse))
                .OverrideAtom<WithoutStrategiesStep>((step, assembler, overridden, recurse) =>
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
                .OverrideAtom<HasStep>((step, assembler, overridden, recurse) =>
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
                .OverrideAtom<RepeatStep>((step, assembler, overridden, recurse) => assembler.Method("repeat", step.Traversal, recurse))
                .OverrideAtom<SideEffectStep>((step, assembler, overridden, recurse) => assembler.Method("sideEffect", step.Traversal, recurse))
                .OverrideAtom<ToTraversalStep>((step, assembler, overridden, recurse) => assembler.Method("to", step.Traversal, recurse))
                .OverrideAtom<UnionStep>((step, assembler, overridden, recurse) => assembler.Method("union", step.Traversals, recurse))
                .OverrideAtom<UntilStep>((step, assembler, overridden, recurse) => assembler.Method("until", step.Traversal, recurse))
                .OverrideAtom<ValuesStep>((step, assembler, overridden, recurse) => assembler.Method("values", step.Keys, recurse))
                .OverrideAtom<VerticesStep>((step, assembler, overridden, recurse) => assembler.Method("vertices", step.Traversal, recurse))
                .OverrideAtom<WhereTraversalStep>((step, assembler, overridden, recurse) => assembler.Method("where", step.Traversal, recurse))
                .OverrideAtom<WithStrategiesStep>((step, assembler, overridden, recurse) => assembler.Method("withStrategies", step.Traversal, recurse))
                .OverrideAtom<IdStep>((step, assembler, overridden, recurse) => assembler.Method("id"))
                .OverrideAtom<BarrierStep>((step, assembler, overridden, recurse) => assembler.Method("barrier"))
                .OverrideAtom<OrderStep>((step, assembler, overridden, recurse) => assembler.Method("order"))
                .OverrideAtom<CreateStep>((step, assembler, overridden, recurse) => assembler.Method("create"))
                .OverrideAtom<UnfoldStep>((step, assembler, overridden, recurse) => assembler.Method("unfold"))
                .OverrideAtom<IdentityStep>((step, assembler, overridden, recurse) => assembler.Method("identity"))
                .OverrideAtom<EmitStep>((step, assembler, overridden, recurse) => assembler.Method("emit"))
                .OverrideAtom<DedupStep>((step, assembler, overridden, recurse) => assembler.Method("dedup"))
                .OverrideAtom<OutVStep>((step, assembler, overridden, recurse) => assembler.Method("outV"))
                .OverrideAtom<OtherVStep>((step, assembler, overridden, recurse) => assembler.Method("otherV"))
                .OverrideAtom<InVStep>((step, assembler, overridden, recurse) => assembler.Method("inV"))
                .OverrideAtom<BothVStep>((step, assembler, overridden, recurse) => assembler.Method("bothV"))
                .OverrideAtom<DropStep>((step, assembler, overridden, recurse) => assembler.Method("drop"))
                .OverrideAtom<FoldStep>((step, assembler, overridden, recurse) => assembler.Method("fold"))
                .OverrideAtom<ExplainStep>((step, assembler, overridden, recurse) => assembler.Method("explain"))
                .OverrideAtom<ProfileStep>((step, assembler, overridden, recurse) => assembler.Method("profile"))
                .OverrideAtom<CountStep>((step, assembler, overridden, recurse) =>
                {
                    if (step.Scope.Equals(Scope.Local))
                        assembler.Method("count", step.Scope, recurse);
                    else
                        assembler.Method("count");
                })
                .OverrideAtom<BuildStep>((step, assembler, overridden, recurse) => assembler.Method("build"))
                .OverrideAtom<SumStep>((step, assembler, overridden, recurse) => assembler.Method("sum", step.Scope, recurse))
                .OverrideAtom<TailStep>((step, assembler, overridden, recurse) =>
                {
                    if (step.Scope.Equals(Scope.Local))
                        assembler.Method("tail", step.Scope, step.Count, recurse);
                    else
                        assembler.Method("tail", step.Count, recurse);
                })
                .OverrideAtom<SelectStep>((step, assembler, overridden, recurse) => assembler.Method("select", step.StepLabels, recurse))
                .OverrideAtom<AsStep>((step, assembler, overridden, recurse) => assembler.Method("as", step.StepLabels, recurse))
                .OverrideAtom<FromLabelStep>((step, assembler, overridden, recurse) => assembler.Method("from", step.StepLabel, recurse))
                .OverrideAtom<ToLabelStep>((step, assembler, overridden, recurse) => assembler.Method("to", step.StepLabel, recurse))
                .OverrideAtom<TimesStep>((step, assembler, overridden, recurse) => assembler.Method("times", step.Count, recurse))
                .OverrideAtom<FilterStep>((step, assembler, overridden, recurse) => assembler.Method("filter", step.Lambda, recurse))
                .OverrideAtom<AggregateStep>((step, assembler, overridden, recurse) => assembler.Method("aggregate", step.StepLabel, recurse))
                .OverrideAtom<WherePredicateStep>((step, assembler, overridden, recurse) => assembler.Method("where", step.Predicate, recurse))
                .OverrideAtom<ByLambdaStep>((step, assembler, overridden, recurse) => assembler.Method("by", step.Lambda, recurse))
                .OverrideAtom<SkipStep>((step, assembler, overridden, recurse) => assembler.Method("skip", step.Count, recurse))
                .OverrideAtom<PropertyStep>((step, assembler, overridden, recurse) =>
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
                .OverrideAtom<RangeStep>((step, assembler, overridden, recurse) => assembler.Method("range", step.Lower, step.Upper, recurse))
                .OverrideAtom<ByMemberStep>((step, assembler, overridden, recurse) => assembler.Method("by", step.Key, step.Order, recurse))
                .OverrideAtom<KeyStep>((step, assembler, overridden, recurse) => assembler.Method("key"))
                .OverrideAtom<PropertiesStep>((step, assembler, overridden, recurse) => assembler.Method("properties", step.Keys, recurse))
                .OverrideAtom<VStep>((step, assembler, overridden, recurse) => assembler.Method("V", step.Ids, recurse))
                .OverrideAtom<EStep>((step, assembler, overridden, recurse) => assembler.Method("E", step.Ids, recurse))
                .OverrideAtom<InjectStep>((step, assembler, overridden, recurse) => assembler.Method("inject", step.Elements, recurse))
                .OverrideAtom<P.Eq>((p, assembler, overridden, recurse) => assembler.Method("eq", p.Argument, recurse))
                .OverrideAtom<P.Between>((p, assembler, overridden, recurse) => assembler.Method("between", p.Lower, p.Upper, recurse))
                .OverrideAtom<P.Gt>((p, assembler, overridden, recurse) => assembler.Method("gt", p.Argument, recurse))
                .OverrideAtom<P.Gte>((p, assembler, overridden, recurse) => assembler.Method("gte", p.Argument, recurse))
                .OverrideAtom<P.Lt>((p, assembler, overridden, recurse) => assembler.Method("lt", p.Argument, recurse))
                .OverrideAtom<P.Lte>((p, assembler, overridden, recurse) => assembler.Method("lte", p.Argument, recurse))
                .OverrideAtom<P.Neq>((p, assembler, overridden, recurse) => assembler.Method("neq", p.Argument, recurse))
                .OverrideAtom<P.Within>((p, assembler, overridden, recurse) => assembler.Method("within", p.Arguments, recurse))
                .OverrideAtom<P.Without>((p, assembler, overridden, recurse) => assembler.Method("without", p.Arguments, recurse))
                .OverrideAtom<P.Outside>((p, assembler, overridden, recurse) => assembler.Method("outside", p.Lower, p.Upper, recurse))
                .OverrideAtom<P.AndP>((p, assembler, overridden, recurse) =>
                {
                    recurse(p.Operand1);
                    assembler.Method("and", p.Operand2, recurse);
                })
                .OverrideAtom<P.OrP>((p, assembler, overridden, recurse) =>
                {
                    recurse(p.Operand1);
                    assembler.Method("or", p.Operand2, recurse);
                })
                .OverrideAtom<TextP.StartingWith>((p, assembler, overridden, recurse) => assembler.Method("startingWith", p.Value, recurse))
                .OverrideAtom<TextP.EndingWith>((p, assembler, overridden, recurse) => assembler.Method("endingWith", p.Value, recurse))
                .OverrideAtom<TextP.Containing>((p, assembler, overridden, recurse) => assembler.Method("containing", p.Value, recurse))
                .OverrideAtom<Lambda>((lambda, assembler, overridden, recurse) => assembler.Lambda(lambda.LambdaString))
                .OverrideAtom<Cardinality>((enumValue, assembler, overridden, recurse) => assembler.Field(enumValue.Name))
                .OverrideAtom<Order>((enumValue, assembler, overridden, recurse) => assembler.Field(enumValue.Name))
                .OverrideAtom<Scope>((enumValue, assembler, overridden, recurse) => assembler.Field(enumValue.Name))
                .OverrideAtom<T>((enumValue, assembler, overridden, recurse) => assembler.Field(enumValue.Name))
                .OverrideAtom<HasValueStep>((step, assembler, overridden, recurse) =>
                {
                    assembler.Method(
                        "hasValue",
                        step.Argument is P.Eq eq
                            ? eq.Argument
                            : step.Argument,
                        recurse);
                })
                .OverrideAtom<AddEStep>((step, assembler, overridden, recurse) => assembler.Method("addE", step.Label, recurse))
                .OverrideAtom<AddVStep>((step, assembler, overridden, recurse) => assembler.Method("addV", step.Label, recurse))
                .OverrideAtom<AndStep>((step, assembler, overridden, recurse) => assembler.Method("and", step.Traversals.SelectMany(FlattenLogicalTraversals<AndStep>), recurse))
                .OverrideAtom<ByTraversalStep>((step, assembler, overridden, recurse) => assembler.Method("by", step.Traversal, step.Order, recurse))
                .OverrideAtom<ChooseTraversalStep>((step, assembler, overridden, recurse) =>
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
                .OverrideAtom<ChoosePredicateStep>((step, assembler, overridden, recurse) =>
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
                .OverrideAtom<CoalesceStep>((step, assembler, overridden, recurse) => assembler.Method("coalesce", step.Traversals, recurse))
                .OverrideAtom<CoinStep>((step, assembler, overridden, recurse) => assembler.Method("coin", step.Probability, recurse))
                .OverrideAtom<ConstantStep>((step, assembler, overridden, recurse) => assembler.Method("constant", step.Value, recurse))
                .OverrideAtom<BothStep>((step, assembler, overridden, recurse) => assembler.Method("both", step.Labels, recurse))
                .OverrideAtom<BothEStep>((step, assembler, overridden, recurse) => assembler.Method("bothE", step.Labels, recurse))
                .OverrideAtom<InStep>((step, assembler, overridden, recurse) => assembler.Method("in", step.Labels, recurse))
                .OverrideAtom<InEStep>((step, assembler, overridden, recurse) => assembler.Method("inE", step.Labels, recurse))
                .OverrideAtom<OutStep>((step, assembler, overridden, recurse) => assembler.Method("out", step.Labels, recurse))
                .OverrideAtom<OutEStep>((step, assembler, overridden, recurse) => assembler.Method("outE", step.Labels, recurse))
                .OverrideAtom<HasLabelStep>((step, assembler, overridden, recurse) => assembler.Method("hasLabel", step.Labels, recurse))
                .OverrideAtom<LabelStep>((step, assembler, overridden, recurse) => assembler.Method("label"))
                .OverrideAtom<EdgesStep>((step, assembler, overridden, recurse) => assembler.Method("edges", step.Traversal, recurse))
                .OverrideAtom<FromTraversalStep>((step, assembler, overridden, recurse) => assembler.Method("from", step.Traversal, recurse))
                .OverrideAtom<IdentifierStep>((step, assembler, overridden, recurse) => assembler.Identifier(step.Identifier))
                .OverrideAtom<IsStep>((step, assembler, overridden, recurse) =>
                {
                    assembler.Method(
                        "is",
                        step.Argument is P.Eq eq
                            ? eq.Argument
                            : step.Argument,
                        recurse);
                })
                .OverrideAtom<LimitStep>((step, assembler, overridden, recurse) =>
                {
                    if (step.Scope.Equals(Scope.Local))
                        assembler.Method("limit", step.Scope, step.Count, recurse);
                    else
                        assembler.Method("limit", step.Count, recurse);
                })
                .OverrideAtom<LocalStep>((step, assembler, overridden, recurse) => assembler.Method("local", step.Traversal, recurse))
                .OverrideAtom<MapStep>((step, assembler, overridden, recurse) => assembler.Method("map", step.Traversal, recurse))
                .OverrideAtom<NoneStep>((step, assembler, overridden, recurse) => assembler.Method("none"))
                .OverrideAtom<FlatMapStep>((step, assembler, overridden, recurse) => assembler.Method("flatMap", step.Traversal, recurse))
                .OverrideAtom<MatchStep>((step, assembler, overridden, recurse) => assembler.Method("match", step.Traversals, recurse))
                .OverrideAtom<NotStep>((step, assembler, overridden, recurse) =>
                {
                    var traversalSteps = step.Traversal.AsAdmin().Steps;

                    if (!(traversalSteps.Count != 0 && traversalSteps[traversalSteps.Count - 1] is HasStep hasStep && hasStep.Value is P p && p.EqualsConstant(false)))
                        assembler.Method("not", step.Traversal, recurse);
                })
                .OverrideAtom<OptionalStep>((step, assembler, overridden, recurse) => assembler.Method("optional", step.Traversal, recurse))
                .OverrideAtom<OrStep>((step, assembler, overridden, recurse) => assembler.Method("or", step.Traversals.SelectMany(FlattenLogicalTraversals<OrStep>), recurse))
                .OverrideAtom<ValueStep>((step, assembler, overridden, recurse) => assembler.Method("value"))
                .OverrideAtom<ValueMapStep>((step, assembler, overridden, recurse) => assembler.Method("valueMap", step.Keys, recurse))
                .OverrideAtom<ProjectStep.ByTraversalStep>((step, assembler, overridden, recurse) => assembler.Method("by", step.Traversal, recurse))
                .OverrideAtom<ProjectStep>((step, assembler, overridden, recurse) => assembler.Method("project", step.Projections, recurse));
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
