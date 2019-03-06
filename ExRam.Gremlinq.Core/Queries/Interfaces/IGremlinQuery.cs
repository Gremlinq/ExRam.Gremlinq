using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ExRam.Gremlinq.Core
{
    public partial interface IGremlinQuery : IGremlinQuerySource
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

    public partial interface IGremlinQuery<TElement> : IGremlinQuery, IAsyncEnumerable<TElement>
    {
        IGremlinQuery<TElement> Inject(params TElement[] elements);

        ValueTask<TElement> FirstAsync();
        ValueTask<TElement> FirstAsync(CancellationToken ct);

        ValueTask<TElement> FirstOrDefaultAsync();
        ValueTask<TElement> FirstOrDefaultAsync(CancellationToken ct);
    }
}
