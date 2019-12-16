using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ExRam.Gremlinq.Core
{
    public partial interface IGremlinQuery : IGremlinQueryBase
    {
        TaskAwaiter GetAwaiter();

        IGremlinQueryAdmin AsAdmin();
        IValueGremlinQuery<long> Count();
        IValueGremlinQuery<long> CountLocal();
        IValueGremlinQuery<TValue> Constant<TValue>(TValue constant);

        IGremlinQuery<string> Explain();

        IGremlinQuery<string> Profile();

        IGremlinQuery<TStepElement> Select<TStepElement>(StepLabel<TStepElement> label);
        TQuery Select<TQuery, TElement>(StepLabel<TQuery, TElement> label) where TQuery : IGremlinQuery;
    }

    public partial interface IGremlinQuery<TElement> : IGremlinQuery
    {
        new GremlinQueryAwaiter<TElement> GetAwaiter();

        IGremlinQuery<TElement> Inject(params TElement[] elements);

        IAsyncEnumerable<TElement> ToAsyncEnumerable();
    }
}
