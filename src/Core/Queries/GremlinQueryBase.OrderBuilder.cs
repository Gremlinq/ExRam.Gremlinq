// ReSharper disable ArrangeThisQualifier
using System.Linq.Expressions;
using ExRam.Gremlinq.Core.Steps;
using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core
{
    partial class GremlinQueryBase<T1, T2, T3, T4>
    {
        private sealed class OrderBuilder : IOrderBuilderWithBy<T1, GremlinQueryBase<T1, T2, T3, T4>>
        {
            private readonly GremlinQueryBase<T1, T2, T3, T4> _query;

            public OrderBuilder(GremlinQueryBase<T1, T2, T3, T4> query)
            {
                _query = query;
            }

            GremlinQueryBase<T1, T2, T3, T4> IOrderBuilderWithBy<GremlinQueryBase<T1, T2, T3, T4>>.Build() => _query;

            IOrderBuilderWithBy<T1, GremlinQueryBase<T1, T2, T3, T4>> IOrderBuilder<T1, GremlinQueryBase<T1, T2, T3, T4>>.By(Expression<Func<T1, object?>> projection) => By(projection, Gremlin.Net.Process.Traversal.Order.Asc);

            IOrderBuilderWithBy<T1, GremlinQueryBase<T1, T2, T3, T4>> IOrderBuilder<T1, GremlinQueryBase<T1, T2, T3, T4>>.By(Func<GremlinQueryBase<T1, T2, T3, T4>, IGremlinQueryBase> traversal) => By(traversal, Gremlin.Net.Process.Traversal.Order.Asc);

            IOrderBuilderWithBy<GremlinQueryBase<T1, T2, T3, T4>> IOrderBuilder<GremlinQueryBase<T1, T2, T3, T4>>.By(Func<GremlinQueryBase<T1, T2, T3, T4>, IGremlinQueryBase> traversal) => By(traversal, Gremlin.Net.Process.Traversal.Order.Asc);

            IOrderBuilderWithBy<T1, GremlinQueryBase<T1, T2, T3, T4>> IOrderBuilder<T1, GremlinQueryBase<T1, T2, T3, T4>>.ByDescending(Expression<Func<T1, object?>> projection) => By(projection, Gremlin.Net.Process.Traversal.Order.Desc);

            IOrderBuilderWithBy<T1, GremlinQueryBase<T1, T2, T3, T4>> IOrderBuilder<T1, GremlinQueryBase<T1, T2, T3, T4>>.ByDescending(Func<GremlinQueryBase<T1, T2, T3, T4>, IGremlinQueryBase> traversal) => By(traversal, Gremlin.Net.Process.Traversal.Order.Desc);

            IOrderBuilderWithBy<GremlinQueryBase<T1, T2, T3, T4>> IOrderBuilder<GremlinQueryBase<T1, T2, T3, T4>>.ByDescending(Func<GremlinQueryBase<T1, T2, T3, T4>, IGremlinQueryBase> traversal) => By(traversal, Gremlin.Net.Process.Traversal.Order.Desc);

            private OrderBuilder By(Expression<Func<T1, object?>> projection, Order order) => new(_query
                .Continue()
                .Build(
                    static (builder, tuple) =>
                    {
                        var (key, order) = tuple;

                        return builder
                            .AddStep(new OrderStep.ByMemberStep(key, order));
                    },
                    (_query.GetKey(projection), order)));

            private OrderBuilder By(Func<GremlinQueryBase<T1, T2, T3, T4>, IGremlinQueryBase> continuation, Order order) => new(_query
                .Continue()
                .With(continuation)
                .Build(
                    static (builder, traversal, order) => builder
                        .AddStep(new OrderStep.ByTraversalStep(traversal, order)),
                    order));
        }
    }
}
