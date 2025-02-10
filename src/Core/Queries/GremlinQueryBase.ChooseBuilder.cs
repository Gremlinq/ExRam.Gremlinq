// ReSharper disable ArrangeThisQualifier
using ExRam.Gremlinq.Core.Steps;

namespace ExRam.Gremlinq.Core
{
    partial class GremlinQueryBase<T1, T2, T3, T4>
    {
        private sealed class ChooseBuilder<TTargetQuery, TPickElement> :
            IChooseBuilder<GremlinQueryBase<T1, T2, T3, T4>>,
            IChooseBuilderWithCondition<GremlinQueryBase<T1, T2, T3, T4>, TPickElement>,
            IChooseBuilderWithCase<GremlinQueryBase<T1, T2, T3, T4>, TPickElement, TTargetQuery>
            where TTargetQuery : IGremlinQueryBase
        {
            private readonly ContinuationBuilder<GremlinQueryBase<T1, T2, T3, T4>, GremlinQueryBase<T1, T2, T3, T4>> _continuation;

            public ChooseBuilder(GremlinQueryBase<T1, T2, T3, T4> query) : this(query.Continue(), query)
            {

            }

            private ChooseBuilder(ContinuationBuilder<GremlinQueryBase<T1, T2, T3, T4>, GremlinQueryBase<T1, T2, T3, T4>> continuation, GremlinQueryBase<T1, T2, T3, T4> targetQuery)
            {
                _continuation = continuation
                    .WithOuter(targetQuery);
            }

            public IChooseBuilderWithCondition<GremlinQueryBase<T1, T2, T3, T4>, TNewPickElement> On<TNewPickElement>(Func<GremlinQueryBase<T1, T2, T3, T4>, IGremlinQueryBase<TNewPickElement>> chooseContinuation)
            {
                return new ChooseBuilder<TTargetQuery, TNewPickElement>(
                    _continuation,
                    _continuation
                        .With(chooseContinuation)
                        .Build(static (builder, traversal) => builder
                            .AddStep(new ChooseOptionTraversalStep(traversal))));
            }

            public IChooseBuilderWithCase<GremlinQueryBase<T1, T2, T3, T4>, TPickElement, TNewTargetQuery> Case<TNewTargetQuery>(TPickElement element, Func<GremlinQueryBase<T1, T2, T3, T4>, TNewTargetQuery> continuation) where TNewTargetQuery : IGremlinQueryBase
            {
                return new ChooseBuilder<TNewTargetQuery, TPickElement>(
                    _continuation,
                    _continuation
                        .With(continuation)
                        .Build(
                            static (builder, traversal, element) => builder
                                .AddStep(new OptionTraversalStep(element, traversal))
                                .WithNewProjection(
                                    static (projection, otherProjection) => projection.Lowest(otherProjection),
                                    traversal.Projection),
                            element));
            }

            public IChooseBuilderWithCaseOrDefault<TNewTargetQuery> Default<TNewTargetQuery>(Func<GremlinQueryBase<T1, T2, T3, T4>, TNewTargetQuery> continuation) where TNewTargetQuery : IGremlinQueryBase
            {
                return new ChooseBuilder<TNewTargetQuery, TPickElement>(
                    _continuation,
                    _continuation
                        .With(continuation)
                        .Build(static (builder, traversal) => builder
                            .AddStep(new OptionTraversalStep(default, traversal))
                            .WithNewProjection(
                                static (projection, otherProjection) => projection.Lowest(otherProjection),
                                traversal.Projection)));
            }

            public IChooseBuilderWithCase<GremlinQueryBase<T1, T2, T3, T4>, TPickElement, TTargetQuery> Case(TPickElement element, Func<GremlinQueryBase<T1, T2, T3, T4>, TTargetQuery> continuation) => Case<TTargetQuery>(element, continuation);

            public IChooseBuilderWithCaseOrDefault<TTargetQuery> Default(Func<GremlinQueryBase<T1, T2, T3, T4>, TTargetQuery> continuation) => Default<TTargetQuery>(continuation);

            public TTargetQuery TargetQuery => _continuation.Build(static builder => builder.As<TTargetQuery>());
        }
    }
}
