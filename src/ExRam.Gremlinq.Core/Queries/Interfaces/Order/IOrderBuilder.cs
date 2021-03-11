using System;
using System.Linq.Expressions;
using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core
{
    public interface IOrderBuilder<out TSourceQuery>
        where TSourceQuery : IGremlinQueryBase
    {
        IOrderBuilderWithBy<TSourceQuery> By(ILambda lambda);

        IOrderBuilderWithBy<TSourceQuery> By(Func<TSourceQuery, IGremlinQueryBase> traversal);
        IOrderBuilderWithBy<TSourceQuery> ByDescending(Func<TSourceQuery, IGremlinQueryBase> traversal);
    }

    public interface IOrderBuilder<TElement, out TSourceQuery> :
        IOrderBuilder<TSourceQuery>
        where TSourceQuery : IGremlinQueryBase<TElement>
    {
        IOrderBuilderWithBy<TElement, TSourceQuery> By(Expression<Func<TElement, object?>> projection);
        IOrderBuilderWithBy<TElement, TSourceQuery> ByDescending(Expression<Func<TElement, object?>> projection);

        new IOrderBuilderWithBy<TElement, TSourceQuery> By(ILambda lambda);

        new IOrderBuilderWithBy<TElement, TSourceQuery> By(Func<TSourceQuery, IGremlinQueryBase> traversal);
        new IOrderBuilderWithBy<TElement, TSourceQuery> ByDescending(Func<TSourceQuery, IGremlinQueryBase> traversal);
    }

    public interface IOrderBuilderWithBy<out TSourceQuery> : IOrderBuilder<TSourceQuery>
        where TSourceQuery : IGremlinQueryBase
    {
        TSourceQuery Build();
    }

    public interface IOrderBuilderWithBy<TElement, out TSourceQuery> :
        IOrderBuilderWithBy<TSourceQuery>,
        IOrderBuilder<TElement, TSourceQuery>
        where TSourceQuery : IGremlinQueryBase<TElement>
    {
    }
}
