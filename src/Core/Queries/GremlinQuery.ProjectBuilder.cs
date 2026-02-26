// ReSharper disable ArrangeThisQualifier
using System.Collections.Immutable;
using System.Linq.Expressions;
using ExRam.Gremlinq.Core.Steps;

namespace ExRam.Gremlinq.Core
{
    internal partial class GremlinQuery<T1, T2, T3, T4>
    {
        private sealed class ProjectBuilder : IProjectBuilder<GremlinQuery<T1, T2, T3, T4>, T1>
        {
            private readonly bool _emptyProjectionProtection;
            private readonly GremlinQuery<T1, T2, T3, T4> _sourceQuery;

            public ProjectBuilder(GremlinQuery<T1, T2, T3, T4> sourceQuery) : this(
                sourceQuery,
                sourceQuery.Environment.Options.GetValue(GremlinqOption.EnableEmptyProjectionValueProtection))
            {
            }

            private ProjectBuilder(GremlinQuery<T1, T2, T3, T4> sourceQuery, bool emptyProjectionProtection)
            {
                _sourceQuery = sourceQuery;
                _emptyProjectionProtection = emptyProjectionProtection;
            }

            IProjectBuilder<GremlinQuery<T1, T2, T3, T4>, T1> IProjectBuilder<GremlinQuery<T1, T2, T3, T4>, T1>.WithEmptyProjectionProtection() => new ProjectBuilder(_sourceQuery, true);

            IProjectTupleBuilder<GremlinQuery<T1, T2, T3, T4>, T1> IProjectBuilder<GremlinQuery<T1, T2, T3, T4>, T1>.ToTuple() => Continue<object>();

            IProjectDynamicBuilder<GremlinQuery<T1, T2, T3, T4>, T1> IProjectBuilder<GremlinQuery<T1, T2, T3, T4>, T1>.ToDynamic() => Continue<object>();

            IProjectMapBuilder<GremlinQuery<T1, T2, T3, T4>, T1, TTargetType> IProjectBuilder<GremlinQuery<T1, T2, T3, T4>, T1>.To<TTargetType>() => Continue<TTargetType>();

            private ProjectBuilder<TItem1, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object> Continue<TItem1>()
                => new(
                    _sourceQuery,
                    FastImmutableList<string>.Empty,
                    FastImmutableList<ProjectStep.ByStep>.Empty,
                    _emptyProjectionProtection
                        ? _sourceQuery.Environment.Options.GetValue(GremlinqOption.EmptyProjectionProtectionDecoratorSteps)
                        : Traversal.Empty);
        }

