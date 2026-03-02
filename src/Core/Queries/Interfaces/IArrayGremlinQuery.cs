namespace ExRam.Gremlinq.Core
{
    public interface IArrayGremlinQueryBase : IGremlinQueryBase
    {
        /// <summary>
        /// Unrolls a list/array result into individual traverser elements.
        /// Corresponds to the Gremlin <c>unfold()</c> step.
        /// </summary>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#unfold-step">Reference Documentation - Unfold Step</seealso>
        IGremlinQuery<object> Unfold();

        /// <summary>
        /// Compute the sum of items within each local list/array.
        /// Corresponds to the Gremlin <c>sum(local)</c> step.
        /// </summary>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#sum-step">Reference Documentation - Sum Step</seealso>
        IGremlinQuery<object> SumLocal();

        /// <summary>
        /// Compute the minimum of items within each local list/array.
        /// Corresponds to the Gremlin <c>min(local)</c> step.
        /// </summary>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#min-step">Reference Documentation - Min Step</seealso>
        IGremlinQuery<object> MinLocal();

        /// <summary>
        /// Compute the maximum of items within each local list/array.
        /// Corresponds to the Gremlin <c>max(local)</c> step.
        /// </summary>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#max-step">Reference Documentation - Max Step</seealso>
        IGremlinQuery<object> MaxLocal();

        /// <summary>
        /// Compute the mean of items within each local list/array.
        /// Corresponds to the Gremlin <c>mean(local)</c> step.
        /// </summary>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#mean-step">Reference Documentation - Mean Step</seealso>
        IGremlinQuery<object> MeanLocal();

        /// <inheritdoc cref="IGremlinQueryBase.Lower" />
        new IGremlinQuery<object[]> Lower();
    }

    public interface IArrayGremlinQueryBaseRec<TSelf> : IArrayGremlinQueryBase, IGremlinQueryBaseRec<TSelf>
        where TSelf : IArrayGremlinQueryBaseRec<TSelf>
    {
        /// <summary>
        /// Limit the number of items within each local list/array.
        /// Corresponds to the Gremlin <c>limit(local)</c> step.
        /// </summary>
        /// <param name="count">The maximum number of items.</param>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#limit-step">Reference Documentation - Limit Step</seealso>
        TSelf LimitLocal(long count);

        /// <summary>
        /// Get a range of items within each local list/array.
        /// Corresponds to the Gremlin <c>range(local)</c> step.
        /// </summary>
        /// <param name="low">The inclusive low bound of the range.</param>
        /// <param name="high">The exclusive high bound of the range.</param>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#range-step">Reference Documentation - Range Step</seealso>
        TSelf RangeLocal(long low, long high);

        /// <summary>
        /// Skip items within each local list/array.
        /// Corresponds to the Gremlin <c>skip(local)</c> step.
        /// </summary>
        /// <param name="count">The number of items to skip.</param>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#skip-step">Reference Documentation - Skip Step</seealso>
        TSelf SkipLocal(long count);

        /// <summary>
        /// Get the last items within each local list/array.
        /// Corresponds to the Gremlin <c>tail(local)</c> step.
        /// </summary>
        /// <param name="count">The number of items to keep from the end.</param>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#tail-step">Reference Documentation - Tail Step</seealso>
        TSelf TailLocal(long count);
    }

    public interface IArrayGremlinQueryBase<TArrayItem> : IArrayGremlinQueryBase
    {
        /// <inheritdoc cref="IArrayGremlinQueryBase.Unfold" />
        new IGremlinQuery<TArrayItem> Unfold();

        /// <inheritdoc cref="IArrayGremlinQueryBase.SumLocal" />
        new IGremlinQuery<TArrayItem> SumLocal();

        /// <inheritdoc cref="IArrayGremlinQueryBase.MinLocal" />
        new IGremlinQuery<TArrayItem> MinLocal();

        /// <inheritdoc cref="IArrayGremlinQueryBase.MaxLocal" />
        new IGremlinQuery<TArrayItem> MaxLocal();

        /// <inheritdoc cref="IArrayGremlinQueryBase.MeanLocal" />
        new IGremlinQuery<TArrayItem> MeanLocal();

        /// <inheritdoc cref="IGremlinQueryBase.Lower" />
        new IGremlinQuery<TArrayItem[]> Lower();
    }

    public interface IArrayGremlinQueryBaseRec<TArrayItem, TSelf> :
        IArrayGremlinQueryBase<TArrayItem>,
        IArrayGremlinQueryBaseRec<TSelf> where TSelf : IArrayGremlinQueryBaseRec<TArrayItem, TSelf>;

    public interface IArrayGremlinQueryBase<TArray, TArrayItem> :
        IArrayGremlinQueryBase<TArrayItem>,
        IGremlinQueryBase<TArray>
    {
        /// <inheritdoc cref="IGremlinQueryBase.Lower" />
        new IGremlinQuery<TArray> Lower();
    }

    public interface IArrayGremlinQueryBaseRec<TArray, TArrayItem, TSelf> :
        IArrayGremlinQueryBase<TArray, TArrayItem>,
        IArrayGremlinQueryBaseRec<TArrayItem, TSelf>,
        IGremlinQueryBaseRec<TArray, TSelf> where TSelf : IArrayGremlinQueryBaseRec<TArray, TArrayItem, TSelf>;

    public interface IArrayGremlinQueryBase<TArray, TArrayItem, out TOriginalQuery> :
        IArrayGremlinQueryBase<TArray, TArrayItem>
    {
        /// <inheritdoc cref="IArrayGremlinQueryBase.SumLocal" />
        new TOriginalQuery SumLocal();

        /// <inheritdoc cref="IArrayGremlinQueryBase.MinLocal" />
        new TOriginalQuery MinLocal();

        /// <inheritdoc cref="IArrayGremlinQueryBase.MaxLocal" />
        new TOriginalQuery MaxLocal();

        /// <inheritdoc cref="IArrayGremlinQueryBase.MeanLocal" />
        new TOriginalQuery MeanLocal();

        /// <inheritdoc cref="IArrayGremlinQueryBase.Unfold" />
        new TOriginalQuery Unfold();
    }

    public interface IArrayGremlinQueryBaseRec<TArray, TArrayItem, out TOriginalQuery, TSelf> :
        IArrayGremlinQueryBase<TArray, TArrayItem, TOriginalQuery>,
        IArrayGremlinQueryBaseRec<TArray, TArrayItem, TSelf>
            where TOriginalQuery : IGremlinQueryBase
            where TSelf : IArrayGremlinQueryBaseRec<TArray, TArrayItem,  TOriginalQuery, TSelf>;

    public interface IArrayGremlinQuery<TArray, TArrayItem, TOriginalQuery> :
        IArrayGremlinQueryBaseRec<TArray, TArrayItem, TOriginalQuery, IArrayGremlinQuery<TArray, TArrayItem, TOriginalQuery>>
            where TOriginalQuery : IGremlinQueryBase;
}
