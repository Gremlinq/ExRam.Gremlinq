using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Collections.Concurrent;
using System.Collections.Generic;
using LanguageExt;
using Unit = System.Reactive.Unit;

namespace ExRam.Gremlinq
{
    public static class GremlinQueryLanguage
    {
        private static readonly IReadOnlyDictionary<ExpressionType, string> SupportedComparisons = new Dictionary<ExpressionType, string>
        {
            { ExpressionType.Equal, "eq" },
            { ExpressionType.NotEqual, "neq" },
            { ExpressionType.LessThan, "lt" },
            { ExpressionType.LessThanOrEqual, "lte" },
            { ExpressionType.GreaterThanOrEqual, "gte" },
            { ExpressionType.GreaterThan, "gt" }
        };

        private static readonly ConcurrentDictionary<(IGraphModel model, Type type), string[]> TypeLabelDict = new ConcurrentDictionary<(IGraphModel, Type), string[]>();

        public static IGremlinQuery<T> AddV<T>(this IGremlinQuery query, T vertex)
        {
            return query
                .AddStep<T>("addV", query.Provider.Model
                    .TryGetLabelOfType(vertex.GetType())
                    .IfNone(typeof(T).Name))
                .AddProperties(vertex);
        }

        public static IGremlinQuery<T> AddV<T>(this IGremlinQuery query)
            where T : new()
        {
            return query.AddV(new T());
        }

        public static IGremlinQuery<T> AddE<T>(this IGremlinQuery query)
            where T : new()
        {
            return query.AddE(new T());
        }

        public static IGremlinQuery<T> AddE<T>(this IGremlinQuery query, T edge)
        {
            return query
                .AddStep<T>("addE", query.Provider.Model
                    .TryGetLabelOfType(edge.GetType())
                    .IfNone(typeof(T).Name))
                .AddProperties(edge);
        }

        private static IGremlinQuery<T> AddProperties<T>(this IGremlinQuery<T> query, T element)
        {
            return typeof(T).GetProperties()
                .Where(property => property.PropertyType.IsNativeType())
                .Select(property => (name: property.Name, value: property.GetValue(element)))
                .Where(tuple => tuple.value != null)
                .Aggregate(query, (current, tuple) => current.Property(tuple.name, tuple.value));
        }

        public static IGremlinQuery<T> And<T>(this IGremlinQuery<T> query, params Func<IGremlinQuery<T>, IGremlinQuery>[] andTraversals)
        {
            return query
                .AddStep<T>(
                    "and",
                    // ReSharper disable once CoVariantArrayConversion
                    andTraversals
                        .Select(andTraversal => andTraversal(query.ToAnonymous()))
                        .ToArray());
        }

        public static IGremlinQuery<T> As<S, T>(this IGremlinQuery<S> query, Func<IGremlinQuery<S>, StepLabel<S>, IGremlinQuery<T>> continuation)
        {
            var stepLabel = query.StepLabelFactory.Create<S>();

            return continuation(
                query.As(stepLabel),
                stepLabel);
        }

        public static IGremlinQuery<T> As<T>(this IGremlinQuery<T> query, StepLabel<T> stepLabel)
        {
            return query
                .AddStep<T>("as", stepLabel);
        }

        public static IGremlinQuery<T> Barrier<T>(this IGremlinQuery<T> query)
        {
            return query
                .AddStep<T>("barrier");
        }

        public static IGremlinQuery<T> Coalesce<T>(this IGremlinQuery query, params Func<IGremlinQuery<Unit>, IGremlinQuery<T>>[] traversals)
        {
            return query
                .Cast<Unit>()
                .Coalesce(traversals);
        }

        public static IGremlinQuery<T> Coalesce<S, T>(this IGremlinQuery<S> query, params Func<IGremlinQuery<S>, IGremlinQuery<T>>[] traversals)
        {
            return query
                .AddStep<T>(
                    "coalesce", 
                    // ReSharper disable once CoVariantArrayConversion
                    traversals
                        .Select(traversal => traversal(query.ToAnonymous()))
                        .ToArray());
        }

        public static IGremlinQuery<TResult> Choose<TSource, TResult>(this IGremlinQuery<TSource> query, Func<IGremlinQuery<TSource>, IGremlinQuery> traversalPredicate, Func<IGremlinQuery<TSource>, IGremlinQuery<TResult>> trueChoice, Func<IGremlinQuery<TSource>, IGremlinQuery<TResult>> falseChoice)
        {
            var anonymous = query.ToAnonymous();

            return query
                .AddStep<TResult>(
                    "choose",
                    traversalPredicate(anonymous),
                    trueChoice(anonymous),
                    falseChoice(anonymous));
        }

