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

        IGremlinQuery<(T1, T2)> Select<T1, T2>(StepLabel<T1> label1, StepLabel<T2> label2);
        IGremlinQuery<(T1, T2, T3)> Select<T1, T2, T3>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3);
        IGremlinQuery<(T1, T2, T3, T4)> Select<T1, T2, T3, T4>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3, StepLabel<T4> label4);
    }

    public partial interface IGremlinQuery<TElement> : IGremlinQuery, IAsyncEnumerable<TElement>
    {
        IGremlinQuery<TElement> Inject(params TElement[] elements);

        Task<TElement> First();
        Task<TElement> First(CancellationToken ct);

        Task<TElement> FirstOrDefault();
        Task<TElement> FirstOrDefault(CancellationToken ct);
    }
}
