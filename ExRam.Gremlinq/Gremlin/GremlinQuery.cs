using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using LanguageExt;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace ExRam.Gremlinq
{
    internal sealed class GremlinQueryImpl<TElement, TOutVertex, TInVertex> :
        IVGremlinQuery<TElement>,
        IEGremlinQuery<TElement, TOutVertex>,
        IEGremlinQuery<TElement, TOutVertex, TInVertex>
    {
        public static readonly GremlinQueryImpl<TElement, TOutVertex, TInVertex> Anonymous = Create("__");

        public GremlinQueryImpl(IImmutableList<GremlinStep> steps, IImmutableDictionary<StepLabel, string> stepLabelBindings)
        {
            Steps = steps;
            StepLabelMappings = stepLabelBindings;
        }

        public static GremlinQueryImpl<TElement, TOutVertex, TInVertex> Create(string graphName = "g")
        {
            return new GremlinQueryImpl<TElement, TOutVertex, TInVertex>(ImmutableList<GremlinStep>.Empty.Add(new IdentifierGremlinStep(graphName)), ImmutableDictionary<StepLabel, string>.Empty);
        }

        IVGremlinQuery<TNewVertex> IGremlinQuery.AddV<TNewVertex>(TNewVertex vertex)
        {
            return AddStep<TNewVertex>(new AddVGremlinStep(vertex))
                .AddStep(new AddElementPropertiesStep(vertex));
        }

        IVGremlinQuery<TNewVertex> IGremlinQuery.AddV<TNewVertex>()
        {
            return ((IGremlinQuery<TElement>)this).AddV(new TNewVertex());
        }

        #region AddE
        IEGremlinQuery<TNewEdge> IGremlinQuery.AddE<TNewEdge>()
        {
            return AddE(new TNewEdge());
        }

        IEGremlinQuery<TNewEdge> IGremlinQuery.AddE<TNewEdge>(TNewEdge edge)
        {
            return AddE(edge);
        }

        IEGremlinQuery<TEdge, TElement> IVGremlinQuery<TElement>.AddE<TEdge>(TEdge edge)
        {
            return AddE(edge);
        }

        IEGremlinQuery<TNewEdge, TElement> IVGremlinQuery<TElement>.AddE<TNewEdge>()
        {
            return AddE(new TNewEdge());
        }

        private GremlinQueryImpl<TNewEdge, TElement, Unit> AddE<TNewEdge>(TNewEdge newEdge)
        {
            return AddStep<TNewEdge, TElement, Unit>(new AddEGremlinStep(newEdge))
                .AddStep(new AddElementPropertiesStep(newEdge));
        }
        #endregion

        IGremlinQuery<TElement> IGremlinQuery<TElement>.And(params Func<IGremlinQuery<TElement>, IGremlinQuery>[] andTraversals)
        {
            return And(andTraversals);
        }

        IVGremlinQuery<TElement> IVGremlinQuery<TElement>.And(params Func<IVGremlinQuery<TElement>, IGremlinQuery>[] andTraversals)
        {
            return And(andTraversals);
        }

        GremlinQueryImpl<TElement, TOutVertex, TInVertex> And(params Func<GremlinQueryImpl<TElement, TOutVertex, TInVertex>, IGremlinQuery>[] andTraversals)
        {
            return Call(
                "and",
                andTraversals
                    .Select(andTraversal => andTraversal(Anonymous))
                    .Aggregate(
                        ImmutableList<object>.Empty,
                        (list, query2) => query2.Steps.Count == 2 && (query2.Steps[1] as MethodGremlinStep)?.Name == "and"
                            ? list.AddRange(((MethodGremlinStep)query2.Steps[1]).Parameters)
                            : list.Add(query2)));
        }

        IGremlinQuery<TTarget> IGremlinQuery<TElement>.As<TTarget>(Func<IGremlinQuery<TElement>, StepLabel<TElement>, IGremlinQuery<TTarget>> continuation)
        {
            var stepLabel = new StepLabel<TElement>();

            return continuation(
                ((IGremlinQuery<TElement>)this).As(stepLabel),
                stepLabel);
        }

        IGremlinQuery<TElement> IGremlinQuery<TElement>.As(StepLabel<TElement> stepLabel)
        {
            return Call("as", stepLabel);
        }

        IGremlinQuery<TElement> IGremlinQuery<TElement>.Barrier()
        {
            return Call("barrier");
        }

        IGremlinQuery<TTarget> IGremlinQuery.Cast<TTarget>() => Cast<TTarget>();

        IVGremlinQuery<TOtherVertex> IVGremlinQuery<TElement>.Cast<TOtherVertex>() => Cast<TOtherVertex>();

        IEGremlinQuery<TOtherEdge, TOutVertex> IEGremlinQuery<TElement, TOutVertex>.Cast<TOtherEdge>() => Cast<TOtherEdge>();

        IEGremlinQuery<TOtherEdge> IEGremlinQuery<TElement>.Cast<TOtherEdge>() => Cast<TOtherEdge>();

        IEGremlinQuery<TOtherEdge, TOutVertex, TInVertex> IEGremlinQuery<TElement, TOutVertex, TInVertex>.Cast<TOtherEdge>() => Cast<TOtherEdge>();

        private GremlinQueryImpl<TTarget, TOutVertex, TInVertex> Cast<TTarget>()
        {
            return new GremlinQueryImpl<TTarget, TOutVertex, TInVertex>(Steps, StepLabelMappings);
        }

        //public IEGremlinQuery<TTarget, Unit, TElement> CastOuTNewVertex<TTarget>()
        //{
        //    return new GremlinQueryImpl<TTarget, TInVertex, TElement>(this.Steps, this.StepLabelMappings);
        //}

        //public IEGremlinQuery<TOuTNewVertex, TTarget, TElement> CastInVertex<TTarget>()
        //{
        //    return new GremlinQueryImpl<TOuTNewVertex, TTarget, TElement>(this.Steps, this.StepLabelMappings);
        //}

        IGremlinQuery<TTarget> IGremlinQuery<TElement>.Coalesce<TTarget>(params Func<IGremlinQuery<TElement>, IGremlinQuery<TTarget>>[] traversals)
        {
            return Call<TTarget>(
                "coalesce",
                traversals
                   .Select(traversal => traversal(Anonymous))
                   .ToImmutableList<object>());
        }

        IGremlinQuery<TResult> IGremlinQuery<TElement>.Choose<TResult>(Func<IGremlinQuery<TElement>, IGremlinQuery> traversalPredicate, Func<IGremlinQuery<TElement>, IGremlinQuery<TResult>> trueChoice, Func<IGremlinQuery<TElement>, IGremlinQuery<TResult>> falseChoice)
        {
            var anonymous = Anonymous;

            return Call<TResult>(
                "choose",
                traversalPredicate(anonymous),
                trueChoice(anonymous),
                falseChoice(anonymous));
        }

        IGremlinQuery<TResult> IGremlinQuery<TElement>.Choose<TResult>(Func<IGremlinQuery<TElement>, IGremlinQuery> traversalPredicate, Func<IGremlinQuery<TElement>, IGremlinQuery<TResult>> trueChoice)
        {
            var anonymous = Anonymous;

            return Call<TResult>(
                "choose",
                traversalPredicate(anonymous),
                trueChoice(anonymous));
        }

        IVGremlinQuery<Vertex> IVGremlinQuery<TElement>.Both<TNewEdge>()
        {
            return AddStep<Vertex>(new DerivedLabelNamesGremlinStep<TNewEdge>("both"));
        }

        IEGremlinQuery<TNewEdge> IVGremlinQuery<TElement>.BothE<TNewEdge>()
        {
            return AddStep<TNewEdge>(new DerivedLabelNamesGremlinStep<TNewEdge>("bothE"));
        }

        IVGremlinQuery<Vertex> IEGremlinQuery<TElement>.BothV()
        {
            return Call<Vertex>("bothV");
        }

        //IGremlinQuery<TEnd> IGremlinQuery<TElement>.BranchOnIdentity<TEnd>(params Func<IGremlinQuery<TElement>, IGremlinQuery<TEnd>>[] options)
        //{
        //    return ((IGremlinQuery<TElement>)this)
        //        .Branch(_ => _.Identity(), options);
        //}

        //IGremlinQuery<TEnd> IGremlinQuery<TElement>.Branch<TBranch, TEnd>(Func<IGremlinQuery<TElement>, IGremlinQuery<TBranch>> branchSelector, params Func<IVGremlinQuery<TElement>, IGremlinQuery<TEnd>>[] options)
        //{
        //    return options
        //        .Aggregate(
        //            this
        //                .Call<TBranch>("branch", branchSelector(this.Anonymous)),
        //            (branchQuery, option) => branchQuery.Call<TBranch>("option", option(branchQuery.Anonymous)))
        //        .Cast<TEnd>();
        //}

        //IGremlinQuery<TTarget> IVGremlinQuery<TElement>.Branch<TBranch, TTarget>(Func<IVGremlinQuery<TElement>, IGremlinQuery<TBranch>> branchSelector, params Func<IVGremlinQuery<TElement>, IGremlinQuery<TTarget>>[] options)
        //{
        //    return options
        //        .Aggregate(
        //            this
        //                .Call<TBranch>("branch", branchSelector(this.Anonymous)),
        //            (branchQuery, option) => branchQuery.Call<TBranch>("option", option(branchQuery.Anonymous)))
        //        .Cast<TTarget>();
        //}

        //private GremlinQueryImpl<TTarget, TE, TInVertex> Branch<TBranch, TTarget>(Func<IVGremlinQuery<TElement>, IGremlinQuery<TBranch>> branchSelector, params Func<IGremlinQuery<TElement>, IGremlinQuery<TTarget>>[] options)
        //{
        //    return options
        //        .Aggregate(
        //            this
        //                .Call<TBranch>("branch", branchSelector(this.Anonymous)),
        //            (branchQuery, option) => branchQuery.Call<TBranch>("option", option(branchQuery.Anonymous)))
        //        .Cast<TTarget>();
        //}

        IGremlinQuery<TElement> IGremlinQuery<TElement>.ByTraversal(Func<IGremlinQuery<TElement>, IGremlinQuery> traversal, Order sortOrder)
        {
            return Call("by", traversal(Anonymous), sortOrder);
        }

        IGremlinQuery<TElement> IGremlinQuery<TElement>.ByMember(Expression<Func<TElement, object>> projection, Order sortOrder)
        {
            var body = projection.Body;
            if (body is UnaryExpression && body.NodeType == ExpressionType.Convert)
                body = ((UnaryExpression)body).Operand;

            if (body is MemberExpression memberExpression)
            {
                return Call("by", memberExpression.Member.Name, sortOrder);
            }

            throw new NotSupportedException();
        }

        IGremlinQuery<TElement> IGremlinQuery<TElement>.ByLambda(string lambdaString)
        {
            return Call("by", new Lambda(lambdaString));
        }

        IGremlinQuery<TElement> IGremlinQuery<TElement>.Dedup()
        {
            return Call("dedup");
        }

        IGremlinQuery<Unit> IGremlinQuery.Drop()
        {
            return Call<Unit>("drop");
        }

        IEGremlinQuery<Edge> IGremlinQuery.E(params object[] ids)
        {
            return Call<Edge>("E", ids);
        }

        IGremlinQuery<TElement> IGremlinQuery<TElement>.Emit()
        {
            return Call("emit");
        }

        IGremlinQuery<TElement> IGremlinQuery<TElement>.Explain()
        {
            return Call("explain");
        }

        IGremlinQuery<TElement> IGremlinQuery<TElement>.FilterWithLambda(string lambda)
        {
            return Call("filter", new Lambda(lambda));
        }

        IGremlinQuery<TElement[]> IGremlinQuery<TElement>.Fold()
        {
            return Call<TElement[]>("fold");
        }

        IGremlinQuery<TElement> IGremlinQuery<TElement>.From<TStepLabel>(StepLabel<TStepLabel> stepLabel)
        {
            return Call("from", stepLabel);
        }

        IEGremlinQuery<TElement, TTargetVertex, TOutVertex> IEGremlinQuery<TElement, TOutVertex>.From<TTargetVertex>(Func<IGremlinQuery<TElement>, IGremlinQuery<TTargetVertex>> fromVertexTraversal)
        {
            return Call<TElement, TTargetVertex, TOutVertex>("from", fromVertexTraversal(Anonymous));
        }

        //IEGremlinQuery<TElement, TTargetVertex, TAdjacentVertex> From<TTargetVertex>(Func<IGremlinQuery<TEdge>, IGremlinQuery<TTargetVertex>> fromVertexTraversal);
        //{
        //    return Call("from", fromVertex(Anonymous));
        //}

        IGremlinQuery<object> IGremlinQuery.Id()
        {
            return Call<object>("id");
        }

        IGremlinQuery<TElement> IGremlinQuery<TElement>.Identity()
        {
            return Call("identity");
        }

        IVGremlinQuery<Vertex> IVGremlinQuery<TElement>.In<TNewEdge>()
        {
            return AddStep<Vertex>(new DerivedLabelNamesGremlinStep<TNewEdge>("in"));
        }

        IGremlinQuery<TTarget> IGremlinQuery.InsertStep<TTarget>(int index, GremlinStep step)
        {
            return new GremlinQueryImpl<TTarget, TOutVertex, TInVertex>(Steps.Insert(index, step), StepLabelMappings);
        }

        IEGremlinQuery<TNewEdge> IVGremlinQuery<TElement>.InE<TNewEdge>()
        {
            return AddStep<TNewEdge>(new DerivedLabelNamesGremlinStep<TNewEdge>("inE"));
        }

        IVGremlinQuery<Vertex> IEGremlinQuery<TElement>.InV()
        {
            return Call<Vertex, Unit, Unit>("inV");
        }

        IVGremlinQuery<TInVertex> IEGremlinQuery<TElement, TOutVertex, TInVertex>.InV()
        {
            return Call<TInVertex, Unit, Unit>("inV");
        }

        IGremlinQuery<TElement> IGremlinQuery<TElement>.Inject(params TElement[] elements)
        {
            return Call("inject", elements);
        }

        IGremlinQuery<TElement> IGremlinQuery<TElement>.Limit(long limit)
        {
            return Call("limit", limit);
        }

        IGremlinQuery<TTarget> IGremlinQuery<TElement>.Local<TTarget>(Func<IGremlinQuery<TElement>, IGremlinQuery<TTarget>> localTraversal)
        {
            return Call<TTarget>("local", localTraversal(Anonymous));
        }

        IGremlinQuery<TTarget> IGremlinQuery<TElement>.Map<TTarget>(Func<IGremlinQuery<TElement>, IGremlinQuery<TTarget>> mapping)
        {
            return Call<TTarget>("map", mapping(Anonymous));
        }

        IGremlinQuery<TElement> IGremlinQuery<TElement>.Match(params Func<IGremlinQuery<TElement>, IGremlinQuery<TElement>>[] matchTraversals)
        {
            return // ReSharper disable once CoVariantArrayConversion
                Call("match", matchTraversals.Select(traversal => traversal(Anonymous)).ToArray());
        }

        IGremlinQuery<TElement> IGremlinQuery<TElement>.Not(Func<IGremlinQuery<TElement>, IGremlinQuery> notTraversal)
        {
            return Not(notTraversal);
        }

        IVGremlinQuery<TElement> IVGremlinQuery<TElement>.Not(Func<IVGremlinQuery<TElement>, IGremlinQuery> notTraversal)
        {
            return Not(notTraversal);
        }

        private GremlinQueryImpl<TElement, TOutVertex, TInVertex> Not(Func<IVGremlinQuery<TElement>, IGremlinQuery> notTraversal)
        {
            return Call("not", notTraversal(Anonymous));
        }

        IGremlinQuery<TTarget> IGremlinQuery.OfType<TTarget>() => OfType<TTarget>();

        IVGremlinQuery<TTarget> IVGremlinQuery<TElement>.OfType<TTarget>() => OfType<TTarget>();

        IEGremlinQuery<TTarget, TOutVertex> IEGremlinQuery<TElement, TOutVertex>.OfType<TTarget>() => OfType<TTarget>();

        IEGremlinQuery<TTarget> IEGremlinQuery<TElement>.OfType<TTarget>() => OfType<TTarget>();

        IEGremlinQuery<TTarget, TOutVertex, TInVertex> IEGremlinQuery<TElement, TOutVertex, TInVertex>.OfType<TTarget>() => OfType<TTarget>();

        private GremlinQueryImpl<TTarget, TOutVertex, TInVertex> OfType<TTarget>()
        {
            return AddStep<TTarget>(new DerivedLabelNamesGremlinStep<TTarget>("hasLabel"));
        }

        IGremlinQuery<TTarget> IVGremlinQuery<TElement>.Optional<TTarget>(Func<IVGremlinQuery<TElement>, IGremlinQuery<TTarget>> optionalTraversal)
        {
            return Call<TTarget>("optional", optionalTraversal(Anonymous));
        }

        IGremlinQuery<TElement> IGremlinQuery<TElement>.Or(params Func<IGremlinQuery<TElement>, IGremlinQuery>[] orTraversals)
        {
            return Call(
                "or",
                orTraversals
                    .Select(andTraversal => andTraversal(Anonymous))
                    .Aggregate(
                        ImmutableList<object>.Empty,
                        (list, query2) => query2.Steps.Count == 2 && (query2.Steps[1] as MethodGremlinStep)?.Name == "or"
                            ? list.AddRange(((MethodGremlinStep)query2.Steps[1]).Parameters)
                            : list.Add(query2)));
        }

        IGremlinQuery<TElement> IGremlinQuery<TElement>.Order()
        {
            return Call("order");
        }

        IVGremlinQuery<Vertex> IEGremlinQuery<TElement>.OtherV()
        {
            return Call<Vertex>("otherV");
        }

        IEGremlinQuery<TNewEdge> IVGremlinQuery<TElement>.OutE<TNewEdge>()
        {
            return AddStep<TNewEdge>(new DerivedLabelNamesGremlinStep<TNewEdge>("outE"));
        }

        IVGremlinQuery<Vertex> IEGremlinQuery<TElement>.OutV()
        {
            return Call<Vertex, Unit, Unit>("outV");
        }

        IVGremlinQuery<TOutVertex> IEGremlinQuery<TElement, TOutVertex, TInVertex>.OutV()
        {
            return Call<TOutVertex, Unit, Unit>("outV");
        }

        IVGremlinQuery<Vertex> IVGremlinQuery<TElement>.Out<TNewEdge>()
        {
            return AddStep<Vertex>(new DerivedLabelNamesGremlinStep<TNewEdge>("out"));
        }

        IGremlinQuery<string> IGremlinQuery<TElement>.Profile()
        {
            return Call<string>("profile");
        }

        IGremlinQuery<TElement> IGremlinQuery<TElement>.Property<TProperty>(Expression<Func<TElement, TProperty>> propertyExpression, TProperty property)
        {
            if (propertyExpression.Body is MemberExpression memberExpression)
            {
                if (memberExpression.Expression == propertyExpression.Parameters[0])
                {
                    return ((IGremlinQuery<TElement>)this).Property(memberExpression.Member.Name, property);
                }
            }

            throw new NotSupportedException();
        }

        IGremlinQuery<TElement> IGremlinQuery<TElement>.Property(string key, object value)
        {
            return Call("property", key, value);
        }

        IGremlinQuery<TElement> IGremlinQuery<TElement>.Range(long low, long high)
        {
            return Call("range", low, high);
        }

        IVGremlinQuery<TElement> IVGremlinQuery<TElement>.Repeat(Func<IVGremlinQuery<TElement>, IVGremlinQuery<TElement>> repeatTraversal)
        {
            return Call("repeat", repeatTraversal(Anonymous));
        }

        IGremlinQuery<TStep> IGremlinQuery.Select<TStep>(StepLabel<TStep> label)
        {
            return Call<TStep>("select", label);
        }

        IGremlinQuery<(T1, T2)> IGremlinQuery.Select<T1, T2>(StepLabel<T1> label1, StepLabel<T2> label2)
        {
            return Call<(T1, T2)>("select", label1, label2)
                .AddStepLabelBinding(x => x.Item1, label1)
                .AddStepLabelBinding(x => x.Item2, label2);
        }

        IGremlinQuery<(T1, T2, T3)> IGremlinQuery.Select<T1, T2, T3>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3)
        {
            return Call<(T1, T2, T3)>("select", label1, label2, label3)
                .AddStepLabelBinding(x => x.Item1, label1)
                .AddStepLabelBinding(x => x.Item2, label2)
                .AddStepLabelBinding(x => x.Item3, label3);
        }

        IGremlinQuery<(T1, T2, T3, T4)> IGremlinQuery.Select<T1, T2, T3, T4>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3, StepLabel<T4> label4)
        {
            return Call<(T1, T2, T3, T4)>("select", label1, label2, label3, label4)
                .AddStepLabelBinding(x => x.Item1, label1)
                .AddStepLabelBinding(x => x.Item2, label2)
                .AddStepLabelBinding(x => x.Item3, label3)
                .AddStepLabelBinding(x => x.Item4, label4);
        }

        IGremlinQuery<TElement> IGremlinQuery<TElement>.SideEffect(Func<IGremlinQuery<TElement>, IGremlinQuery> sideEffectTraversal)
        {
            return Call("sideEffect", sideEffectTraversal(Anonymous));
        }

        IGremlinQuery<TElement> IGremlinQuery<TElement>.Skip(long skip)
        {
            return Call("skip", skip);
        }

        IGremlinQuery<TElement> IGremlinQuery<TElement>.Sum(Scope scope)
        {
            return Call("sum", scope);
        }

        IGremlinQuery<TElement> IGremlinQuery<TElement>.Times(int count)
        {
            return Call("times", count);
        }

        IGremlinQuery<TElement> IGremlinQuery<TElement>.Tail(long limit)
        {
            return Call("tail", limit);
        }

        IGremlinQuery<TElement> IGremlinQuery<TElement>.To<TStepLabel>(StepLabel<TStepLabel> stepLabel)
        {
            return Call("to", stepLabel);
        }

        IEGremlinQuery<TElement, TOutVertex, TTargetVertex> IEGremlinQuery<TElement, TOutVertex>.To<TTargetVertex>(Func<IGremlinQuery<TElement>, IGremlinQuery<TTargetVertex>> toVertexTraversal)
        {
            return Call<TElement, TOutVertex, TTargetVertex>("to", toVertexTraversal(Anonymous));
        }

        //IEGremlinQuery<TElement, TOutVertex, TTargetVertex> To<TTargetVertex>(Func<IGremlinQuery<TElement>, IGremlinQuery<TTargetVertex>> toVertexTraversal)
        //{
        //    return Call("to", toVertex(Anonymous));
        //}

        IGremlinQuery<TElement> IGremlinQuery<TElement>.Unfold(IGremlinQuery<IEnumerable<TElement>> query)
        {
            throw new NotImplementedException();    //Bug!
                                                    //return query
                                                    //    .Call<TElement>("unfold");
        }

        IGremlinQuery<TTarget> IVGremlinQuery<TElement>.Union<TTarget>(params Func<IVGremlinQuery<TElement>, IGremlinQuery<TTarget>>[] unionTraversals)
        {
            return Call<TTarget>("union", unionTraversals
                .Select(unionTraversal => unionTraversal(Anonymous))
                .ToImmutableList<object>());
        }

        IGremlinQuery<TElement> IGremlinQuery<TElement>.Until(Func<IGremlinQuery<TElement>, IGremlinQuery> untilTraversal)
        {
            return Call("until", untilTraversal(Anonymous));
        }

        IVGremlinQuery<Vertex> IGremlinQuery.V(params object[] ids)
        {
            return Call<Vertex>("V", ids);
        }

        IGremlinQuery<TTarget> IGremlinQuery<TElement>.Values<TTarget>(params Expression<Func<TElement, TTarget>>[] projections)
        {
            return AddStep<TTarget>(new ValuesGremlinStep<TElement, TTarget>(projections));
        }

        IGremlinQuery<TElement> IGremlinQuery<TElement>.Where(Func<IGremlinQuery<TElement>, IGremlinQuery> filterTraversal)
        {
            return Call("where", filterTraversal(Anonymous));
        }

        GroovyExpressionState IGroovySerializable.Serialize(StringBuilder stringBuilder, GroovyExpressionState state)
        {
            foreach (var step in Steps)
            {
                if (step is IGroovySerializable serializableStep)
                    state = serializableStep.Serialize(stringBuilder, state);
                else
                    throw new ArgumentException("Query contains non-serializable step. Please call RewriteSteps on the query first.");
            }

            return state;
        }

        private GremlinQueryImpl<TElement, TOutVertex, TInVertex> Call(string name, params object[] parameters)
        {
            return Call<TElement>(name, parameters.ToImmutableList());
        }

        private GremlinQueryImpl<TTarget, TOutVertex, TInVertex> Call<TTarget>(string name, params object[] parameters)
        {
            return Call<TTarget>(name, parameters.ToImmutableList());
        }

        private GremlinQueryImpl<TTarget, TNewOutVertex, TNewInVertex> Call<TTarget, TNewOutVertex, TNewInVertex>(string name, params object[] parameters)
        {
            return Call<TTarget, TNewOutVertex, TNewInVertex>(name, parameters.ToImmutableList());
        }

        private GremlinQueryImpl<TElement, TOutVertex, TInVertex> Call(string name, ImmutableList<object> parameters)
        {
            return AddStep<TElement>(new MethodGremlinStep(name, parameters));
        }

        private GremlinQueryImpl<TTarget, TOutVertex, TInVertex> Call<TTarget>(string name, ImmutableList<object> parameters)
        {
            return AddStep<TTarget>(new MethodGremlinStep(name, parameters));
        }

        private GremlinQueryImpl<TTarget, TNewOutVertex, TNewInVertex> Call<TTarget, TNewOutVertex, TNewInVertex>(string name, ImmutableList<object> parameters)
        {
            return AddStep<TTarget, TNewOutVertex, TNewInVertex>(new MethodGremlinStep(name, parameters));
        }

        private GremlinQueryImpl<TElement, TOutVertex, TInVertex> AddStep(GremlinStep step)
        {
            return AddStep<TElement>(step);
        }

        private GremlinQueryImpl<TNewElement, TOutVertex, TInVertex> AddStep<TNewElement>(GremlinStep step)
        {
            return AddStep<TNewElement, TOutVertex, TInVertex>(step);
        }

        private GremlinQueryImpl<TNewElement, TNewOutVertex, TNewInVertex> AddStep<TNewElement, TNewOutVertex, TNewInVertex>(GremlinStep step)
        {
            return new GremlinQueryImpl<TNewElement, TNewOutVertex, TNewInVertex>(Steps.Insert(Steps.Count, step), StepLabelMappings);
        }

        public IImmutableList<GremlinStep> Steps { get; }
        public IImmutableDictionary<StepLabel, string> StepLabelMappings { get; }
    }

    internal static class GremlinQuery<TElement>
    {
        public static IGremlinQuery<TElement> Create(string graphName = "g")
        {
            return new GremlinQueryImpl<TElement, Unit, Unit>(ImmutableList<GremlinStep>.Empty.Add(new IdentifierGremlinStep(graphName)), ImmutableDictionary<StepLabel, string>.Empty);
        }
    }

    public static class GremlinQuery
    {
        public static readonly IGremlinQuery<Unit> Anonymous = GremlinQueryImpl<Unit, Unit, Unit>.Anonymous;

        public static IGremlinQuery<Unit> Create(string graphName)
        {
            return GremlinQuery<Unit>.Create(graphName);
        }

        public static IGremlinQuery<TElement> Create<TElement>(string graphName)
        {
            return GremlinQuery<TElement>.Create(graphName);
        }

        public static IGremlinQuery<TElement> SetModel<TElement>(this IGremlinQuery<TElement> query, IGraphModel model)
        {
            return query
                .AddStep(new SetModelGremlinStep(model));
        }

        public static IGremlinQuery<TElement> SetTypedGremlinQueryProvider<TElement>(this IGremlinQuery<TElement> query, ITypedGremlinQueryProvider queryProvider)
        {
            return query
                .AddStep(new SetTypedGremlinQueryProviderGremlinStep(queryProvider));
        }

        public static (string queryString, IDictionary<string, object> parameters) Serialize(this IGremlinQuery query)
        {
            var stringBuilder = new StringBuilder();

            var groovyBuilder = query
                .Serialize(stringBuilder, GroovyExpressionState.FromQuery(query));

            return (stringBuilder.ToString(), groovyBuilder.GetVariables());
        }

        public static Task<TElement> FirstAsync<TElement>(this IGremlinQuery<TElement> query, CancellationToken ct = default)
        {
            return query
                .Limit(1)
                .Execute()
                .First(ct);
        }

        public static async Task<Option<TElement>> FirstOrNoneAsync<TElement>(this IGremlinQuery<TElement> query, CancellationToken ct = default)
        {
            var array = await query
                .Limit(1)
                .Execute()
                .ToArray(ct)
                .ConfigureAwait(false);

            return array.Length > 0
                ? array[0]
                : Option<TElement>.None;
        }

        public static Task<TElement[]> ToArrayAsync<TElement>(this IGremlinQuery<TElement> query, CancellationToken ct = default)
        {
            return query
                .Execute()
                .ToArray(ct);
        }

        public static IGremlinQuery<TElement> Call<TElement>(this IGremlinQuery<TElement> query, string name, params object[] parameters)
        {
            return query.Call(name, parameters.ToImmutableList());
        }

        public static IGremlinQuery<TElement> Call<TElement>(this IGremlinQuery<TElement> query, string name, ImmutableList<object> parameters)
        {
            return query.InsertStep<TElement>(query.Steps.Count, new MethodGremlinStep(name, parameters));
        }

        internal static IGremlinQuery<TElement> AddStep<TElement>(this IGremlinQuery<TElement> query, GremlinStep step)
        {
            return query.InsertStep<TElement>(query.Steps.Count, step);
        }
        
        public static IGremlinQuery<TNewEdge> E<TNewEdge>(this IGremlinQuery query, params object[] ids)
        {
            return query
                .E(ids)
                .OfType<TNewEdge>();
        }

        public static IGremlinQuery<TElement> ReplaceSteps<TElement>(this IGremlinQuery<TElement> query, IImmutableList<GremlinStep> steps)
        {
            return ReferenceEquals(steps, query.Steps)
                ? query 
                : new GremlinQueryImpl<TElement, Unit, Unit>(steps, query.StepLabelMappings);
        }

        public static IGremlinQuery<TElement> Resolve<TElement>(this IGremlinQuery<TElement> query)
        {
            var model = query
                .TryGetModel()
                .IfNone(() => throw new ArgumentException("No IGraphModel set on the query."));

            return query
                .RewriteSteps(x => Option<IEnumerable<GremlinStep>>.Some(x.Resolve(model)))
                .Cast<TElement>();
        }

        public static IGremlinQuery<Unit> RewriteSteps(this IGremlinQuery query, Func<NonTerminalGremlinStep, Option<IEnumerable<GremlinStep>>> resolveFunction)
        {
            var steps = query.Steps;

            for(var index = 0; index < steps.Count; index++)
            {
                var step = steps[index];

                switch (step)
                {
                    case MethodGremlinStep terminal:
                    {
                        var parameters = terminal.Parameters;

                        for (var j = 0; j < parameters.Count; j++)
                        {
                            var parameter = parameters[j];

                            if (parameter is IGremlinQuery subQuery)
                                parameters = parameters.SetItem(j, subQuery.RewriteSteps(resolveFunction));
                        }

                        if (!object.ReferenceEquals(parameters, terminal.Parameters))
                            steps = steps.SetItem(index, new MethodGremlinStep(terminal.Name, parameters));

                        break;
                    }
                    case NonTerminalGremlinStep nonTerminal:
                    {
                        var newTuple = resolveFunction(nonTerminal)
                            .Fold(
                                (steps, index),
                                (tuple, newSteps) => (
                                    tuple.steps
                                        .RemoveAt(tuple.index)
                                        .InsertRange(tuple.index, newSteps),
                                    tuple.index - 1));

                        index = newTuple.index;
                        steps = newTuple.steps;

                        break;
                    }
                }
            }

            return query
                .Cast<Unit>()
                .ReplaceSteps(steps);
        }

        public static IVGremlinQuery<TNewVertex> V<TNewVertex>(this IGremlinQuery query, params object[] ids)
        {
            return query
                .V(ids)
                .OfType<TNewVertex>();
        }

        internal static IGremlinQuery<TElement> AddStepLabelBinding<TElement>(this IGremlinQuery<TElement> query, Expression<Func<TElement, object>> memberExpression, StepLabel stepLabel)
        {
            var body = memberExpression.Body.StripConvert();
            
            if (!(body is MemberExpression memberExpressionBody))
                throw new ArgumentException();

            return new GremlinQueryImpl<TElement, Unit, Unit>(query.Steps, query.StepLabelMappings.SetItem(stepLabel, memberExpressionBody.Member.Name));
        }

        internal static IGremlinQuery<TElement> ReplaceProvider<TElement>(this IGremlinQuery<TElement> query, ITypedGremlinQueryProvider provider)
        {
            return new GremlinQueryImpl<TElement, Unit, Unit>(query.Steps, query.StepLabelMappings);
        }

        internal static IGremlinQuery<TElement> Where<TElement>(this IGremlinQuery<TElement> query, ParameterExpression parameter, Expression left, Expression right, ExpressionType nodeType)
        {
            if (nodeType == ExpressionType.OrElse || nodeType == ExpressionType.AndAlso)
            {
                var leftLambda = Expression.Lambda<Func<TElement, bool>>(left, parameter);
                var rightLambda = Expression.Lambda<Func<TElement, bool>>(right, parameter);

                return nodeType == ExpressionType.OrElse
                    ? query
                        .Or(
                            _ => _.Where(leftLambda),
                            _ => _.Where(rightLambda))
                    : query
                        .And(
                            _ => _.Where(leftLambda),
                            _ => _.Where(rightLambda));
            }

            return query.Where(parameter, left, right.GetValue(), nodeType);
        }

        internal static IGremlinQuery<TElement> Where<TElement>(this IGremlinQuery<TElement> query, ParameterExpression parameter, Expression left, object rightConstant, ExpressionType nodeType)
        {
            if (rightConstant == null)
            {
                // ReSharper disable once SwitchStatementMissingSomeCases
                switch (nodeType)
                {
                    case ExpressionType.Equal:
                        return query.Not(__ => __.Has(left));
                    case ExpressionType.NotEqual:
                        return query.Has(left);
                }
            }
            else
            {
                var predicateArgument = P.ForExpressionType(nodeType, rightConstant);

                switch (left)
                {
                    case MemberExpression leftMemberExpression when parameter == leftMemberExpression.Expression:
                    {
                        return query.Has(
                            leftMemberExpression,
                            rightConstant is StepLabel
                                ? GremlinQuery
                                    .Anonymous
                                    .Call("where", predicateArgument)
                                : (object)predicateArgument);
                    }
                    case ParameterExpression leftParameterExpression when parameter == leftParameterExpression:
                    {
                        return query.Call(
                            rightConstant is StepLabel
                                ? "where"
                                : "is",
                            predicateArgument);
                    }
                }
            }

            throw new NotSupportedException();
        }

        public static IGremlinQuery<TElement> Where<TElement>(this IGremlinQuery<TElement> query, Expression<Func<TElement, bool>> predicate)
        {
            var body = predicate.Body;

            switch (body)
            {
                case UnaryExpression unaryExpression:
                    {
                        if (unaryExpression.NodeType == ExpressionType.Not)
                            return query.Not(_ => _.Where(Expression.Lambda<Func<TElement, bool>>(unaryExpression.Operand, predicate.Parameters)));

                        break;
                    }
                case MemberExpression memberExpression:
                    {
                        if (memberExpression.Member is PropertyInfo property)
                        {
                            if (property.PropertyType == typeof(bool))
                                return query.Where(predicate.Parameters[0], memberExpression, Expression.Constant(true), ExpressionType.Equal);
                        }

                        break;
                    }
                case BinaryExpression binaryExpression:
                    return query.Where(predicate.Parameters[0], binaryExpression.Left.StripConvert(), binaryExpression.Right.StripConvert(), binaryExpression.NodeType);
                case MethodCallExpression methodCallExpression:
                    {
                        var methodInfo = methodCallExpression.Method;

                        if (methodInfo.DeclaringType == typeof(Enumerable))
                        {
                            // ReSharper disable once SwitchStatementMissingSomeCases
                            switch (methodInfo.Name)
                            {
                                case nameof(Enumerable.Contains) when methodInfo.GetParameters().Length == 2:
                                    {
                                        if (methodCallExpression.Arguments[0] is MemberExpression leftMember && leftMember.Expression == predicate.Parameters[0])
                                            return query.Has(leftMember, P.Eq(methodCallExpression.Arguments[1].GetValue()));

                                        if (methodCallExpression.Arguments[1] is MemberExpression rightMember && rightMember.Expression == predicate.Parameters[0])
                                        {
                                            if (methodCallExpression.Arguments[0].GetValue() is IEnumerable enumerable)
                                                return query.Has(rightMember, P.Within(enumerable.Cast<object>().ToArray()));
                                        }

                                        throw new NotSupportedException();
                                    }
                                case nameof(Enumerable.Any) when methodInfo.GetParameters().Length == 1:
                                    return query.Where(predicate.Parameters[0], methodCallExpression.Arguments[0], Expression.Constant(null, methodCallExpression.Arguments[0].Type), ExpressionType.NotEqual);
                            }
                        }
                        else if (methodInfo.DeclaringType == typeof(EnumerableExtensions))
                        {
                            if (methodInfo.Name == nameof(EnumerableExtensions.Intersects) && methodInfo.GetParameters().Length == 2)
                            {
                                if (methodCallExpression.Arguments[0] is MemberExpression innerMemberExpression)
                                {
                                    var constant = methodCallExpression.Arguments[1].GetValue();

                                    if (constant is IEnumerable arrayConstant)
                                        return query.Has(innerMemberExpression, P.Within(arrayConstant.Cast<object>().ToArray()));
                                }
                            }
                        }
                        else if (methodInfo.DeclaringType == typeof(string))
                        {
                            if (methodInfo.Name == nameof(string.StartsWith))
                            {
                                if (methodCallExpression.Arguments[0] is MemberExpression argumentExpression && argumentExpression.Expression == predicate.Parameters[0])
                                {
                                    if (methodCallExpression.Object.GetValue() is string stringValue)
                                    {
                                        return query.Has(
                                            argumentExpression,
                                            P.Within(Enumerable
                                                .Range(0, stringValue.Length + 1)
                                                .Select(i => stringValue.Substring(0, i))
                                                .Cast<object>()
                                                .ToArray()));
                                    }
                                }
                                else if (methodCallExpression.Object is MemberExpression memberExpression && memberExpression.Expression == predicate.Parameters[0])
                                {
                                    if (methodCallExpression.Arguments[0].GetValue() is string lowerBound)
                                    {
                                        string upperBound;

                                        if (lowerBound.Length == 0 || lowerBound[lowerBound.Length - 1] == char.MaxValue)
                                            upperBound = lowerBound + char.MinValue;
                                        else
                                        {
                                            var upperBoundChars = lowerBound.ToCharArray();
                                            
                                            upperBoundChars[upperBoundChars.Length - 1]++;
                                            upperBound = new string(upperBoundChars);
                                        }

                                        return query.Has(memberExpression, P.Between(lowerBound, upperBound));
                                    }
                                }
                            }
                        }

                        break;
                    }
            }

            throw new NotSupportedException();
        }

        internal static IGremlinQuery<TElement> Has<TElement>(this IGremlinQuery<TElement> query, Expression expression, Option<object> maybeArgument = default)
        {
            string name;

            switch (expression)
            {
                case MemberExpression leftMemberExpression:
                {
                    name = leftMemberExpression.Member.Name;
                    break;
                }
                case ParameterExpression leftParameterExpression:
                {
                    name = leftParameterExpression.Name;
                    break;
                }
                default:
                    throw new NotSupportedException();
            }

            return query.AddStep(new HasStep(name, maybeArgument));
        }

        public static IGremlinQuery<Unit> WithSubgraphStrategy(this IGremlinQuery<Unit> query, Func<IGremlinQuery<Unit>, IGremlinQuery> vertexCriterion, Func<IGremlinQuery<Unit>, IGremlinQuery> edgeCriterion)
        {
            var vertexCriterionTraversal = vertexCriterion(GremlinQuery.Anonymous);
            var edgeCriterionTraversal = edgeCriterion(GremlinQuery.Anonymous);

            if (vertexCriterionTraversal.Steps.Count > 1 || edgeCriterionTraversal.Steps.Count > 1)
            {
                var strategy = GremlinQuery
                    .Create("SubgraphStrategy")
                    .Call("build");

                if (vertexCriterionTraversal.Steps.Count > 0)
                    strategy = strategy.Call("vertices", vertexCriterionTraversal);

                if (edgeCriterionTraversal.Steps.Count > 0)
                    strategy = strategy.Call("edges", edgeCriterionTraversal);

                return query.AddStep(new MethodGremlinStep("withStrategies", strategy.Call("create")));
            }

            return query;
        }

        internal static Option<ITypedGremlinQueryProvider> TryGetTypedGremlinQueryProvider(this IGremlinQuery query)
        {
            return query
                .Steps
                .OfType<SetTypedGremlinQueryProviderGremlinStep>()
                .Select(x => Option<ITypedGremlinQueryProvider>.Some(x.TypedGremlinQueryProvider))
                .LastOrDefault();
        }

        internal static Option<IGraphModel> TryGetModel(this IGremlinQuery query)
        {
            return query
                .Steps
                .OfType<SetModelGremlinStep>()
                .Select(x => Option<IGraphModel>.Some(x.Model))
                .LastOrDefault();
        }
    }
}