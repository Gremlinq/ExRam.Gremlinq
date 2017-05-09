using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
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
            public GremlinQueryImpl(string graphName, IImmutableList<GremlinStep> steps, IGremlinQueryProvider provider, IImmutableDictionary<MemberInfo, string> memberInfoMappings)
            {
                this.Steps = steps;
                this.Provider = provider;
                this.GraphName = graphName;
                this.MemberInfoMappings = memberInfoMappings;
            }

            public (string queryString, IDictionary<string, object> parameters) Serialize(IParameterCache parameterCache, bool inlineParameters)
            {
                var parameters = new Dictionary<string, object>();
                var builder = new StringBuilder(this.GraphName);

                foreach (var step in this.Steps)
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

                        if (parameter is IGremlinSerializable serializable)
                        {
                            var (innerQueryString, innerParameters) = serializable.Serialize(parameterCache, inlineParameters);
                            
                            builder.Append(innerQueryString);

                            foreach (var kvp in innerParameters)
                            {
                                parameters[kvp.Key] = kvp.Value;
                            }
                        }
                        else
                        {
                            if (parameter is string && inlineParameters)
                                builder.Append($"'{parameter}'");
                            else
                            {
                                if (inlineParameters)
                                    builder.Append(parameter);
                                else
                                {
                                    var newParameterName = parameterCache.Cache(parameter);
                                    parameters[newParameterName] = parameter;

                                    builder.Append(newParameterName);
                                }
                            }
                        }
                    }

                    builder.Append(")");
                }

                return (builder.ToString(), parameters);
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

        public static (string queryString, IDictionary<string, object> parameters) Serialize<T>(this IGremlinQuery<T> query, bool inlineParameters)
        {
            return query.Serialize(new DefaultParameterCache(), inlineParameters);
        }

        [Obsolete]
        public static T First<T>(this IGremlinQuery<T> query)
        {
            return query
                .FirstAsync()
                .Result;
        }

        public static Task<T> FirstAsync<T>(this IGremlinQuery<T> query)
        {
            return query
                .Limit(1)
                .Execute()
                .First();
        }

        [Obsolete]
        public static Option<T> FirstOrNone<T>(this IGremlinQuery<T> query)
        {
            return query
                .FirstOrNoneAsync()
                .Result;
        }

        public static async Task<Option<T>> FirstOrNoneAsync<T>(this IGremlinQuery<T> query)
        {
            var array = await query
                .Limit(1)
                .Execute()
                .ToArray();

            return array.Length > 0
                ? array[0]
                : Option<T>.None;
        }

        [Obsolete]
        public static T[] ToArray<T>(this IGremlinQuery<T> query)
        {
            return query
                .ToArrayAsync()
                .Result;
        }

        public static Task<T[]> ToArrayAsync<T>(this IGremlinQuery<T> query)
        {
            return query
                .Execute()
                .ToArray();
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
    }
}