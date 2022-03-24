using System;
using System.Linq.Expressions;

namespace ExRam.Gremlinq.Core
{
    public interface IProjectResult
    {
        TTargetQuery Build<TTargetQuery>() where TTargetQuery : IGremlinQueryBase;
    }

    // ReSharper disable once UnusedTypeParameter
    public interface IProjectResult<TResult> : IProjectResult
    {
    }

    public interface IProjectBuilder<out TSourceQuery, TElement>
        where TSourceQuery : IGremlinQueryBase
    {
        IProjectTupleBuilder<TSourceQuery, TElement> ToTuple();
        IProjectDynamicBuilder<TSourceQuery, TElement> ToDynamic();
    }

    public interface IProjectDynamicBuilder<out TSourceQuery, TElement> : IProjectResult
        where TSourceQuery : IGremlinQueryBase
    {
        IProjectDynamicBuilder<TSourceQuery, TElement> By(Func<TSourceQuery, IGremlinQueryBase> projection);
        IProjectDynamicBuilder<TSourceQuery, TElement> By(string name, Func<TSourceQuery, IGremlinQueryBase> projection);
        IProjectDynamicBuilder<TSourceQuery, TElement> By(string name, Expression<Func<TElement, object>> projection);
        IProjectDynamicBuilder<TSourceQuery, TElement> By(Expression<Func<TElement, object>> projection);
    }
}
