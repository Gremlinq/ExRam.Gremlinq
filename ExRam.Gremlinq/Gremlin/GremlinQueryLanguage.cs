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
        public static IGremlinQuery<TElement> AddV<TElement>(this IGremlinQuery query, TElement vertex)
        {
            return query
                .AddStep<TElement>(new AddVGremlinStep(vertex))
                .AddStep<TElement>(new AddElementPropertiesStep(vertex));
        }

        public static IGremlinQuery<TElement> AddV<TElement>(this IGremlinQuery query)
            where TElement : new()
        {
            return query.AddV(new TElement());
        }

        public static IGremlinQuery<TElement> AddE<TElement>(this IGremlinQuery query)
            where TElement : new()
        {
            return query.AddE(new TElement());
        }

        public static IGremlinQuery<TElement> AddE<TElement>(this IGremlinQuery query, TElement edge)
        {
            return query
                .AddStep<TElement>(new AddEGremlinStep(edge))
                .AddStep<TElement>(new AddElementPropertiesStep(edge));
        }

        public static IGremlinQuery<TElement> And<TElement>(this IGremlinQuery<TElement> query, params Func<IGremlinQuery<TElement>, IGremlinQuery>[] andTraversals)
        {
            return query.And<TElement>(andTraversals
                .Select(andTraversal => andTraversal(query.ToAnonymous())));
        }

        private static IGremlinQuery<TElement> And<TElement>(this IGremlinQuery query, IEnumerable<IGremlinQuery> andTraversals)
        {
            return query.AddStep<TElement>(
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

        public static IGremlinQuery<TElement> As<TElement>(this IGremlinQuery<TElement> query, StepLabel<TElement> stepLabel)
        {
            return query
                .AddStep<TElement>("as", stepLabel);
        }

        public static IGremlinQuery<TElement> Barrier<TElement>(this IGremlinQuery<TElement> query)
        {
            return query
                .AddStep<TElement>("barrier");
        }

        public static IGremlinQuery<TElement> Coalesce<TElement>(this IGremlinQuery query, params Func<IGremlinQuery<Unit>, IGremlinQuery<TElement>>[] traversals)
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

        public static IGremlinQuery<Vertex> Both<TElement>(this IGremlinQuery query)
        {
            return query
                .AddStep<Vertex>(new DerivedLabelNamesGremlinStep<TElement>("both"));
        }

        public static IGremlinQuery<TElement> BothE<TElement>(this IGremlinQuery query)
        {
            return query
                .AddStep<TElement>(new DerivedLabelNamesGremlinStep<TElement>("bothE"));
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

        public static IGremlinQuery<TElement> ByTraversal<TElement>(this IGremlinQuery<TElement> query, Func<IGremlinQuery<TElement>, IGremlinQuery> traversal, GremlinSortOrder sortOrder = GremlinSortOrder.Increasing)
        {
            return query
                .AddStep<TElement>("by", traversal(query.ToAnonymous()), new SpecialGremlinString("Order." + sortOrder.ToString().Substring(0, 4).ToLower()));
        }

        public static IGremlinQuery<TElement> ByMember<TElement>(this IGremlinQuery<TElement> query, Expression<Func<TElement, object>> projection, GremlinSortOrder sortOrder = GremlinSortOrder.Increasing)
        {
            var body = projection.Body;
            if (body is UnaryExpression && body.NodeType == ExpressionType.Convert)
                body = ((UnaryExpression)body).Operand;

            if (body is MemberExpression memberExpression)
            {
                return query
                    .AddStep<TElement>("by", memberExpression.Member.Name, new SpecialGremlinString("Order." + sortOrder.ToString().Substring(0, 4).ToLower()));
            }

            throw new NotSupportedException();
        }

        public static IGremlinQuery<TElement> ByLambda<TElement>(this IGremlinQuery<TElement> query, string lambdaString)
        {
            return query
                .AddStep<TElement>("by", new SpecialGremlinString($"{{{lambdaString}}}"));
        }

        public static IGremlinQuery<TElement> Choose<TElement>(this IGremlinQuery query, IGremlinQuery traversalPredicate, IGremlinQuery<TElement> trueChoice, IGremlinQuery<TElement> falseChoice)
        {
            return query
                .AddStep<TElement>("choose", traversalPredicate, trueChoice, falseChoice);
        }

        public static IGremlinQuery<TElement> Choose<TElement>(this IGremlinQuery query, IGremlinQuery traversalPredicate, IGremlinQuery<TElement> trueChoice)
        {
            return query
                .AddStep<TElement>("choose", traversalPredicate, trueChoice);
        }

        public static IGremlinQuery<TElement> Dedup<TElement>(this IGremlinQuery<TElement> query)
        {
            return query
                .AddStep<TElement>("dedup");
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

        public static IGremlinQuery<TElement> E<TElement>(this IGremlinQuery query, params object[] ids)
        {
            return query
                .E(ids)
                .OfType<TElement>();
        }

        public static IGremlinQuery<TElement> Emit<TElement>(this IGremlinQuery<TElement> query)
        {
            return query
                .AddStep<TElement>("emit");
        }

        public static IGremlinQuery Emit(this IGremlinQuery query)
        {
            return query
                .Cast<Unit>()
                .Emit();
        }

        public static IGremlinQuery<TElement> Explain<TElement>(this IGremlinQuery<TElement> query)
        {
            return query
                .AddStep<TElement>("explain");
        }

        public static IGremlinQuery<TElement> FilterWithLambda<TElement>(this IGremlinQuery<TElement> query, string lambda)
        {
            return query
                .AddStep<TElement>("filter", new SpecialGremlinString($"{{{lambda}}}"));
        }

        public static IGremlinQuery<TElement[]> Fold<TElement>(this IGremlinQuery<TElement> query)
        {
            return query
                .AddStep<TElement[]>("fold");
        }

        public static IGremlinQuery<TSource> From<TSource, TStepLabel>(this IGremlinQuery<TSource> query, StepLabel<TStepLabel> stepLabel)
        {
            return query
                .AddStep<TSource>("from", stepLabel);
        }

        public static IGremlinQuery<TElement> From<TElement>(this IGremlinQuery<TElement> query, Func<IGremlinQuery<TElement>, IGremlinQuery> fromVertex)
        {
            return query
                .AddStep<TElement>("from", fromVertex(query.ToAnonymous()));
        }

        public static IGremlinQuery<TElement> Identity<TElement>(this IGremlinQuery<TElement> query)
        {
            return query
                .AddStep<TElement>("identity");
        }

        public static IGremlinQuery<Vertex> In<TElement>(this IGremlinQuery query)
        {
            return query
                .AddStep<Vertex>(new DerivedLabelNamesGremlinStep<TElement>("in"));
        }

        public static IGremlinQuery<TElement> InE<TElement>(this IGremlinQuery query)
        {
            return query
                .AddStep<TElement>(new DerivedLabelNamesGremlinStep<TElement>("inE"));
        }

        public static IGremlinQuery<TElement> InV<TElement>(this IGremlinQuery query)
        {
            return query
                .AddStep<Vertex>("inV")
                .OfType<TElement>();
        }

        public static IGremlinQuery<TElement> Inject<TElement>(this IGremlinQuery<TElement> query, params TElement[] elements)
        {
            return query
                .AddStep<TElement>("inject", elements);
        }

        public static IGremlinQuery<TElement> Limit<TElement>(this IGremlinQuery<TElement> query, long limit)
        {
            return query
                .AddStep<TElement>("limit", limit);
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

        public static IGremlinQuery<TElement> Not<TElement>(this IGremlinQuery<TElement> query, Func<IGremlinQuery<TElement>, IGremlinQuery> notTraversal)
        {
            return query
                .AddStep<TElement>("not", notTraversal(query.ToAnonymous()));
        }

        public static IGremlinQuery<TElement> OfType<TElement>(this IGremlinQuery query)
        {
            return query
                .Cast<TElement>()
                .AddStep<TElement>(new DerivedLabelNamesGremlinStep<TElement>("hasLabel"));
        }

        public static IGremlinQuery<TElement> Optional<TElement>(this IGremlinQuery<TElement> query, Func<IGremlinQuery<TElement>, IGremlinQuery<TElement>> optionalTraversal)
        {
            return query
                .AddStep<TElement>("optional", optionalTraversal(query.ToAnonymous()));
        }

        public static IGremlinQuery<TElement> Or<TElement>(this IGremlinQuery<TElement> query, params Func<IGremlinQuery<TElement>, IGremlinQuery>[] orTraversals)
        {
            return query.Or<TElement>(
                orTraversals.Select(andTraversal => andTraversal(query.ToAnonymous())));
        }

        private static IGremlinQuery<TElement> Or<TElement>(this IGremlinQuery query, IEnumerable<IGremlinQuery> orTraversals)
        {
            return query.AddStep<TElement>(
                "or",
                orTraversals.Aggregate(
                    ImmutableList<object>.Empty,
                    (list, query2) => query2.Steps.Count == 1 && (query2.Steps[0] as TerminalGremlinStep)?.Name == "or"
                        ? list.AddRange(((TerminalGremlinStep)query2.Steps[0]).Parameters)
                        : list.Add(query2)));
        }

        public static IGremlinQuery<TElement> Order<TElement>(this IGremlinQuery<TElement> query)
        {
            return query
                .AddStep<TElement>("order");
        }

        public static IGremlinQuery<TElement> OtherV<TElement>(this IGremlinQuery query)
        {
            return query
                .AddStep<TElement>("otherV");
        }

        public static IGremlinQuery<TElement> OutE<TElement>(this IGremlinQuery query)
        {
            return query
                .AddStep<TElement>(new DerivedLabelNamesGremlinStep<TElement>("outE"));
        }

        public static IGremlinQuery<TElement> OutV<TElement>(this IGremlinQuery query)
        {
            return query
                .AddStep<Vertex>("outV")
                .OfType<TElement>();
        }

        public static IGremlinQuery<Vertex> Out<TElement>(this IGremlinQuery query)
        {
            return query
                .AddStep<Vertex>(new DerivedLabelNamesGremlinStep<TElement>("out"));
        }

        public static IGremlinQuery<TElement> Profile<TElement>(this IGremlinQuery<TElement> query)
        {
            return query
                .AddStep<TElement>("profile");
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

        private static IGremlinQuery<TElement> Property<TElement>(this IGremlinQuery<TElement> query, string key, object value)
        {
            return query
                .AddStep<TElement>("property", key, value);
        }

        public static IGremlinQuery<TElement> Range<TElement>(this IGremlinQuery<TElement> query, long low, long high)
        {
            return query
                .AddStep<TElement>("range", low, high);
        }

        public static IGremlinQuery<TElement> Repeat<TElement>(this IGremlinQuery query, Func<IGremlinQuery<Unit>, IGremlinQuery<TElement>> repeatTraversal)
        {
            return query
                .AddStep<TElement>("repeat", repeatTraversal(query.Cast<Unit>().ToAnonymous()));
        }

        public static IGremlinQuery<TElement> Select<TElement>(this IGremlinQuery query, StepLabel<TElement> label)
        {
            return query
                .AddStep<TElement>("select", label);
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

        public static IGremlinQuery<TElement> SideEffect<TElement>(this IGremlinQuery<TElement> query, Func<IGremlinQuery<TElement>, IGremlinQuery> sideEffectTraversal)
        {
            return query
                .AddStep<TElement>("sideEffect", sideEffectTraversal(query.ToAnonymous()));
        }

        public static IGremlinQuery<TElement> Skip<TElement>(this IGremlinQuery<TElement> query, long skip)
        {
            return query
                .AddStep<TElement>("skip", skip);
        }

        public static IGremlinQuery<TElement> Sum<TElement>(this IGremlinQuery<TElement> query, bool local = false)
        {
            return local
                ? query
                    .AddStep<TElement>("sum", new SpecialGremlinString("Scope.local"))
                : query
                    .AddStep<TElement>("sum");
        }

        public static IGremlinQuery<TElement> Times<TElement>(this IGremlinQuery<TElement> query, int count)
        {
            return query
                .AddStep<TElement>("times", count);
        }

        public static IGremlinQuery<TElement> Tail<TElement>(this IGremlinQuery<TElement> query, long limit)
        {
            return query
                .AddStep<TElement>("tail", limit);
        }

        public static IGremlinQuery<TSource> To<TSource, TStepLabel>(this IGremlinQuery<TSource> query, StepLabel<TStepLabel> stepLabel)
        {
            return query
                .AddStep<TSource>("to", stepLabel);
        }

        public static IGremlinQuery<TElement> To<TElement>(this IGremlinQuery<TElement> query, Func<IGremlinQuery<TElement>, IGremlinQuery> toVertex)
        {
            return query
                .AddStep<TElement>("to", toVertex(query.ToAnonymous()));
        }

        public static IGremlinQuery<TElement> Unfold<TElement>(this IGremlinQuery<IEnumerable<TElement>> query)
        {
            return query
                .AddStep<TElement>("unfold");
        }

        public static IGremlinQuery<TTarget> Union<TSource, TTarget>(this IGremlinQuery<TSource> query, params Func<IGremlinQuery<TSource>, IGremlinQuery<TTarget>>[] unionTraversals)
        {
            return query
                .AddStep<TTarget>("union", unionTraversals
                    .Select(unionTraversal => unionTraversal(query.ToAnonymous()))
                    .ToImmutableList<object>());
        }

        public static IGremlinQuery<TElement> Until<TElement>(this IGremlinQuery<TElement> query, Func<IGremlinQuery<TElement>, IGremlinQuery> untilTraversal)
        {
            return query
                .AddStep<TElement>("until", untilTraversal(query.ToAnonymous()));
        }

        public static IGremlinQuery<Vertex> V(this IGremlinQuery query, params object[] ids)
        {
            return query
                .AddStep<Vertex>("V", ids);
        }

        public static IGremlinQuery<TElement> V<TElement>(this IGremlinQuery query, params object[] ids)
        {
            return query
                .V(ids)
                .OfType<TElement>();
        }

        public static IGremlinQuery<TTarget> Values<TSource, TTarget>(this IGremlinQuery<TSource> query, params Expression<Func<TSource, TTarget>>[] projections)
        {
            return query.AddStep<TTarget>(new ValuesGremlinStep<TSource, TTarget>(projections));
        }

        public static IGremlinQuery<TElement> Where<TElement>(this IGremlinQuery<TElement> query, Func<IGremlinQuery<TElement>, IGremlinQuery> filterTraversal)
        {
            return query
                .AddStep<TElement>("where", filterTraversal(query.ToAnonymous()));
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
                                        new OrP(Enumerable
                                            .Range(0, stringValue.Length + 1)
                                            .Select(i => P.Eq(stringValue.Substring(0, i)))
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
                                    var ultimateChar = upperBoundChars[upperBoundChars.Length - 1];

                                    if (ultimateChar < char.MaxValue)
                                    {
                                        upperBoundChars[upperBoundChars.Length - 1]++;

                                        return query.Has(memberExpression, P.Within(lowerBound, new string(upperBoundChars)));
                                    }
                                }
                            }
                        }
                    }

                    break;
                }
            }

            throw new NotSupportedException();
        }

        private static IGremlinQuery<TElement> Where<TElement>(this IGremlinQuery<TElement> query, ParameterExpression parameter, Expression left, Expression right, ExpressionType nodeType)
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

        private static IGremlinQuery<TElement> Where<TElement>(this IGremlinQuery<TElement> query, ParameterExpression parameter, Expression left, object rightConstant, ExpressionType nodeType)
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
                                ? query.ToAnonymous().AddStep<TElement>("where", predicateArgument)
                                : (object)predicateArgument);
                    }
                    case ParameterExpression leftParameterExpression when parameter == leftParameterExpression:
                    {
                        return query.AddStep<TElement>(
                            rightConstant is StepLabel
                                ? "where"
                                : "is",
                            predicateArgument);
                    }
                }
            }

            throw new NotSupportedException();
        }

        private static IGremlinQuery<TElement> Has<TElement>(this IGremlinQuery<TElement> query, Expression expression, Option<object> maybeArgument = default(Option<object>))
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

            return query.AddStep<TElement>(new HasStep(name, maybeArgument));
        }
    }
}