        private sealed partial class ProjectBuilder<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, TItem15, TItem16> :
            IProjectDynamicBuilder<GremlinQuery<T1, T2, T3, T4>, T1>,
            IProjectMapBuilder<GremlinQuery<T1, T2, T3, T4>, T1, TItem1>
        {
            private readonly FastImmutableList<string> _names;
            private readonly GremlinQuery<T1, T2, T3, T4> _outer;
            private readonly FastImmutableList<ProjectStep.ByStep> _bySteps;
            private readonly Traversal _emptyProjectionProtectionDecoratorSteps;

            public ProjectBuilder(
                GremlinQuery<T1, T2, T3, T4> outer,
                FastImmutableList<string> names,
                FastImmutableList<ProjectStep.ByStep> bySteps,
                Traversal emptyProjectionProtectionDecoratorSteps)
            {
                _names = names;
                _outer = outer;
                _bySteps = bySteps;
                _emptyProjectionProtectionDecoratorSteps = emptyProjectionProtectionDecoratorSteps;
            }

            IProjectDynamicBuilder<GremlinQuery<T1, T2, T3, T4>, T1> IProjectDynamicBuilder<GremlinQuery<T1, T2, T3, T4>, T1>.By(Func<GremlinQuery<T1, T2, T3, T4>, IGremlinQueryBase> projection)
            {
                ArgumentNullException.ThrowIfNull(projection);

                return ByLambda<object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object>(projection);
            }

            IProjectDynamicBuilder<GremlinQuery<T1, T2, T3, T4>, T1> IProjectDynamicBuilder<GremlinQuery<T1, T2, T3, T4>, T1>.By(string name, Func<GremlinQuery<T1, T2, T3, T4>, IGremlinQueryBase> projection)
            {
                ArgumentNullException.ThrowIfNull(name);
                ArgumentNullException.ThrowIfNull(projection);

                return ByLambda<object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object>(projection, name);
            }

            IProjectDynamicBuilder<GremlinQuery<T1, T2, T3, T4>, T1> IProjectDynamicBuilder<GremlinQuery<T1, T2, T3, T4>, T1>.By(string name, Expression<Func<T1, object>> projection)
            {
                ArgumentNullException.ThrowIfNull(name);
                ArgumentNullException.ThrowIfNull(projection);

                return ByExpression<object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object>(projection, name);
            }

            IProjectDynamicBuilder<GremlinQuery<T1, T2, T3, T4>, T1> IProjectDynamicBuilder<GremlinQuery<T1, T2, T3, T4>, T1>.By(Expression<Func<T1, object>> projection)
            {
                ArgumentNullException.ThrowIfNull(projection);

                return projection.IsIdentityExpression()
                    ? ByLambda<object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object>(static __ => __.Identity())
                    : projection.Body.Strip() is MemberExpression memberExpression
                        ? ByExpression<object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object>(memberExpression, memberExpression.Member.Name)
                        : throw new ExpressionNotSupportedException(projection);
            }

            IMapGremlinQuery<TItem1> IProjectMapResult<TItem1>.Build() => Build<IMapGremlinQuery<TItem1>>();

            IGremlinQuery<dynamic> IProjectDynamicResult.Build() => Build<IGremlinQuery<dynamic>>();

            IProjectMapBuilder<GremlinQuery<T1, T2, T3, T4>, T1, TItem1> IProjectMapBuilder<GremlinQuery<T1, T2, T3, T4>, T1, TItem1>.By<TSourceProperty, TTargetProperty>(Expression<Func<TItem1, TTargetProperty>> targetExpression, Func<GremlinQuery<T1, T2, T3, T4>, IGremlinQueryBase<TSourceProperty>> projection)
            {
                ArgumentNullException.ThrowIfNull(targetExpression);
                ArgumentNullException.ThrowIfNull(projection);

                return By(
                    targetExpression,
                    static (@this, memberName, projection) => @this.ByLambda<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, TItem15, TItem16>(projection, memberName),
                    projection);
            }

            IProjectMapBuilder<GremlinQuery<T1, T2, T3, T4>, T1, TItem1> IProjectMapBuilder<GremlinQuery<T1, T2, T3, T4>, T1, TItem1>.By<TSourceProperty, TTargetProperty>(Expression<Func<TItem1, TTargetProperty>> targetExpression, Expression<Func<T1, TSourceProperty>> projection)
            {
                ArgumentNullException.ThrowIfNull(targetExpression);
                ArgumentNullException.ThrowIfNull(projection);

                return By(
                    targetExpression,
                    static (@this, memberName, projection) => @this.ByExpression<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, TItem15, TItem16>(projection, memberName),
                    projection);
            }

            private ProjectBuilder<TNewItem1, TNewItem2, TNewItem3, TNewItem4, TNewItem5, TNewItem6, TNewItem7, TNewItem8, TNewItem9, TNewItem10, TNewItem11, TNewItem12, TNewItem13, TNewItem14, TNewItem15, TNewItem16> ByLambda<TNewItem1, TNewItem2, TNewItem3, TNewItem4, TNewItem5, TNewItem6, TNewItem7, TNewItem8, TNewItem9, TNewItem10, TNewItem11, TNewItem12, TNewItem13, TNewItem14, TNewItem15, TNewItem16>(Func<GremlinQuery<T1, T2, T3, T4>, IGremlinQueryBase> projection, string? name = null) => new(
                _outer,
                _names
                    .Push(name ?? $"Item{_names.Count + 1}"),
                _bySteps
                    .Push(new ProjectStep.ByTraversalStep(_outer
                        .Continue()
                        .With(projection)
                        .Build(static (_, traversal) => traversal))),
                _emptyProjectionProtectionDecoratorSteps);

            private ProjectBuilder<TNewItem1, TNewItem2, TNewItem3, TNewItem4, TNewItem5, TNewItem6, TNewItem7, TNewItem8, TNewItem9, TNewItem10, TNewItem11, TNewItem12, TNewItem13, TNewItem14, TNewItem15, TNewItem16> ByExpression<TNewItem1, TNewItem2, TNewItem3, TNewItem4, TNewItem5, TNewItem6, TNewItem7, TNewItem8, TNewItem9, TNewItem10, TNewItem11, TNewItem12, TNewItem13, TNewItem14, TNewItem15, TNewItem16>(Expression projection, string? name = null) => projection is LambdaExpression lambdaExpression && lambdaExpression.IsIdentityExpression()
                ? ByLambda<TNewItem1, TNewItem2, TNewItem3, TNewItem4, TNewItem5, TNewItem6, TNewItem7, TNewItem8, TNewItem9, TNewItem10, TNewItem11, TNewItem12, TNewItem13, TNewItem14, TNewItem15, TNewItem16>(static __ => __.Identity(), name)
                : new(
                    _outer,
                    _names
                        .Push(name ?? $"Item{_names.Count + 1}"),
                    _bySteps
                        .Push(new ProjectStep.ByKeyStep(_outer.GetKey(projection))),
                    _emptyProjectionProtectionDecoratorSteps);

            private IProjectMapBuilder<GremlinQuery<T1, T2, T3, T4>, T1, TItem1> By<TTargetProperty, TState>(Expression<Func<TItem1, TTargetProperty>> targetExpression, Func<ProjectBuilder<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, TItem15, TItem16>, string, TState, ProjectBuilder<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, TItem15, TItem16>> transformation, TState state) => targetExpression.Body is MemberExpression memberExpression
                ? transformation(this, memberExpression.Member.Name, state)
                : throw new ExpressionNotSupportedException(targetExpression);

            private TTargetQuery Build<TTargetQuery>() where TTargetQuery : IGremlinQueryBase => _outer
                .Continue()
                .Build(
                    static (builder, state) =>
                    {
                        var (names, bySteps, emptyProjectionProtectionDecoratorSteps) = state;

                        var projectStep = new ProjectStep(names.AsSpan().ToImmutableArray());

                        builder = builder
                            .AddStep(projectStep)
                            .WithNewProjection(
                                static (projection, tuple) => projection.Project(tuple.projectStep, tuple.bySteps.AsSpan()),
                                (projectStep, bySteps));

                        for (var i = 0; i < bySteps.Count; i++)
                        {
                            var closureByStep = bySteps[i];

                            if (emptyProjectionProtectionDecoratorSteps.Count > 0)
                            {
                                var byTraversalStep = closureByStep
                                    .ToByTraversalStep();

                                closureByStep = new ProjectStep.ByTraversalStep(Traversal
                                    .Create(
                                        byTraversalStep.Traversal.Count + 2,
                                        byTraversalStep,
                                        static (steps, byTraversalStep) =>
                                        {
                                            steps[^2] = LimitStep.LimitGlobal1;
                                            steps[^1] = FoldStep.Instance;

                                            byTraversalStep.Traversal.Steps
                                                .CopyTo(steps);
                                        })
                                    .WithProjection(byTraversalStep.Traversal.Projection));
                            }

                            builder = builder
                                .AddStep(closureByStep);
                        }

                        if (emptyProjectionProtectionDecoratorSteps.Count > 0)
                        {
                            builder = builder
                                .AddSteps(emptyProjectionProtectionDecoratorSteps.Steps);
                        }

                        return builder
                            .BuildAs<TTargetQuery>();
                    },
                    (_names, _bySteps, _emptyProjectionProtectionDecoratorSteps));
        }
    }
}
