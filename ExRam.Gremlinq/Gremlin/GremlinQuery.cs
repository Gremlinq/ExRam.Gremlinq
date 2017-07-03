using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using LanguageExt;
using System.Linq.Expressions;
using System.Threading.Tasks;

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

            public (string queryString, IDictionary<string, object> parameters) Serialize(IGraphModel graphModel, IParameterCache parameterCache, bool inlineParameters)
            {
                var parameters = new Dictionary<string, object>();
                var builder = new StringBuilder(this.TraversalSourceName);

                foreach (var step in this.Steps)
                {
                    foreach (var terminalStep in Resolve(step, graphModel))
                    {
                        var (innerQueryString, innerParameters) = terminalStep.Serialize(graphModel, parameterCache, inlineParameters);

                        builder.Append('.');
                        builder.Append(innerQueryString);
                        
                        foreach (var kvp in innerParameters)
                        {
                            parameters[kvp.Key] = kvp.Value;
                        }
                    }
                }

                return (builder.ToString(), parameters);
            }
            
            private IEnumerable<TerminalGremlinStep> Resolve(GremlinStep step, IGraphModel model)
            {
                if (step is TerminalGremlinStep terminal)
                    yield return terminal;
                else if (step is NonTerminalGremlinStep nonTerminal)
                {
                    foreach (var innerTerminal in nonTerminal.Resolve(model))
                    {
                        yield return innerTerminal;
                    }
                }
                else
                    throw new ArgumentException();
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

        public static IGremlinQuery Create(string graphName = null)
        {
            return new GremlinQueryImpl(graphName, ImmutableList<GremlinStep>.Empty, ImmutableDictionary<string, StepLabel>.Empty, IdentifierFactory.CreateDefault());
        }
   
        public static IGremlinQuery<T> ToAnonymous<T>(this IGremlinQuery<T> query)
        {
            return new GremlinQueryImpl<T>("__", ImmutableList<GremlinStep>.Empty, query.StepLabelMappings, query.IdentifierFactory);
        }

        public static (string queryString, IDictionary<string, object> parameters) Serialize<T>(this IGremlinQuery<T> query, IGraphModel graphModel, bool inlineParameters)
        {
            return query.Serialize(graphModel, new DefaultParameterCache(), inlineParameters);
        }

        public static Task<T> FirstAsync<T>(this IGremlinQuery<T> query, IGremlinQueryProvider provider)
        {
            return query
                .Limit(1)
                .Execute(provider)
                .First();
        }

        public static async Task<Option<T>> FirstOrNoneAsync<T>(this IGremlinQuery<T> query, IGremlinQueryProvider provider)
        {
            var array = await query
                .Limit(1)
                .Execute(provider)
                .ToArray();

            return array.Length > 0
                ? array[0]
                : Option<T>.None;
        }

        public static Task<T[]> ToArrayAsync<T>(this IGremlinQuery<T> query, IGremlinQueryProvider provider)
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

        internal static IGremlinQuery ReplaceProvider(this IGremlinQuery query, IGremlinQueryProvider provider)
        {
            return new GremlinQueryImpl(query.TraversalSourceName, query.Steps, query.StepLabelMappings, query.IdentifierFactory);
        }

        internal static IGremlinQuery<T> ReplaceProvider<T>(this IGremlinQuery<T> query, IGremlinQueryProvider provider)
        {
            return new GremlinQueryImpl<T>(query.TraversalSourceName, query.Steps, query.StepLabelMappings, query.IdentifierFactory);
        }
    }
}