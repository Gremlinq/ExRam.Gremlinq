using System;
using System.Linq.Expressions;

namespace ExRam.Gremlinq.Core
{
    public interface IValueTupleGremlinQueryBase :
        IGremlinQueryBase
    {
    }

    public interface IValueTupleGremlinQueryBase<TElement> :
        IValueTupleGremlinQueryBase,
        IGremlinQueryBase<TElement>
    {
        //IValueGremlinQuery<TTargetValue> Select<TTargetValue>(Expression<Func<TElement, TTargetValue>> projection);
    }

    public interface IValueTupleGremlinQuery<TElement> :
        IValueTupleGremlinQueryBase<TElement>,
        IGremlinQueryBaseRec<TElement, IValueTupleGremlinQuery<TElement>>
    {

    }
}
