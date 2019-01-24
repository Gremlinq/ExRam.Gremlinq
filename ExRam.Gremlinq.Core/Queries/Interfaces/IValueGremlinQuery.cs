using System;
using System.Linq.Expressions;

namespace ExRam.Gremlinq.Core
{
    public partial interface IValueGremlinQuery<TElement> : IGremlinQuery<TElement>
    {
        TTargetQuery Choose<TTargetQuery>(Expression<Func<TElement, bool>> predicate, Func<IValueGremlinQuery<TElement>, TTargetQuery> trueChoice, Func<IValueGremlinQuery<TElement>, TTargetQuery> falseChoice) where TTargetQuery : IGremlinQuery;
        TTargetQuery Choose<TTargetQuery>(Expression<Func<TElement, bool>> predicate, Func<IValueGremlinQuery<TElement>, TTargetQuery> trueChoice) where TTargetQuery : IGremlinQuery;

        new IValueGremlinQuery<TElement> Inject(params TElement[] elements);

        IValueGremlinQuery<TElement> SumLocal();
        IValueGremlinQuery<TElement> SumGlobal();

        IValueGremlinQuery<TElement> Where(Expression<Func<TElement, bool>> predicate);
    }
}
