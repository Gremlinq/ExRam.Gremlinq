using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using LanguageExt;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Unit = System.Reactive.Unit;

namespace ExRam.Gremlinq
{
    public static class GremlinQuery<TElement>
    {
        internal sealed class GremlinQueryImpl : IGremlinQuery<TElement>
        {
            public GremlinQueryImpl(string traversalSourceName, IImmutableList<GremlinStep> steps, IImmutableDictionary<StepLabel, string> stepLabelBindings)
            {
                this.Steps = steps;
                this.TraversalSourceName = traversalSourceName;
                this.StepLabelMappings = stepLabelBindings;
            }

            public void Serialize(StringBuilder builder, IParameterCache parameterCache)
            {
                builder.Append(this.TraversalSourceName);

                foreach (var step in this.Steps)
                {
                    if (step is IGremlinSerializable serializableStep)
                    {
                        builder.Append('.');
                        serializableStep.Serialize(builder, parameterCache);
                    }
                    else
                        throw new ArgumentException("Query contains non-serializable step. Please call RewriteSteps on the query first.");
                }
            }

            public string TraversalSourceName { get; }
            public IImmutableList<GremlinStep> Steps { get; }
            public IImmutableDictionary<StepLabel, string> StepLabelMappings { get; }
        }

        public static readonly IGremlinQuery<TElement> Anonymous = GremlinQuery<TElement>.Create("__");

        public static IGremlinQuery<TElement> Create(string graphName = null)
        {
            return new GremlinQueryImpl(graphName, ImmutableList<GremlinStep>.Empty, ImmutableDictionary<StepLabel, string>.Empty);
        }
    }

    public static class GremlinQuery
    {
        public static readonly IGremlinQuery<Unit> Anonymous = GremlinQuery<Unit>.Anonymous;

        public static IGremlinQuery<Unit> Create(string graphName = null)
        {
            return GremlinQuery<Unit>.Create(graphName);
        }

        public static IGremlinQuery<TElement> ToAnonymous<TElement>(this IGremlinQuery<TElement> query)
        {
            return GremlinQuery<TElement>.Anonymous;
        }

        public static (string queryString, IDictionary<string, object> parameters) Serialize(this IGremlinQuery query)
        {
            var cache = new DefaultParameterCache(query.StepLabelMappings);
            var stringBuilder = new StringBuilder();

            query.Serialize(stringBuilder, cache);

            return (stringBuilder.ToString(), cache.GetDictionary());
        }

        public static Task<TElement> FirstAsync<TElement>(this IGremlinQuery<TElement> query, ITypedGremlinQueryProvider provider, CancellationToken ct = default(CancellationToken))
        {
            return query
                .Limit(1)
                .Execute(provider)
                .First(ct);
        }

        public static async Task<Option<TElement>> FirstOrNoneAsync<TElement>(this IGremlinQuery<TElement> query, ITypedGremlinQueryProvider provider, CancellationToken ct = default(CancellationToken))
        {
            var array = await query
                .Limit(1)
                .Execute(provider)
                .ToArray(ct)
                .ConfigureAwait(false);

            return array.Length > 0
                ? array[0]
                : Option<TElement>.None;
        }

        public static Task<TElement[]> ToArrayAsync<TElement>(this IGremlinQuery<TElement> query, ITypedGremlinQueryProvider provider, CancellationToken ct = default(CancellationToken))
        {
            return query
                .Execute(provider)
                .ToArray(ct);
        }

        public static IGremlinQuery<TElement> AddStep<TElement>(this IGremlinQuery query, string name, params object[] parameters)
        {
            return query.AddStep<TElement>(new TerminalGremlinStep(name, parameters));
        }

        public static IGremlinQuery<TElement> AddStep<TElement>(this IGremlinQuery query, GremlinStep step)
        {
            return new GremlinQuery<TElement>.GremlinQueryImpl(query.TraversalSourceName, query.Steps.Add(step), query.StepLabelMappings);
        }

        public static IGremlinQuery<TElement> AddStep<TElement>(this IGremlinQuery query, string name, ImmutableList<object> parameters)
        {
            return query.InsertStep<TElement>(query.Steps.Count, new TerminalGremlinStep(name, parameters));
        }

        public static IGremlinQuery<TElement> InsertStep<TElement>(this IGremlinQuery query, int index, GremlinStep step)
        {
            return new GremlinQuery<TElement>.GremlinQueryImpl(query.TraversalSourceName, query.Steps.Insert(index, step), query.StepLabelMappings);
        }

        public static IGremlinQuery ReplaceSteps(this IGremlinQuery query, IImmutableList<GremlinStep> steps)
        {
            return new GremlinQuery<Unit>.GremlinQueryImpl(query.TraversalSourceName, steps, query.StepLabelMappings);
        }

        public static IGremlinQuery<TElement> Cast<TElement>(this IGremlinQuery query)
        {
            return new GremlinQuery<TElement>.GremlinQueryImpl(query.TraversalSourceName, query.Steps, query.StepLabelMappings);
        }

        public static IGremlinQuery Resolve(this IGremlinQuery query, IGraphModel model)
        {
            return query.RewriteSteps(x => Option<IEnumerable<GremlinStep>>.Some(x.Resolve(model)));
        }

        public static IGremlinQuery RewriteSteps(this IGremlinQuery query, Func<NonTerminalGremlinStep, Option<IEnumerable<GremlinStep>>> resolveFunction)
        {
            var steps = query.Steps;

            for (var i = 0; i < steps.Count; i++)
            {
                var step = steps[i];

                switch (step)
                {
                    case TerminalGremlinStep terminal:
                        {
                            var parameters = terminal.Parameters;

                            for (var j = 0; j < parameters.Count; j++)
                            {
                                var parameter = parameters[j];

                                if (parameter is IGremlinQuery subQuery)
                                    parameters = parameters.SetItem(j, subQuery.RewriteSteps(resolveFunction));
                            }

                            // ReSharper disable once PossibleUnintendedReferenceComparison
                            if (parameters != terminal.Parameters)
                                steps = steps.SetItem(i, new TerminalGremlinStep(terminal.Name, parameters));
                            break;
                        }
                    case NonTerminalGremlinStep nonTerminal:
                        {
                            resolveFunction(nonTerminal)
                            .IfSome(resolvedSteps =>
                            {
                                steps = steps
                                    .RemoveAt(i)
                                    .InsertRange(i, resolvedSteps);

                                i--;
                            });
                            break;
                        }
                }
            }

            // ReSharper disable once PossibleUnintendedReferenceComparison
            return steps != query.Steps
                ? query.ReplaceSteps(steps)
                : query;
        }

        internal static IGremlinQuery<TElement> AddStepLabelBinding<TElement>(this IGremlinQuery<TElement> query, Expression<Func<TElement, object>> memberExpression, StepLabel stepLabel)
        {
            var body = memberExpression.Body;
            if (body is UnaryExpression && body.NodeType == ExpressionType.Convert)
                body = ((UnaryExpression)body).Operand;

            if (!(body is MemberExpression memberExpressionBody))
                throw new ArgumentException();

            return new GremlinQuery<TElement>.GremlinQueryImpl(query.TraversalSourceName, query.Steps, query.StepLabelMappings.SetItem(stepLabel, memberExpressionBody.Member.Name));
        }

        internal static IGremlinQuery<TElement> ReplaceProvider<TElement>(this IGremlinQuery<TElement> query, ITypedGremlinQueryProvider provider)
        {
            return new GremlinQuery<TElement>.GremlinQueryImpl(query.TraversalSourceName, query.Steps, query.StepLabelMappings);
        }
    }
}