        public static IGremlinQuery<TResult> Choose<TSource, TResult>(this IGremlinQuery<TSource> query, Func<IGremlinQuery<TSource>, IGremlinQuery> traversalPredicate, Func<IGremlinQuery<TSource>, IGremlinQuery<TResult>> trueChoice)
        {
            var anonymous = query.ToAnonymous();

            return query
                .AddStep<TResult>(
                    "choose",
                    traversalPredicate(anonymous),
                    trueChoice(anonymous));
        }

        public static IGremlinQuery<Vertex> Both<T>(this IGremlinQuery query)
        {
            return query
                // ReSharper disable once CoVariantArrayConversion
                .AddStep<Vertex>("both", GetDerivedLabelNames<T>(query.Provider.Model));
        }

        public static IGremlinQuery<T> BothE<T>(this IGremlinQuery query)
        {
            return query
                // ReSharper disable once CoVariantArrayConversion
                .AddStep<T>("bothE", GetDerivedLabelNames<T>(query.Provider.Model));
        }

        public static IGremlinQuery<Vertex> BothV(this IGremlinQuery query)
        {
            return query
                .AddStep<Vertex>("bothV");
        }

        public static IGremlinQuery<TEnd> BranchOnIdentity<TStart, TEnd>(this IGremlinQuery<TStart> query, params Func<IGremlinQuery<TStart>, IGremlinQuery<TEnd>>[] options)
        {
            return query
                .Branch(_ => _.Identity(), options);
        }

        public static IGremlinQuery<TEnd> Branch<TStart, TBranch, TEnd>(this IGremlinQuery<TStart> query, Func<IGremlinQuery<TStart>, IGremlinQuery<TBranch>> branchSelector, params Func<IGremlinQuery<TBranch>, IGremlinQuery<TEnd>>[] options)
        {
            return options
                .Aggregate(
                    query
                        .AddStep<TBranch>("branch", branchSelector(query.ToAnonymous())),
                    (branchQuery, option) => branchQuery.AddStep<TBranch>("option", option(branchQuery.ToAnonymous())))
                .Cast<TEnd>();
        }

        public static IGremlinQuery<T> ByTraversal<T>(this IGremlinQuery<T> query, Func<IGremlinQuery<T>, IGremlinQuery> traversal, GremlinSortOrder sortOrder = GremlinSortOrder.Increasing)
        {
            return query
                .AddStep<T>("by", traversal(query.ToAnonymous()), new SpecialGremlinString(sortOrder.ToString().Substring(0, 4).ToLower()));
        }

        public static IGremlinQuery<T> ByMember<T>(this IGremlinQuery<T> query, Expression<Func<T, object>> projection, GremlinSortOrder sortOrder = GremlinSortOrder.Increasing)
        {
            var body = projection.Body;
            if (body is UnaryExpression && body.NodeType == ExpressionType.Convert)
                body = ((UnaryExpression)body).Operand;

            var memberExpression = body as MemberExpression;
            if (memberExpression != null)
            {
                return query
                    .AddStep<T>("by", memberExpression.Member.Name, new SpecialGremlinString(sortOrder.ToString().Substring(0, 4).ToLower()));
            }

            throw new NotSupportedException();
        }

        public static IGremlinQuery<T> ByLambda<T>(this IGremlinQuery<T> query, string lambdaString) //TODO: Quick and dirty
        {
            return query
                .AddStep<T>("by", new SpecialGremlinString($"{{{lambdaString}}}"));
        }

        public static IGremlinQuery<T> Choose<T>(this IGremlinQuery query, IGremlinQuery traversalPredicate, IGremlinQuery<T> trueChoice, IGremlinQuery<T> falseChoice)
        {
            return query
                .AddStep<T>("choose", traversalPredicate, trueChoice, falseChoice);
        }

        public static IGremlinQuery<T> Choose<T>(this IGremlinQuery query, IGremlinQuery traversalPredicate, IGremlinQuery<T> trueChoice)
        {
            return query
                .AddStep<T>("choose", traversalPredicate, trueChoice);
        }

        public static IGremlinQuery<Unit> Drop(this IGremlinQuery query)
        {
            return query
                .AddStep<Unit>("drop");
        }

        public static IGremlinQuery<Edge> E(this IGremlinQuery query, params object[] ids)
        {
            return query
                .AddStep<Edge>("E", ids);
        }

