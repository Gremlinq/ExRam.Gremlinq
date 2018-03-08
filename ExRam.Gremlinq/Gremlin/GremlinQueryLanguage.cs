using System;
using System.Collections;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Reflection;
using LanguageExt;
using Unit = System.Reactive.Unit;

namespace ExRam.Gremlinq
{
    public static class GremlinQueryLanguage
    {      
        public static IGremlinQuery<T> AddV<T>(this IGremlinQuery query, T vertex)
        {
            return query
                .AddStep<T>(new AddVGremlinStep(vertex))
                .AddStep<T>(new AddElementPropertiesStep(vertex));
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
                .AddStep<T>(new AddEGremlinStep(edge))
                .AddStep<T>(new AddElementPropertiesStep(edge));
        }

        public static IGremlinQuery<T> And<T>(this IGremlinQuery<T> query, params Func<IGremlinQuery<T>, IGremlinQuery>[] andTraversals)
        {
            return query.And<T>(andTraversals
                .Select(andTraversal => andTraversal(query.ToAnonymous())));
        }

        private static IGremlinQuery<T> And<T>(this IGremlinQuery query, IEnumerable<IGremlinQuery> andTraversals)
        {
            return query.AddStep<T>(
                "and",
                andTraversals.Aggregate(
                    ImmutableList<object>.Empty,
                    (list, query2) => query2.Steps.Count == 1 && (query2.Steps[0] as TerminalGremlinStep)?.Name == "and"
                        ? list.AddRange(((TerminalGremlinStep)query2.Steps[0]).Parameters)
                        : list.Add(query2)));
        }

