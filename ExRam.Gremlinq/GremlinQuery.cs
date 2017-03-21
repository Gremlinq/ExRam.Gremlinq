using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Text;
using LanguageExt;
using System.Linq.Expressions;

namespace ExRam.Gremlinq
{
    public static class GremlinQuery
    {
        private class GremlinQueryImpl : IGremlinQuery
        {
            public GremlinQueryImpl(string graphName, IImmutableList<GremlinStep> steps, IGremlinQueryProvider provider, IImmutableDictionary<MemberInfo, string> memberInfoMappings)
            {
                this.Steps = steps;
                this.Provider = provider;
                this.GraphName = graphName;
                this.MemberInfoMappings = memberInfoMappings;
            }

            public string GraphName { get; }
            public IGremlinQueryProvider Provider { get; }
            public IImmutableList<GremlinStep> Steps { get; }
            public IImmutableDictionary<MemberInfo, string> MemberInfoMappings { get; }
        }

        private sealed class GremlinQueryImpl<T> : GremlinQueryImpl, IGremlinQuery<T>
        {
            public GremlinQueryImpl(string graphName, IImmutableList<GremlinStep> steps, IGremlinQueryProvider provider, IImmutableDictionary<MemberInfo, string> memberInfoMappings) : base(graphName, steps, provider, memberInfoMappings)
            {
            }
        }

        public static IGremlinQuery ForGraph(string graphName, IGremlinQueryProvider provider)
        {
            return new GremlinQueryImpl(graphName, ImmutableList<GremlinStep>.Empty, provider, ImmutableDictionary<MemberInfo, string>.Empty);
        }

        public static IGremlinQuery<T> ToAnonymous<T>(this IGremlinQuery<T> query)
        {
            return new GremlinQueryImpl<T>("__", ImmutableList<GremlinStep>.Empty, query.Provider, query.MemberInfoMappings);
        }

        public static T First<T>(this IGremlinQuery<T> query)
        {
            return query
                .Limit(1)
                .Execute()
                .First();
        }

        public static Option<T> FirstOrNone<T>(this IGremlinQuery<T> query)
        {
            var array = query
                .Limit(1)
                .Execute()
                .ToArray();

            return array.Length > 0
                ? array[0]
                : Option<T>.None;
        }

        public static T[] ToArray<T>(this IGremlinQuery<T> query)
        {
            return query
                .Execute()
                .ToArray();
        }
        
        public static (string queryString, IDictionary<string, object> parameters) ToExecutableQuery(this IGremlinQuery query, bool inlineParameters = false)
        {
            var parameters = new Dictionary<string, object>();
            var queryString = CreateQueryString(query, parameters, inlineParameters);

            return (queryString, parameters);
        }

        internal static IGremlinQuery<T> AddStep<T>(this IGremlinQuery query, string name, params object[] parameters)
        {
            return new GremlinQueryImpl<T>(query.GraphName, query.Steps.Add(new GremlinStep(name, parameters)), query.Provider, query.MemberInfoMappings);
        }

        internal static IGremlinQuery<T> AddMemberInfoMapping<T>(this IGremlinQuery<T> query, Expression<Func<T, object>> memberExpression, string mapping)
        {
            var body = memberExpression.Body;
            if (body is UnaryExpression && body.NodeType == ExpressionType.Convert)
                body = ((UnaryExpression)body).Operand;

            var memberExpressionBody = body as MemberExpression;
            if (memberExpressionBody == null)
                throw new ArgumentException();

            return new GremlinQueryImpl<T>(query.GraphName, query.Steps, query.Provider, query.MemberInfoMappings.SetItem(memberExpressionBody.Member, mapping));
        }

        internal static IGremlinQuery<T> Cast<T>(this IGremlinQuery query)
        {
            return new GremlinQueryImpl<T>(query.GraphName, query.Steps, query.Provider, query.MemberInfoMappings);
        }

        internal static IGremlinQuery ReplaceProvider(this IGremlinQuery query, IGremlinQueryProvider provider)
        {
            return new GremlinQueryImpl(query.GraphName, query.Steps, provider, query.MemberInfoMappings);
        }

        internal static IGremlinQuery<T> ReplaceProvider<T>(this IGremlinQuery<T> query, IGremlinQueryProvider provider)
        {
            return new GremlinQueryImpl<T>(query.GraphName, query.Steps, provider, query.MemberInfoMappings);
        }

        private static string CreateQueryString(IGremlinQuery query, IDictionary<string, object> parameters, bool inlineParameters)
        {
            var builder = new StringBuilder(query.GraphName);

            foreach (var step in query.Steps)
            {
                var appendComma = false;

                if (builder.Length > 0)
                    builder.Append('.');

                builder.Append(step.Name);
                builder.Append("(");

                foreach (var parameter in step.Parameters)
                {
                    if (appendComma)
                        builder.Append(", ");
                    else
                        appendComma = true;

                    var gremlinSubQuery = parameter as IGremlinQuery;

                    if (gremlinSubQuery != null)
                        builder.Append(CreateQueryString(gremlinSubQuery, parameters, inlineParameters));
                    else
                    {
                        if (parameter is string && inlineParameters)
                            builder.Append($"'{parameter}'");
                        else
                        {
                            if (parameter is SpecialGremlinString || inlineParameters)
                                builder.Append(parameter);
                            else
                            {
                                var newParameterName = $"_P{parameters.Count}";
                                parameters.Add(newParameterName, parameter);

                                builder.Append(newParameterName);
                            }
                        }
                    }
                }

                builder.Append(")");
            }

            return builder.ToString();
        }
    }
}