        public static IGremlinQuery<T> E<T>(this IGremlinQuery query, params object[] ids)
        {
            return query
                .E(ids)
                .OfType<T>();
        }

        public static IGremlinQuery<T> Emit<T>(this IGremlinQuery<T> query)
        {
            return query
                .AddStep<T>("emit");
        }

        public static IGremlinQuery<T> Explain<T>(this IGremlinQuery<T> query)
        {
            return query
                .AddStep<T>("explain");
        }

        public static IGremlinQuery<T> FilterWithLambda<T>(this IGremlinQuery<T> query, string lambda)
        {
            return query
                .AddStep<T>("filter", new SpecialGremlinString($"{{{lambda}}}"));
        }

        public static IGremlinQuery<T> Filter<T>(this IGremlinQuery<T> query, Func<IGremlinQuery<T>, IGremlinQuery> filterTraversal)
        {
            return query
                .AddStep<T>("filter", filterTraversal(query.ToAnonymous()));
        }

        public static IGremlinQuery<T[]> Fold<T>(this IGremlinQuery<T> query)
        {
            return query
                .AddStep<T[]>("fold");
        }

        public static IGremlinQuery<T> From<S, T>(this IGremlinQuery<T> query, StepLabel<S> stepLabel)
        {
            return query
                .AddStep<T>("from", stepLabel);
        }

        public static IGremlinQuery<T> From<T>(this IGremlinQuery<T> query, Func<IGremlinQuery<T>, IGremlinQuery> fromVertex)
        {
            return query
                .AddStep<T>("from", fromVertex(query.ToAnonymous()));
        }

        // ReSharper disable once SuggestBaseTypeForParameter
        private static IGremlinQuery<T> HasLabel<T>(this IGremlinQuery<T> query, params string[] labels)
        {
            return query
                // ReSharper disable once CoVariantArrayConversion
                .AddStep<T>("hasLabel", labels);
        }

        public static IGremlinQuery<T> Identity<T>(this IGremlinQuery<T> query)
        {
            return query
                .AddStep<T>("identity");
        }

        public static IGremlinQuery<Vertex> In<T>(this IGremlinQuery query)
        {
            return query
                // ReSharper disable once CoVariantArrayConversion
                .AddStep<Vertex>("in", GetDerivedLabelNames<T>(query.Provider.Model));
        }

        public static IGremlinQuery<T> InE<T>(this IGremlinQuery query)
        {
            return query
                // ReSharper disable once CoVariantArrayConversion
                .AddStep<T>("inE", GetDerivedLabelNames<T>(query.Provider.Model));
        }

        public static IGremlinQuery<T> InV<T>(this IGremlinQuery query)
        {
            return query
                .AddStep<Vertex>("inV")
                .OfType<T>();
        }

        public static IGremlinQuery<T> Limit<T>(this IGremlinQuery<T> query, long limit)
        {
            return query
                .AddStep<T>("limit", limit);
        }

        public static IGremlinQuery<T> Limit<S, T>(this IGremlinQuery<S> query, Func<IGremlinQuery<S>, IGremlinQuery<T>> localTraversal)
        {
            return query
                .AddStep<T>("local", localTraversal(query.ToAnonymous()));
        }

        public static IGremlinQuery<T> Map<S, T>(this IGremlinQuery<S> query, Func<IGremlinQuery<S>, IGremlinQuery<T>> mapping)
        {
            return query
                .AddStep<T>("map", mapping(query.ToAnonymous()));
        }

        public static IGremlinQuery<T> Not<T>(this IGremlinQuery<T> query, Func<IGremlinQuery<T>, IGremlinQuery> notTraversal)
        {
            return query
                .AddStep<T>("not", notTraversal(query.ToAnonymous()));
        }

        public static IGremlinQuery<T> OfType<T>(this IGremlinQuery query)
        {
            return query
                .Cast<T>()
                .HasLabel(query.Provider.Model.GetDerivedLabelNames<T>());
        }

        public static IGremlinQuery<T> Optional<T>(this IGremlinQuery<T> query, Func<IGremlinQuery<T>, IGremlinQuery<T>> optionalTraversal)
        {
            return query
                .AddStep<T>("optional", optionalTraversal(query.ToAnonymous()));
        }

        public static IGremlinQuery<T> Or<T>(this IGremlinQuery<T> query, params Func<IGremlinQuery<T>, IGremlinQuery>[] orTraversals)
        {
            return query.Or<T>(
                orTraversals.Select(andTraversal => andTraversal(query.ToAnonymous())));
        }