        public static IGremlinQuery<TTarget> As<TSource, TTarget>(this IGremlinQuery<TSource> query, Func<IGremlinQuery<TSource>, StepLabel<TSource>, IGremlinQuery<TTarget>> continuation)
        {
            var stepLabel = query.IdentifierFactory.CreateStepLabel<TSource>();

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

        public static IGremlinQuery<TTarget> Coalesce<TSource, TTarget>(this IGremlinQuery<TSource> query, params Func<IGremlinQuery<TSource>, IGremlinQuery<TTarget>>[] traversals)
        {
            return query
                .AddStep<TTarget>(
                    "coalesce", 
                    traversals
                        .Select(traversal => traversal(query.ToAnonymous()))
                        .ToImmutableList<object>());
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
                .AddStep<Vertex>(new DerivedLabelNamesGremlinStep<T>("both"));
        }

        public static IGremlinQuery<T> BothE<T>(this IGremlinQuery query)
        {
            return query
                .AddStep<T>(new DerivedLabelNamesGremlinStep<T>("bothE"));
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
                .AddStep<T>("by", traversal(query.ToAnonymous()), new SpecialGremlinString("Order." + sortOrder.ToString().Substring(0, 4).ToLower()));
        }

        public static IGremlinQuery<T> ByMember<T>(this IGremlinQuery<T> query, Expression<Func<T, object>> projection, GremlinSortOrder sortOrder = GremlinSortOrder.Increasing)
        {
            var body = projection.Body;
            if (body is UnaryExpression && body.NodeType == ExpressionType.Convert)
                body = ((UnaryExpression)body).Operand;

            if (body is MemberExpression memberExpression)
            {
                return query
                    .AddStep<T>("by", memberExpression.Member.Name, new SpecialGremlinString("Order." + sortOrder.ToString().Substring(0, 4).ToLower()));
            }

            throw new NotSupportedException();
        }

        public static IGremlinQuery<T> ByLambda<T>(this IGremlinQuery<T> query, string lambdaString)
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

        public static IGremlinQuery<T> Dedup<T>(this IGremlinQuery<T> query)
        {
            return query
                .AddStep<T>("dedup");
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

        public static IGremlinQuery Emit(this IGremlinQuery query)
        {
            return query
                .Cast<Unit>()
                .Emit();
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

        public static IGremlinQuery<T[]> Fold<T>(this IGremlinQuery<T> query)
        {
            return query
                .AddStep<T[]>("fold");
        }

        public static IGremlinQuery<TSource> From<TSource, TStepLabel>(this IGremlinQuery<TSource> query, StepLabel<TStepLabel> stepLabel)
        {
            return query
                .AddStep<TSource>("from", stepLabel);
        }

        public static IGremlinQuery<T> From<T>(this IGremlinQuery<T> query, Func<IGremlinQuery<T>, IGremlinQuery> fromVertex)
        {
            return query
                .AddStep<T>("from", fromVertex(query.ToAnonymous()));
        }

        public static IGremlinQuery<T> Identity<T>(this IGremlinQuery<T> query)
        {
            return query
                .AddStep<T>("identity");
        }

        public static IGremlinQuery<Vertex> In<T>(this IGremlinQuery query)
        {
            return query
                .AddStep<Vertex>(new DerivedLabelNamesGremlinStep<T>("in"));
        }

        public static IGremlinQuery<T> InE<T>(this IGremlinQuery query)
        {
            return query
                .AddStep<T>(new DerivedLabelNamesGremlinStep<T>("inE"));
        }

        public static IGremlinQuery<T> InV<T>(this IGremlinQuery query)
        {
            return query
                .AddStep<Vertex>("inV")
                .OfType<T>();
        }

        public static IGremlinQuery<T> Inject<T>(this IGremlinQuery<T> query, params T[] elements)
        {
            return query
                .AddStep<T>("inject", elements);
        }

        public static IGremlinQuery<T> Limit<T>(this IGremlinQuery<T> query, long limit)
        {
            return query
                .AddStep<T>("limit", limit);
        }

        public static IGremlinQuery<TTarget> Local<TSource, TTarget>(this IGremlinQuery<TSource> query, Func<IGremlinQuery<TSource>, IGremlinQuery<TTarget>> localTraversal)
        {
            return query
                .AddStep<TTarget>("local", localTraversal(query.ToAnonymous()));
        }

        public static IGremlinQuery<TTarget> Map<TSource, TTarget>(this IGremlinQuery<TSource> query, Func<IGremlinQuery<TSource>, IGremlinQuery<TTarget>> mapping)
        {
            return query
                .AddStep<TTarget>("map", mapping(query.ToAnonymous()));
        }

        public static IGremlinQuery<TSource> Match<TSource>(this IGremlinQuery<TSource> query, params Func<IGremlinQuery<TSource>, IGremlinQuery<TSource>>[] matchTraversals)
        {
            return query
                // ReSharper disable once CoVariantArrayConversion
                .AddStep<TSource>("match", matchTraversals.Select(traversal => traversal(query.ToAnonymous())).ToArray());
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
                .AddStep<T>(new DerivedLabelNamesGremlinStep<T>("hasLabel"));
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
                orTraversals.Aggregate(
                    ImmutableList<object>.Empty,
                    (list, query2) => query2.Steps.Count == 1 && (query2.Steps[0] as TerminalGremlinStep)?.Name == "or"
                        ? list.AddRange(((TerminalGremlinStep)query2.Steps[0]).Parameters)
                        : list.Add(query2)));
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
                .AddStep<T>(new DerivedLabelNamesGremlinStep<T>("outE"));
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
                .AddStep<Vertex>(new DerivedLabelNamesGremlinStep<T>("out"));
        }

        public static IGremlinQuery<T> Profile<T>(this IGremlinQuery<T> query)
        {
            return query
                .AddStep<T>("profile");
        }
        
        public static IGremlinQuery<TSource> Property<TSource, TProperty>(this IGremlinQuery<TSource> query, Expression<Func<TSource, TProperty>> propertyExpression, TProperty property)
        {
            if (propertyExpression.Body is MemberExpression memberExpression)
            {
                if (memberExpression.Expression == propertyExpression.Parameters[0])
                {
                    return query.Property(memberExpression.Member.Name, property);
                }
            }

            throw new NotSupportedException();
        }

        private static IGremlinQuery<T> Property<T>(this IGremlinQuery<T> query, string key, object value)
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
                .AddStepLabelBinding(x => x.Item1, label1)
                .AddStepLabelBinding(x => x.Item2, label2);
        }

        public static IGremlinQuery<(T1, T2, T3)> Select<T1, T2, T3>(this IGremlinQuery query, StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3)
        {
            return query
                .AddStep<(T1, T2, T3)>("select", label1, label2, label3)
                .AddStepLabelBinding(x => x.Item1, label1)
                .AddStepLabelBinding(x => x.Item2, label2)
                .AddStepLabelBinding(x => x.Item3, label3);
        }

        public static IGremlinQuery<(T1, T2, T3, T4)> Select<T1, T2, T3, T4>(this IGremlinQuery query, StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3, StepLabel<T4> label4)
        {
            return query
                .AddStep<(T1, T2, T3, T4)>("select", label1, label2, label3, label4)
                .AddStepLabelBinding(x => x.Item1, label1)
                .AddStepLabelBinding(x => x.Item2, label2)
                .AddStepLabelBinding(x => x.Item3, label3)
                .AddStepLabelBinding(x => x.Item4, label4);
        }

        public static IGremlinQuery<T> SideEffect<T>(this IGremlinQuery<T> query, Func<IGremlinQuery<T>, IGremlinQuery> sideEffectTraversal)
        {
            return query
                .AddStep<T>("sideEffect", sideEffectTraversal(query.ToAnonymous()));
        }

        public static IGremlinQuery<T> Skip<T>(this IGremlinQuery<T> query, long skip)
        {
            return query
                .AddStep<T>("skip", skip);
        }

        public static IGremlinQuery<T> Sum<T>(this IGremlinQuery<T> query, bool local = false)
        {
            return local
                ? query
                    .AddStep<T>("sum", new SpecialGremlinString("Scope.local"))
                : query
                    .AddStep<T>("sum");
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

        public static IGremlinQuery<TSource> To<TSource, TStepLabel>(this IGremlinQuery<TSource> query, StepLabel<TStepLabel> stepLabel)
        {
            return query
                .AddStep<TSource>("to", stepLabel);
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

        public static IGremlinQuery<TTarget> Union<TSource, TTarget>(this IGremlinQuery<TSource> query, params Func<IGremlinQuery<TSource>, IGremlinQuery<TTarget>>[] unionTraversals)
        {
            return query
                .AddStep<TTarget>("union", unionTraversals
                    .Select(unionTraversal => unionTraversal(query.ToAnonymous()))
                    .ToImmutableList<object>());
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

        public static IGremlinQuery<TTarget> Values<TSource, TTarget>(this IGremlinQuery<TSource> query, params Expression<Func<TSource, TTarget>>[] projections)
        {
            return query.AddStep<TTarget>(
                "values",
                projections
                    .Select(projection =>
                    {
                        if (projection.Body is MemberExpression memberExpression)
                            return memberExpression.Member.Name;

                        throw new NotSupportedException();
                    })
                    .ToImmutableList<object>());
        }

        public static IGremlinQuery<T> Where<T>(this IGremlinQuery<T> query, Func<IGremlinQuery<T>, IGremlinQuery> filterTraversal)
        {
            return query
                .AddStep<T>("where", filterTraversal(query.ToAnonymous()));
        }
        
        public static IGremlinQuery<T> Where<T>(this IGremlinQuery<T> query, Expression<Func<T, bool>> predicate)
        {
            var body = predicate.Body;

            switch (body)
            {
                case UnaryExpression unaryExpression:
                {
                    if (unaryExpression.NodeType == ExpressionType.Not)
                        return query.Not(_ => _.Where(Expression.Lambda<Func<T, bool>>(unaryExpression.Operand, predicate.Parameters)));

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
                        switch (methodInfo.Name)
                        {
                            case nameof(Enumerable.Contains) when methodInfo.GetParameters().Length == 2:
                            {
                                if (methodCallExpression.Arguments[0] is MemberExpression leftMember && leftMember.Expression == predicate.Parameters[0])
                                    return query.Where(leftMember, GremlinPredicate.Eq(methodCallExpression.Arguments[1].GetValue()));

                                if (methodCallExpression.Arguments[1] is MemberExpression rightMember && rightMember.Expression == predicate.Parameters[0])
                                {
                                    if (methodCallExpression.Arguments[0].GetValue() is IEnumerable enumerable)
                                        return query.Where(rightMember, GremlinPredicate.Within(enumerable.Cast<object>().ToArray()));
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
                                    return query.Where(innerMemberExpression, GremlinPredicate.Within(arrayConstant.Cast<object>().ToArray()));
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
                                    return query.AddStep<T>(
                                        "has",
                                        argumentExpression.Member.Name,
                                        new OrGremlinPredicate(Enumerable
                                            .Range(0, stringValue.Length + 1)
                                            .Select(i => GremlinPredicate.Eq(stringValue.Substring(0, i)))
                                            .ToArray()));
                                }
                            }
                            else if (methodCallExpression.Object is MemberExpression memberExpression && memberExpression.Expression == predicate.Parameters[0])
                            {
                                if (methodCallExpression.Arguments[0].GetValue() is string lowerBound)
                                {
                                    if (lowerBound.Length == 0)
                                        return query;

                                    var upperBoundChars = lowerBound.ToCharArray();
                                    upperBoundChars[upperBoundChars.Length - 1]++;

                                    return query.Where(memberExpression, GremlinPredicate.Within(lowerBound, new string(upperBoundChars)));
                                }
                            }
                        }
                    }

                    break;
                }
            }

            throw new NotSupportedException();
        }

        private static IGremlinQuery<T> Where<T>(this IGremlinQuery<T> query, ParameterExpression parameter, Expression left, Expression right, ExpressionType nodeType)
        {
            if (nodeType == ExpressionType.OrElse || nodeType == ExpressionType.AndAlso)
            {
                var leftLambda = Expression.Lambda<Func<T, bool>>(left, parameter);
                var rightLambda = Expression.Lambda<Func<T, bool>>(right, parameter);

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

            if (right is ConstantExpression constantExpression && constantExpression.Value == null && nodeType == ExpressionType.Equal)
                return query.Not(__ => __.Where(parameter, left, right, ExpressionType.NotEqual));

            var constant = right.GetValue();
            var predicateArgument = GremlinPredicate.ForExpressionType(nodeType, constant);

            switch (left)
            {
                case MemberExpression leftMemberExpression when parameter == leftMemberExpression.Expression:
                    return query.Where(leftMemberExpression, predicateArgument);
                case ParameterExpression leftParameterExpression when parameter == leftParameterExpression:
                {
                    return query.AddStep<T>(
                        constant is StepLabel 
                            ? "where" 
                            : "is",
                        predicateArgument);
                }
            }

            throw new NotSupportedException();
        }

        private static IGremlinQuery<T> Where<T>(this IGremlinQuery<T> query, MemberExpression memberExpression, GremlinPredicate predicateArgument)
        {
            if (predicateArgument is GremlinPredicate gremlinPredicate && gremlinPredicate.Arguments.Length > 0 && gremlinPredicate.Arguments[0] is StepLabel)
                return query.AddStep<T>(new HasStep(memberExpression.Member.Name, (object)query.ToAnonymous().AddStep<T>("where", predicateArgument)));
            
            if (predicateArgument.Arguments.Length == 1 && predicateArgument.Arguments[0] == null)
                return query.AddStep<T>(new HasStep(memberExpression.Member.Name));

            return query.AddStep<T>(new HasStep(memberExpression.Member.Name, predicateArgument));
        }
    }
}