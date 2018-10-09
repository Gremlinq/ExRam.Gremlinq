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
    internal sealed class GremlinQueryImpl<TElement> :
        IVGremlinQuery<TElement>,
        IEGremlinQuery<TElement>,
        IEGremlinQuery<Unit, TElement>,
        IEGremlinQuery<Unit, Unit, TElement>
    {
        public GremlinQueryImpl(IImmutableList<GremlinStep> steps, IImmutableDictionary<StepLabel, string> stepLabelBindings)
        {
            this.Steps = steps;
            this.StepLabelMappings = stepLabelBindings;
        }

        IGremlinQuery<TVertex> IGremlinQuery.AddV<TVertex>(TVertex vertex)
        {
            return this
                .AddStep<TVertex>(new AddVGremlinStep(vertex))
                .AddStep(new AddElementPropertiesStep(vertex));
        }

        IGremlinQuery<TVertex> IGremlinQuery.AddV<TVertex>()
        {
            return ((IGremlinQuery<TElement>)this).AddV(new TVertex());
        }

        public IGremlinQuery<TEdge> AddE<TEdge>() where TEdge : new()
        {
            return ((IGremlinQuery<TElement>)this).AddE(new TEdge());
        }

        IGremlinQuery<TEdge> IGremlinQuery.AddE<TEdge>(TEdge edge)
        {
            return this
                .AddStep<TEdge>(new AddEGremlinStep(edge))
                .AddStep(new AddElementPropertiesStep(edge));
        }

        IGremlinQuery<TElement> IGremlinQuery<TElement>.And(params Func<IGremlinQuery<TElement>, IGremlinQuery>[] andTraversals)
        {
            return this.Call(
                "and",
                andTraversals
                    .Select(andTraversal => andTraversal(this.ToAnonymous()))
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
            return this
                .Call("as", stepLabel);
        }

        IGremlinQuery<TElement> IGremlinQuery<TElement>.Barrier()
        {
            return this
                .Call("barrier");
        }

        IGremlinQuery<TTarget> IGremlinQuery.Cast<TTarget>()
        {
            return this.Cast<TTarget>();
        }

        private GremlinQueryImpl<TTarget> Cast<TTarget>()
        {
            return new GremlinQueryImpl<TTarget>(this.Steps, this.StepLabelMappings);
        }


        //public IEGremlinQuery<TTarget, Unit, TElement> CastOutVertex<TTarget>()
        //{
        //    return new GremlinQueryImpl<TTarget, TInVertex, TElement>(this.Steps, this.StepLabelMappings);
        //}

        //public IEGremlinQuery<TOutVertex, TTarget, TElement> CastInVertex<TTarget>()
        //{
        //    return new GremlinQueryImpl<TOutVertex, TTarget, TElement>(this.Steps, this.StepLabelMappings);
        //}

        IGremlinQuery<TTarget> IGremlinQuery<TElement>.Coalesce<TTarget>(params Func<IGremlinQuery<TElement>, IGremlinQuery<TTarget>>[] traversals)
        {
            return this
                .Call<TTarget>(
                    "coalesce",
                    traversals
                        .Select(traversal => traversal(this.ToAnonymous()))
                        .ToImmutableList<object>());
        }

        IGremlinQuery<TResult> IGremlinQuery<TElement>.Choose<TResult>(Func<IGremlinQuery<TElement>, IGremlinQuery> traversalPredicate, Func<IGremlinQuery<TElement>, IGremlinQuery<TResult>> trueChoice, Func<IGremlinQuery<TElement>, IGremlinQuery<TResult>> falseChoice)
        {
            var anonymous = this.ToAnonymous();

            return this
                .Call<TResult>(
                    "choose",
                    traversalPredicate(anonymous),
                    trueChoice(anonymous),
                    falseChoice(anonymous));
        }

        IGremlinQuery<TResult> IGremlinQuery<TElement>.Choose<TResult>(Func<IGremlinQuery<TElement>, IGremlinQuery> traversalPredicate, Func<IGremlinQuery<TElement>, IGremlinQuery<TResult>> trueChoice)
        {
            var anonymous = this.ToAnonymous();

            return this
                .Call<TResult>(
                    "choose",
                    traversalPredicate(anonymous),
                    trueChoice(anonymous));
        }

        IVGremlinQuery<Vertex> IVGremlinQuery<TElement>.Both()
        {
            return this
                .AddStep<Vertex>(new DerivedLabelNamesGremlinStep<TElement>("both"));
        }

        IGremlinQuery<TEdge> IGremlinQuery.BothE<TEdge>()
        {
            return this
                .AddStep<TEdge>(new DerivedLabelNamesGremlinStep<TEdge>("bothE"));
        }

        IVGremlinQuery<Vertex> IEGremlinQuery<TElement>.BothV()
        {
            return this
                .Call<Vertex>("bothV");
        }

        IGremlinQuery<TEnd> IGremlinQuery<TElement>.BranchOnIdentity<TEnd>(params Func<IGremlinQuery<TElement>, IGremlinQuery<TEnd>>[] options)
        {
            return ((IGremlinQuery<TElement>)this)
                .Branch(_ => _.Identity(), options);
        }

        IGremlinQuery<TEnd> IGremlinQuery<TElement>.Branch<TBranch, TEnd>(Func<IGremlinQuery<TElement>, IGremlinQuery<TBranch>> branchSelector, params Func<IGremlinQuery<TBranch>, IGremlinQuery<TEnd>>[] options)
        {
            return options
                .Aggregate(
                    this
                        .Call<TBranch>("branch", branchSelector(this.ToAnonymous())),
                    (branchQuery, option) => branchQuery.Call<TBranch>("option", option(branchQuery.ToAnonymous())))
                .Cast<TEnd>();
        }

        IGremlinQuery<TElement> IGremlinQuery<TElement>.ByTraversal(Func<IGremlinQuery<TElement>, IGremlinQuery> traversal, Order sortOrder)
        {
            return this
                .Call("by", traversal(this.ToAnonymous()), sortOrder);
        }

        IGremlinQuery<TElement> IGremlinQuery<TElement>.ByMember(Expression<Func<TElement, object>> projection, Order sortOrder)
        {
            var body = projection.Body;
            if (body is UnaryExpression && body.NodeType == ExpressionType.Convert)
                body = ((UnaryExpression)body).Operand;

            if (body is MemberExpression memberExpression)
            {
                return this
                    .Call("by", memberExpression.Member.Name, sortOrder);
            }

            throw new NotSupportedException();
        }

        IGremlinQuery<TElement> IGremlinQuery<TElement>.ByLambda(string lambdaString)
        {
            return this
                .Call("by", new Lambda(lambdaString));
        }

        IGremlinQuery<TElement> IGremlinQuery<TElement>.Dedup()
        {
            return this
                .Call("dedup");
        }

        IGremlinQuery<Unit> IGremlinQuery.Drop()
        {
            return this
                .Call<Unit>("drop");
        }

        IGremlinQuery<Edge> IGremlinQuery.E(params object[] ids)
        {
            return this
                .Call<Edge>("E", ids);
        }

        IGremlinQuery<TElement> IGremlinQuery<TElement>.Emit()
        {
            return this
                .Call("emit");
        }

        IGremlinQuery<TElement> IGremlinQuery<TElement>.Explain()
        {
            return this
                .Call("explain");
        }

        IGremlinQuery<TElement> IGremlinQuery<TElement>.FilterWithLambda(string lambda)
        {
            return this
                .Call("filter", new Lambda(lambda));
        }

        IGremlinQuery<TElement[]> IGremlinQuery<TElement>.Fold()
        {
            return this
                .Call<TElement[]>("fold");
        }

        IGremlinQuery<TElement> IGremlinQuery<TElement>.From<TStepLabel>(StepLabel<TStepLabel> stepLabel)
        {
            return this
                .Call("from", stepLabel);
        }

        IGremlinQuery<TElement> IGremlinQuery<TElement>.From(Func<IGremlinQuery<TElement>, IGremlinQuery> fromVertex)
        {
            return this
                .Call("from", fromVertex(this.ToAnonymous()));
        }

        IGremlinQuery<object> IGremlinQuery.Id()
        {
            return this
                .Call<object>("id");
        }

        IGremlinQuery<TElement> IGremlinQuery<TElement>.Identity()
        {
            return this
                .Call("identity");
        }

        IGremlinQuery<Vertex> IGremlinQuery.In<TEdge>()
        {
            return this
                .AddStep<Vertex>(new DerivedLabelNamesGremlinStep<TEdge>("in"));
        }

        IGremlinQuery<TTarget> IGremlinQuery.InsertStep<TTarget>(int index, GremlinStep step)
        {
            return new GremlinQueryImpl<TTarget>(this.Steps.Insert(index, step), this.StepLabelMappings);
        }

        IGremlinQuery<TEdge> IGremlinQuery.InE<TEdge>()
        {
            return this
                .AddStep<TEdge>(new DerivedLabelNamesGremlinStep<TEdge>("inE"));
        }

        IGremlinQuery<TVertex> IGremlinQuery.InV<TVertex>()
        {
            return this
                .Call<Vertex>("inV")
                .OfType<TVertex>();
        }

        IGremlinQuery<TElement> IGremlinQuery<TElement>.Inject(params TElement[] elements)
        {
            return this
                .Call("inject", elements);
        }

        IGremlinQuery<TElement> IGremlinQuery<TElement>.Limit(long limit)
        {
            return this
                .Call("limit", limit);
        }

        IGremlinQuery<TTarget> IGremlinQuery<TElement>.Local<TTarget>(Func<IGremlinQuery<TElement>, IGremlinQuery<TTarget>> localTraversal)
        {
            return this
                .Call<TTarget>("local", localTraversal(this.ToAnonymous()));
        }

        IGremlinQuery<TTarget> IGremlinQuery<TElement>.Map<TTarget>(Func<IGremlinQuery<TElement>, IGremlinQuery<TTarget>> mapping)
        {
            return this
                .Call<TTarget>("map", mapping(this.ToAnonymous()));
        }

        IGremlinQuery<TElement> IGremlinQuery<TElement>.Match(params Func<IGremlinQuery<TElement>, IGremlinQuery<TElement>>[] matchTraversals)
        {
            return this
                // ReSharper disable once CoVariantArrayConversion
                .Call("match", matchTraversals.Select(traversal => traversal(this.ToAnonymous())).ToArray());
        }

        IGremlinQuery<TElement> IGremlinQuery<TElement>.Not(Func<IGremlinQuery<TElement>, IGremlinQuery> notTraversal)
        {
            return this
                .Call("not", notTraversal(this.ToAnonymous()));
        }

        IGremlinQuery<TTarget> IGremlinQuery.OfType<TTarget>()
        {
            return this.OfType<TTarget>();
        }

        private GremlinQueryImpl<TTarget> OfType<TTarget>()
        {
            return this
                .AddStep<TTarget>(new DerivedLabelNamesGremlinStep<TTarget>("hasLabel"));
        }

        IGremlinQuery<TTarget> IGremlinQuery<TElement>.Optional<TTarget>(Func<IGremlinQuery<TElement>, IGremlinQuery<TTarget>> optionalTraversal)
        {
            return this
                .Call<TTarget>("optional", optionalTraversal(this.ToAnonymous()));
        }

        IGremlinQuery<TElement> IGremlinQuery<TElement>.Or(params Func<IGremlinQuery<TElement>, IGremlinQuery>[] orTraversals)
        {
            return this.Call(
                "or",
                orTraversals
                    .Select(andTraversal => andTraversal(this.ToAnonymous()))
                    .Aggregate(
                        ImmutableList<object>.Empty,
                        (list, query2) => query2.Steps.Count == 2 && (query2.Steps[1] as MethodGremlinStep)?.Name == "or"
                            ? list.AddRange(((MethodGremlinStep)query2.Steps[1]).Parameters)
                            : list.Add(query2)));
        }

        IGremlinQuery<TElement> IGremlinQuery<TElement>.Order()
        {
            return this
                .Call("order");
        }

        IGremlinQuery<Vertex> IGremlinQuery.OtherV()
        {
            return this
                .Call<Vertex>("otherV");
        }

        IGremlinQuery<TEdge> IGremlinQuery.OutE<TEdge>()
        {
            return this
                .AddStep<TEdge>(new DerivedLabelNamesGremlinStep<TEdge>("outE"));
        }

        IGremlinQuery<TVertex> IGremlinQuery.OutV<TVertex>()
        {
            return this
                .Call<Vertex>("outV")
                .OfType<TVertex>();
        }

        IGremlinQuery<Vertex> IGremlinQuery.Out<TEdge>()
        {
            return this
                .AddStep<Vertex>(new DerivedLabelNamesGremlinStep<TEdge>("out"));
        }

        IGremlinQuery<string> IGremlinQuery<TElement>.Profile()
        {
            return this
                .Call<string>("profile");
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
            return this
                .Call("property", key, value);
        }

        IGremlinQuery<TElement> IGremlinQuery<TElement>.Range(long low, long high)
        {
            return this
                .Call("range", low, high);
        }

        IGremlinQuery<TElement> IGremlinQuery<TElement>.Repeat(Func<IGremlinQuery<TElement>, IGremlinQuery<TElement>> repeatTraversal)
        {
            return this
                .Call("repeat", repeatTraversal(this.ToAnonymous()));
        }

        IGremlinQuery<TStep> IGremlinQuery.Select<TStep>(StepLabel<TStep> label)
        {
            return this
                .Call<TStep>("select", label);
        }

        IGremlinQuery<(T1, T2)> IGremlinQuery.Select<T1, T2>(StepLabel<T1> label1, StepLabel<T2> label2)
        {
            return this
                .Call<(T1, T2)>("select", label1, label2)
                .AddStepLabelBinding(x => x.Item1, label1)
                .AddStepLabelBinding(x => x.Item2, label2);
        }

        IGremlinQuery<(T1, T2, T3)> IGremlinQuery.Select<T1, T2, T3>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3)
        {
            return this
                .Call<(T1, T2, T3)>("select", label1, label2, label3)
                .AddStepLabelBinding(x => x.Item1, label1)
                .AddStepLabelBinding(x => x.Item2, label2)
                .AddStepLabelBinding(x => x.Item3, label3);
        }

        IGremlinQuery<(T1, T2, T3, T4)> IGremlinQuery.Select<T1, T2, T3, T4>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3, StepLabel<T4> label4)
        {
            return this
                .Call<(T1, T2, T3, T4)>("select", label1, label2, label3, label4)
                .AddStepLabelBinding(x => x.Item1, label1)
                .AddStepLabelBinding(x => x.Item2, label2)
                .AddStepLabelBinding(x => x.Item3, label3)
                .AddStepLabelBinding(x => x.Item4, label4);
        }

        IGremlinQuery<TElement> IGremlinQuery<TElement>.SideEffect(Func<IGremlinQuery<TElement>, IGremlinQuery> sideEffectTraversal)
        {
            return this
                .Call("sideEffect", sideEffectTraversal(this.ToAnonymous()));
        }

        IGremlinQuery<TElement> IGremlinQuery<TElement>.Skip(long skip)
        {
            return this
                .Call("skip", skip);
        }

        IGremlinQuery<TElement> IGremlinQuery<TElement>.Sum(Scope scope)
        {
            return this.Call("sum", scope);
        }

        IGremlinQuery<TElement> IGremlinQuery<TElement>.Times(int count)
        {
            return this
                .Call("times", count);
        }

        IGremlinQuery<TElement> IGremlinQuery<TElement>.Tail(long limit)
        {
            return this
                .Call("tail", limit);
        }

        IGremlinQuery<TElement> IGremlinQuery<TElement>.To<TStepLabel>(StepLabel<TStepLabel> stepLabel)
        {
            return this
                .Call("to", stepLabel);
        }

        IGremlinQuery<TElement> IGremlinQuery<TElement>.To(Func<IGremlinQuery<TElement>, IGremlinQuery> toVertex)
        {
            return this
                .Call("to", toVertex(this.ToAnonymous()));
        }

        IGremlinQuery<TElement> IGremlinQuery<TElement>.Unfold(IGremlinQuery<IEnumerable<TElement>> query)
        {
            throw new NotImplementedException();    //Bug!
                                                    //return query
                                                    //    .Call<TElement>("unfold");
        }

        IGremlinQuery<TTarget> IGremlinQuery<TElement>.Union<TTarget>(params Func<IGremlinQuery<TElement>, IGremlinQuery<TTarget>>[] unionTraversals)
        {
            return this
                .Call<TTarget>("union", unionTraversals
                    .Select(unionTraversal => unionTraversal(this.ToAnonymous()))
                    .ToImmutableList<object>());
        }

        IGremlinQuery<TElement> IGremlinQuery<TElement>.Until(Func<IGremlinQuery<TElement>, IGremlinQuery> untilTraversal)
        {
            return this
                .Call("until", untilTraversal(this.ToAnonymous()));
        }

        IGremlinQuery<Vertex> IGremlinQuery.V(params object[] ids)
        {
            return this
                .Call<Vertex>("V", ids);
        }

        IGremlinQuery<TTarget> IGremlinQuery<TElement>.Values<TTarget>(params Expression<Func<TElement, TTarget>>[] projections)
        {
            return this.AddStep<TTarget>(new ValuesGremlinStep<TElement, TTarget>(projections));
        }

        IGremlinQuery<TElement> IGremlinQuery<TElement>.Where(Func<IGremlinQuery<TElement>, IGremlinQuery> filterTraversal)
        {
            return this
                .Call("where", filterTraversal(this.ToAnonymous()));
        }

        GroovyExpressionState IGroovySerializable.Serialize(StringBuilder stringBuilder, GroovyExpressionState state)
        {
            foreach (var step in this.Steps)
            {
                if (step is IGroovySerializable serializableStep)
                    state = serializableStep.Serialize(stringBuilder, state);
                else
                    throw new ArgumentException("Query contains non-serializable step. Please call RewriteSteps on the query first.");
            }

            return state;
        }

        private GremlinQueryImpl<TTarget> Call<TTarget>(string name, params object[] parameters)
        {
            return this.Call<TTarget>(name, parameters.ToImmutableList());
        }

        private GremlinQueryImpl<TTarget> Call<TTarget>(string name, ImmutableList<object> parameters)
        {
            return this.AddStep<TTarget>(new MethodGremlinStep(name, parameters));
        }

        private GremlinQueryImpl<TElement> AddStep(GremlinStep step)
        {
            return this.AddStep<TElement>(step);
        }

        private GremlinQueryImpl<TNewTarget> AddStep<TNewTarget>(GremlinStep step)
        {
            return new GremlinQueryImpl<TNewTarget>(this.Steps.Insert(this.Steps.Count, step), this.StepLabelMappings);
        }

        public IImmutableList<GremlinStep> Steps { get; }
        public IImmutableDictionary<StepLabel, string> StepLabelMappings { get; }
    }

    internal static class GremlinQuery<TElement>
    {
        public static readonly IGremlinQuery<TElement> Anonymous = GremlinQuery<TElement>.Create("__");

        public static IGremlinQuery<TElement> Create(string graphName = "g")
        {
            return new GremlinQueryImpl<TElement>(ImmutableList<GremlinStep>.Empty.Add(new IdentifierGremlinStep(graphName)), ImmutableDictionary<StepLabel, string>.Empty);
        }
    }

    public static class GremlinQuery
    {
        public static readonly IGremlinQuery<Unit> Anonymous = GremlinQuery<Unit>.Anonymous;

        public static IGremlinQuery<Unit> Create(string graphName)
        {
            return GremlinQuery<Unit>.Create(graphName);
        }

        public static IGremlinQuery<TElement> Create<TElement>(string graphName)
        {
            return GremlinQuery<TElement>.Create(graphName);
        }

        public static IGremlinQuery<TElement> ToAnonymous<TElement>(this IGremlinQuery<TElement> query)
        {
            return GremlinQuery<TElement>.Anonymous;
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
        
        public static IGremlinQuery<TEdge> E<TEdge>(this IGremlinQuery query, params object[] ids)
        {
            return query
                .E(ids)
                .OfType<TEdge>();
        }

        public static IGremlinQuery<TElement> ReplaceSteps<TElement>(this IGremlinQuery<TElement> query, IImmutableList<GremlinStep> steps)
        {
            return ReferenceEquals(steps, query.Steps)
                ? query 
                : new GremlinQueryImpl<TElement>(steps, query.StepLabelMappings);
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

        public static IGremlinQuery<TVertex> V<TVertex>(this IGremlinQuery query, params object[] ids)
        {
            return query
                .V(ids)
                .OfType<TVertex>();
        }

        internal static IGremlinQuery<TElement> AddStepLabelBinding<TElement>(this IGremlinQuery<TElement> query, Expression<Func<TElement, object>> memberExpression, StepLabel stepLabel)
        {
            var body = memberExpression.Body.StripConvert();
            
            if (!(body is MemberExpression memberExpressionBody))
                throw new ArgumentException();

            return new GremlinQueryImpl<TElement>(query.Steps, query.StepLabelMappings.SetItem(stepLabel, memberExpressionBody.Member.Name));
        }

        internal static IGremlinQuery<TElement> ReplaceProvider<TElement>(this IGremlinQuery<TElement> query, ITypedGremlinQueryProvider provider)
        {
            return new GremlinQueryImpl<TElement>(query.Steps, query.StepLabelMappings);
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
                                ? query.ToAnonymous().Call("where", predicateArgument)
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