        private static IGremlinQuery<T> Or<T>(this IGremlinQuery query, IEnumerable<IGremlinQuery> orTraversals)
        {
            return query.AddStep<T>(
                "or",
                // ReSharper disable once CoVariantArrayConversion
                orTraversals.ToArray());
        }

        public static IGremlinQuery<T> Order<T>(this IGremlinQuery<T> query)
        {
            return query
                .AddStep<T>("order");
        }

        public static IGremlinQuery<T> OtherV<T>(this IGremlinQuery query)
        {
            return query
                .AddStep<T>("otherV");
        }

        public static IGremlinQuery<T> OutE<T>(this IGremlinQuery query)
        {
            return query
                // ReSharper disable once CoVariantArrayConversion
                .AddStep<T>("outE", GetDerivedLabelNames<T>(query.Provider.Model));
        }

        public static IGremlinQuery<T> OutV<T>(this IGremlinQuery query)
        {
            return query
                .AddStep<Vertex>("outV")
                .OfType<T>();
        }

        public static IGremlinQuery<Vertex> Out<T>(this IGremlinQuery query)
        {
            return query
                // ReSharper disable once CoVariantArrayConversion
                .AddStep<Vertex>("out", GetDerivedLabelNames<T>(query.Provider.Model));
        }

        public static IGremlinQuery<T> Profile<T>(this IGremlinQuery<T> query)
        {
            return query
                .AddStep<T>("profile");
        }

        public static IGremlinQuery<T> Property<T>(this IGremlinQuery<T> query, string key, object value)
        {
            return query
                .AddStep<T>("property", key, value);
        }

        public static IGremlinQuery<T> Range<T>(this IGremlinQuery<T> query, long low, long high)
        {
            return query
                .AddStep<T>("range", low, high);
        }

        public static IGremlinQuery<T> Repeat<T>(this IGremlinQuery query, Func<IGremlinQuery<Unit>, IGremlinQuery<T>> repeatTraversal)
        {
            return query
                .AddStep<T>("repeat", repeatTraversal(query.Cast<Unit>().ToAnonymous()));
        }

        public static IGremlinQuery<T> Select<T>(this IGremlinQuery query, StepLabel<T> label)
        {
            return query
                .AddStep<T>("select", label);
        }

        public static IGremlinQuery<(T1, T2)> Select<T1, T2>(this IGremlinQuery query, StepLabel<T1> label1, StepLabel<T2> label2)
        {
            return query
                .AddStep<(T1, T2)>("select", label1, label2)
                .AddMemberInfoMapping(x => x.Item1, label1.Label)
                .AddMemberInfoMapping(x => x.Item2, label2.Label);
        }

        public static IGremlinQuery<(T1, T2, T3)> Select<T1, T2, T3>(this IGremlinQuery query, StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3)
        {
            return query
                .AddStep<(T1, T2, T3)>("select", label1, label2, label3)
                .AddMemberInfoMapping(x => x.Item1, label1.Label)
                .AddMemberInfoMapping(x => x.Item2, label2.Label)
                .AddMemberInfoMapping(x => x.Item3, label3.Label);
        }

        public static IGremlinQuery<(T1, T2, T3, T4)> Select<T1, T2, T3, T4>(this IGremlinQuery query, StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3, StepLabel<T4> label4)
        {
            return query
                .AddStep<(T1, T2, T3, T4)>("select", label1, label2, label3, label4)
                .AddMemberInfoMapping(x => x.Item1, label1.Label)
                .AddMemberInfoMapping(x => x.Item2, label2.Label)
                .AddMemberInfoMapping(x => x.Item3, label3.Label)
                .AddMemberInfoMapping(x => x.Item4, label4.Label);
        }

        public static IGremlinQuery<T> Times<T>(this IGremlinQuery<T> query, int count)
        {
            return query
                .AddStep<T>("times", count);
        }

        public static IGremlinQuery<T> Tail<T>(this IGremlinQuery<T> query, long limit)
        {
            return query
                .AddStep<T>("tail", limit);
        }

        public static IGremlinQuery<T> To<S, T>(this IGremlinQuery<T> query, StepLabel<S> stepLabel)
        {
            return query
                .AddStep<T>("to", stepLabel);
        }

        public static IGremlinQuery<T> To<T>(this IGremlinQuery<T> query, Func<IGremlinQuery<T>, IGremlinQuery> toVertex)
        {
            return query
                .AddStep<T>("to", toVertex(query.ToAnonymous()));
        }

