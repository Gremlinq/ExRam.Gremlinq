using System.Collections.Generic;

namespace ExRam.Gremlinq.Core
{
    public partial interface IGremlinQuery : IGremlinQueryBase
    {
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
        IGremlinQuery<TElement> Inject(params TElement[] elements);

        IAsyncEnumerable<TElement> ToAsyncEnumerable();
    }
}
