// ReSharper disable ArrangeThisQualifier
using ExRam.Gremlinq.Core.Projections;
using ExRam.Gremlinq.Core.Steps;

namespace ExRam.Gremlinq.Core
{
    internal partial class GremlinQuery<T1, T2, T3, T4>
    {
        private sealed class ChooseBuilder<TTargetQuery, TPickElement> :
            IChooseBuilder<GremlinQuery<T1, T2, T3, T4>>,
            IChooseBuilderWithCondition<GremlinQuery<T1, T2, T3, T4>, TPickElement>,
            IChooseBuilderWithCase<GremlinQuery<T1, T2, T3, T4>, TPickElement, TTargetQuery>
                where TTargetQuery : IGremlinQueryBase
        {
            private readonly Projection _projection;
            private readonly FastImmutableList<Step> _steps;
            private readonly GremlinQuery<T1, T2, T3, T4> _query;

            public ChooseBuilder(GremlinQuery<T1, T2, T3, T4> query) : this(query, FastImmutableList<Step>.Empty, query.Steps.Projection)
            {

            }

            private ChooseBuilder(GremlinQuery<T1, T2, T3, T4> query, FastImmutableList<Step> steps, Projection projection)
            {
                _steps = steps;
                _query = query;
                _projection = projection;
            }

            public IChooseBuilderWithCondition<GremlinQuery<T1, T2, T3, T4>, TNewPickElement> On<TNewPickElement>(Func<GremlinQuery<T1, T2, T3, T4>, IGremlinQueryBase<TNewPickElement>> chooseContinuation)
            {
                var traversal = _query
                    .Continue()
                    .With(chooseContinuation)
                    .Build(static (_, traversal) => traversal);

                return new ChooseBuilder<TTargetQuery, TNewPickElement>(
                    _query,
                    _steps
                        .Push(new ChooseOptionTraversalStep(traversal)),
                    _projection
                        .Lowest(traversal.Projection));
            }
             
            public IChooseBuilderWithCase<GremlinQuery<T1, T2, T3, T4>, TPickElement, TNewTargetQuery> Case<TNewTargetQuery>(TPickElement element, Func<GremlinQuery<T1, T2, T3, T4>, TNewTargetQuery> continuation) where TNewTargetQuery : IGremlinQueryBase
            {
                var traversal = _query
                    .Continue()
                    .With(continuation)
                    .Build(static (_, traversal) => traversal);

                return new ChooseBuilder<TNewTargetQuery, TPickElement>(
                    _query,
                    _steps
                        .Push(new OptionTraversalStep(element, traversal)),
                    _projection
                        .Lowest(traversal.Projection));
            }

            public IChooseBuilderWithCaseOrDefault<TNewTargetQuery> Default<TNewTargetQuery>(Func<GremlinQuery<T1, T2, T3, T4>, TNewTargetQuery> continuation) where TNewTargetQuery : IGremlinQueryBase
            {
                var traversal = _query
                    .Continue()
                    .With(continuation)
                    .Build(static (_, traversal) => traversal);

                return new ChooseBuilder<TNewTargetQuery, TPickElement>(
                    _query,
                    _steps
                        .Push(new OptionTraversalStep(null, traversal)),
                    _projection
                        .Lowest(traversal.Projection));
            }

            public IChooseBuilderWithCase<GremlinQuery<T1, T2, T3, T4>, TPickElement, TTargetQuery> Case(TPickElement element, Func<GremlinQuery<T1, T2, T3, T4>, TTargetQuery> continuation) => Case<TTargetQuery>(element, continuation);

            public IChooseBuilderWithCaseOrDefault<TTargetQuery> Default(Func<GremlinQuery<T1, T2, T3, T4>, TTargetQuery> continuation) => Default<TTargetQuery>(continuation);

            public TTargetQuery Build() => _query
                .Continue()
                .Build(
                    static (builder, tuple) => builder
                        .AddSteps(tuple._steps.AsSpan())
                        .WithNewProjection(tuple._projection)
                        .BuildAs<TTargetQuery>(),
                    (_steps, _projection));
        }
    }
}