        public static IGremlinQuery<T> Unfold<T>(this IGremlinQuery<IEnumerable<T>> query)
        {
            return query
                .AddStep<T>("unfold");
        }

        public static IGremlinQuery<T> Union<S, T>(this IGremlinQuery<S> query, params Func<IGremlinQuery<S>, IGremlinQuery<T>>[] unionTraversals)
        {
            return query
                // ReSharper disable once CoVariantArrayConversion
                .AddStep<T>("union", unionTraversals.Select(unionTraversal => unionTraversal(query.ToAnonymous())).ToArray());
        }

        public static IGremlinQuery<T> Until<T>(this IGremlinQuery<T> query, Func<IGremlinQuery<T>, IGremlinQuery> untilTraversal)
        {
            return query
                .AddStep<T>("until", untilTraversal(query.ToAnonymous()));
        }

        public static IGremlinQuery<Vertex> V(this IGremlinQuery query, params object[] ids)
        {
            return query
                .AddStep<Vertex>("V", ids);
        }

        public static IGremlinQuery<T> V<T>(this IGremlinQuery query, params object[] ids)
        {
            return query
                .V(ids)
                .OfType<T>();
        }

        public static IGremlinQuery<T> Values<S, T>(this IGremlinQuery<S> query, params Expression<Func<S, T>>[] projections)
        {
            return query.AddStep<T>(
                "values",
                // ReSharper disable once CoVariantArrayConversion
                projections
                    .Select(projection =>
                    {
                        var memberExpression = projection.Body as MemberExpression;
                        if (memberExpression != null)
                            return memberExpression.Member.Name;

                        throw new NotSupportedException();
                    })
                    .ToArray());
        }

        public static IGremlinQuery<T> Where<T>(this IGremlinQuery<T> query, Expression<Func<T, bool>> predicate)
        {
            var binaryExpression = predicate.Body as BinaryExpression;

            if (binaryExpression != null)
            {
                object constant;
                var left = StripConvert(binaryExpression.Left);
                var right = StripConvert(binaryExpression.Right);

                var constantExpression = right as ConstantExpression;
                if (constantExpression != null)
                    constant = constantExpression.Value;
                else
                {
                    var getterLambda = Expression
                        .Lambda<Func<object>>(Expression.Convert(right, typeof(object)))
                        .Compile();

                    constant = getterLambda();
                }

                var predicateArgument = constant != null
                    ? GremlinQueryLanguage.SupportedComparisons
                        .TryGetValue(binaryExpression.NodeType)
                        .Map(predicateName => (object) new GremlinPredicate(predicateName, constant))
                        .IfNone(constant)
                    : null;

                if (left is MemberExpression leftMemberExpression)
                {
                    var memberArgument = StripConvert(leftMemberExpression.Expression);

                    if (predicate.Parameters[0] == memberArgument)
                    {
                        if (predicateArgument != null)
                            return query.AddStep<T>("has", leftMemberExpression.Member.Name, predicateArgument);

                        var step = new GremlinStep("has", leftMemberExpression.Member.Name);

                        if (binaryExpression.NodeType == ExpressionType.Equal)
                            return query.Not(_ => _.AddStep<T>(step));

                        if (binaryExpression.NodeType == ExpressionType.NotEqual)
                            return query.AddStep<T>(step);
                    }
                }
                else if ((left is ParameterExpression leftParameterExpression) && (predicateArgument != null))
                {
                    if (predicate.Parameters[0] == leftParameterExpression)
                        return query.AddStep<T>("where", predicateArgument);
                }
            }

            throw new NotSupportedException();
        }

        private static Expression StripConvert(Expression expression)
        {
            var unaryExpression = expression as UnaryExpression;
            if (unaryExpression != null && expression.NodeType == ExpressionType.Convert)
                return StripConvert(unaryExpression.Operand);

            return expression;
        }

        private static string[] GetDerivedLabelNames<T>(this IGraphModel model)
        {
            return TypeLabelDict
                .GetOrAdd(
                    (model, typeof(T)),
                    tuple => tuple.model
                        .GetDerivedElementInfos(typeof(T), true)
                        .Select(elementInfo => elementInfo.Label)
                        .ToArray());
        }

        private static bool IsNativeType(this Type type)   //TODO: Native types are a matter of...what?
        {
            return type.GetTypeInfo().IsValueType || type == typeof(string) || type.IsArray && type.GetElementType().IsNativeType();
        }
    }
}