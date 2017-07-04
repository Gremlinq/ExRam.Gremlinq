using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using LanguageExt;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Unit = System.Reactive.Unit;

namespace ExRam.Gremlinq
{
    public static class GremlinQuery
    {
        private class GremlinQueryImpl : IGremlinQuery
        {
            public GremlinQueryImpl(string traversalSourceName, IImmutableList<GremlinStep> steps, IImmutableDictionary<string, StepLabel> stepLabelBindings, IIdentifierFactory identifierFactory)
            {
                this.Steps = steps;
                this.TraversalSourceName = traversalSourceName;
                this.StepLabelMappings = stepLabelBindings;
                this.IdentifierFactory = identifierFactory;
            }

            public (string queryString, IDictionary<string, object> parameters) Serialize(IParameterCache parameterCache, bool inlineParameters)
            {
                var parameters = new Dictionary<string, object>();
                var builder = new StringBuilder(this.TraversalSourceName);

                foreach (var step in this.Steps)
                {
                    if (step is IGremlinSerializable serializableStep)
                    {
                        var (innerQueryString, innerParameters) = serializableStep.Serialize(parameterCache, inlineParameters);

                        builder.Append('.');
                        builder.Append(innerQueryString);

                        foreach (var kvp in innerParameters)
                        {
                            parameters[kvp.Key] = kvp.Value;
                        }
                    }
                    else
                        throw new ArgumentException("Query contains non-serializable step. Please call Resolve on the query first.");
                }

                return (builder.ToString(), parameters);
            }

            public string TraversalSourceName { get; }
            public IImmutableList<GremlinStep> Steps { get; }
            public IIdentifierFactory IdentifierFactory { get; }
            public IImmutableDictionary<string, StepLabel> StepLabelMappings { get; }
        }

        private sealed class GremlinQueryImpl<T> : GremlinQueryImpl, IGremlinQuery<T>
        {
            public GremlinQueryImpl(string traversalSourceName, IImmutableList<GremlinStep> steps, IImmutableDictionary<string, StepLabel> stepLabelBindings, IIdentifierFactory identifierFactory) : base(traversalSourceName, steps, stepLabelBindings, identifierFactory)
            {
            }
        }

        public static IGremlinQuery<Unit> Create(string graphName = null)
        {
            return new GremlinQueryImpl<Unit>(graphName, ImmutableList<GremlinStep>.Empty, ImmutableDictionary<string, StepLabel>.Empty, IdentifierFactory.CreateDefault());
        }

        public static IGremlinQuery<T> ToAnonymous<T>(this IGremlinQuery<T> query)
        {
            return new GremlinQueryImpl<T>("__", ImmutableList<GremlinStep>.Empty, query.StepLabelMappings, query.IdentifierFactory);
        }

        public static (string queryString, IDictionary<string, object> parameters) Serialize(this IGremlinSerializable query, bool inlineParameters)
        {
            return query.Serialize(new DefaultParameterCache(), inlineParameters);
        }

        public static Task<T> FirstAsync<T>(this IGremlinQuery<T> query, ITypedGremlinQueryProvider provider)
        {
            return query
                .Limit(1)
                .Execute(provider)
                .First();
        }

        public static async Task<Option<T>> FirstOrNoneAsync<T>(this IGremlinQuery<T> query, ITypedGremlinQueryProvider provider)
        {
            var array = await query
                .Limit(1)
                .Execute(provider)
                .ToArray();

            return array.Length > 0
                ? array[0]
                : Option<T>.None;
        }

        public static Task<T[]> ToArrayAsync<T>(this IGremlinQuery<T> query, ITypedGremlinQueryProvider provider)
        {
            return query
                .Execute(provider)
                .ToArray();
        }

        public static IGremlinQuery<T> AddStep<T>(this IGremlinQuery query, string name, params object[] parameters)
        {
            return query.AddStep<T>(new TerminalGremlinStep(name, parameters));
        }

        public static IGremlinQuery<T> AddStep<T>(this IGremlinQuery query, GremlinStep step)
        {
            return new GremlinQueryImpl<T>(query.TraversalSourceName, query.Steps.Add(step), query.StepLabelMappings, query.IdentifierFactory);
        }

        public static IGremlinQuery<T> AddStep<T>(this IGremlinQuery query, string name, ImmutableList<object> parameters)
        {
            return query.InsertStep<T>(query.Steps.Count, new TerminalGremlinStep(name, parameters));
        }

        public static IGremlinQuery<T> InsertStep<T>(this IGremlinQuery query, int index, GremlinStep step)
        {
            return new GremlinQueryImpl<T>(query.TraversalSourceName, query.Steps.Insert(index, step), query.StepLabelMappings, query.IdentifierFactory);
        }

        public static IGremlinQuery ReplaceSteps(this IGremlinQuery query, IImmutableList<GremlinStep> steps)
        {
            return new GremlinQueryImpl(query.TraversalSourceName, steps, query.StepLabelMappings, query.IdentifierFactory);
        }

        public static IGremlinQuery<T> Cast<T>(this IGremlinQuery query)
        {
            return new GremlinQueryImpl<T>(query.TraversalSourceName, query.Steps, query.StepLabelMappings, query.IdentifierFactory);
        }

        public static IGremlinQuery Resolve(this IGremlinQuery query, IGraphModel model)
        {
            var steps = query.Steps;

            for (var i = 0; i < steps.Count; i++)
            {
                var step = steps[i];

                if (step is TerminalGremlinStep terminal)
                {
                    var parameters = terminal.Parameters;

                    for (var j = 0; j < parameters.Count; j++)
                    {
                        var parameter = parameters[j];

                        if (parameter is IGremlinQuery subQuery)
                            parameters = parameters.SetItem(j, subQuery.Resolve(model));
                    }

                    // ReSharper disable once PossibleUnintendedReferenceComparison
                    if (parameters != terminal.Parameters)
                        steps = steps.SetItem(i, new TerminalGremlinStep(terminal.Name, parameters));
                }
                else if (step is NonTerminalGremlinStep nonTerminal)
                {
                    steps = steps
                        .RemoveAt(i)
                        .InsertRange(i, nonTerminal
                            .Resolve(model));

                    i--;
                }
            }

            // ReSharper disable once PossibleUnintendedReferenceComparison
            return steps != query.Steps
                ? query.ReplaceSteps(steps)
                : query;
        }

        internal static IGremlinQuery<T> AddStepLabelBinding<T>(this IGremlinQuery<T> query, Expression<Func<T, object>> memberExpression, StepLabel stepLabel)
        {
            var body = memberExpression.Body;
            if (body is UnaryExpression && body.NodeType == ExpressionType.Convert)
                body = ((UnaryExpression)body).Operand;

            var memberExpressionBody = body as MemberExpression;
            if (memberExpressionBody == null)
                throw new ArgumentException();

            return new GremlinQueryImpl<T>(query.TraversalSourceName, query.Steps, query.StepLabelMappings.SetItem(memberExpressionBody.Member.Name, stepLabel), query.IdentifierFactory);
        }

        internal static IGremlinQuery<T> ReplaceProvider<T>(this IGremlinQuery<T> query, ITypedGremlinQueryProvider provider)
        {
            return new GremlinQueryImpl<T>(query.TraversalSourceName, query.Steps, query.StepLabelMappings, query.IdentifierFactory);
        }
    }
}