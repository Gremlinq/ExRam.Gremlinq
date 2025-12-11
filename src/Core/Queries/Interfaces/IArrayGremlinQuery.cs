namespace ExRam.Gremlinq.Core
{
    /// <summary>
    /// Represents a query for array results with operations to manipulate and aggregate array data.
    /// </summary>
    public interface IArrayGremlinQueryBase : IGremlinQueryBase
    {
        /// <summary>
        /// Unrolls (flattens) the array elements into individual elements.
        /// </summary>
        /// <returns>A query that returns the individual elements from the arrays.</returns>
        IGremlinQuery<object> Unfold();

        /// <summary>
        /// Sums the elements within each array locally.
        /// </summary>
        /// <returns>A query that returns the sum of each array's elements.</returns>
        IGremlinQuery<object> SumLocal();

        /// <summary>
        /// Finds the minimum element within each array locally.
        /// </summary>
        /// <returns>A query that returns the minimum element from each array.</returns>
        IGremlinQuery<object> MinLocal();

        /// <summary>
        /// Finds the maximum element within each array locally.
        /// </summary>
        /// <returns>A query that returns the maximum element from each array.</returns>
        IGremlinQuery<object> MaxLocal();

        /// <summary>
        /// Calculates the mean (average) of elements within each array locally.
        /// </summary>
        /// <returns>A query that returns the mean of each array's elements.</returns>
        IGremlinQuery<object> MeanLocal();

        /// <summary>
        /// Downcasts the query to a general query returning arrays.
        /// </summary>
        /// <returns>A query that returns arrays.</returns>
        new IGremlinQuery<object[]> Lower();
    }

    /// <summary>
    /// Represents a recursive array query with local operations.
    /// </summary>
    /// <typeparam name="TSelf">The concrete query type for method chaining.</typeparam>
    public interface IArrayGremlinQueryBaseRec<TSelf> : IArrayGremlinQueryBase, IGremlinQueryBaseRec<TSelf>
        where TSelf : IArrayGremlinQueryBaseRec<TSelf>
    {
        /// <summary>
        /// Limits the number of elements within each array locally.
        /// </summary>
        /// <param name="count">The maximum number of elements to keep in each array.</param>
        /// <returns>The query with locally limited arrays.</returns>
        TSelf LimitLocal(long count);

        /// <summary>
        /// Selects a range of elements within each array locally.
        /// </summary>
        /// <param name="low">The starting index (inclusive).</param>
        /// <param name="high">The ending index (exclusive).</param>
        /// <returns>The query with locally ranged arrays.</returns>
        TSelf RangeLocal(long low, long high);

        /// <summary>
        /// Skips a specified number of elements within each array locally.
        /// </summary>
        /// <param name="count">The number of elements to skip in each array.</param>
        /// <returns>The query with locally skipped elements.</returns>
        TSelf SkipLocal(long count);

        /// <summary>
        /// Takes the last specified number of elements within each array locally.
        /// </summary>
        /// <param name="count">The number of elements to take from the end of each array.</param>
        /// <returns>The query with locally tailed arrays.</returns>
        TSelf TailLocal(long count);
    }

    /// <summary>
    /// Represents a strongly-typed array query with operations for the array items.
    /// </summary>
    /// <typeparam name="TArrayItem">The type of items in the arrays.</typeparam>
    public interface IArrayGremlinQueryBase<TArrayItem> : IArrayGremlinQueryBase
    {
        /// <summary>
        /// Unrolls (flattens) the array elements into individual typed elements.
        /// </summary>
        /// <returns>A query that returns the individual typed elements from the arrays.</returns>
        new IGremlinQuery<TArrayItem> Unfold();

        /// <summary>
        /// Sums the elements within each array locally.
        /// </summary>
        /// <returns>A query that returns the typed sum of each array's elements.</returns>
        new IGremlinQuery<TArrayItem> SumLocal();

        /// <summary>
        /// Finds the minimum element within each array locally.
        /// </summary>
        /// <returns>A query that returns the typed minimum element from each array.</returns>
        new IGremlinQuery<TArrayItem> MinLocal();

        /// <summary>
        /// Finds the maximum element within each array locally.
        /// </summary>
        /// <returns>A query that returns the typed maximum element from each array.</returns>
        new IGremlinQuery<TArrayItem> MaxLocal();

        /// <summary>
        /// Calculates the mean (average) of elements within each array locally.
        /// </summary>
        /// <returns>A query that returns the typed mean of each array's elements.</returns>
        new IGremlinQuery<TArrayItem> MeanLocal();

        /// <summary>
        /// Downcasts the query to a general query returning typed arrays.
        /// </summary>
        /// <returns>A query that returns typed arrays.</returns>
        new IGremlinQuery<TArrayItem[]> Lower();
    }

    /// <summary>
    /// Represents a recursive strongly-typed array query.
    /// </summary>
    /// <typeparam name="TArrayItem">The type of items in the arrays.</typeparam>
    /// <typeparam name="TSelf">The concrete query type for method chaining.</typeparam>
    public interface IArrayGremlinQueryBaseRec<TArrayItem, TSelf> :
        IArrayGremlinQueryBase<TArrayItem>,
        IArrayGremlinQueryBaseRec<TSelf> where TSelf : IArrayGremlinQueryBaseRec<TArrayItem, TSelf>;

    /// <summary>
    /// Represents a strongly-typed array query with explicit array and item types.
    /// </summary>
    /// <typeparam name="TArray">The array type.</typeparam>
    /// <typeparam name="TArrayItem">The type of items in the arrays.</typeparam>
    public interface IArrayGremlinQueryBase<TArray, TArrayItem> :
        IArrayGremlinQueryBase<TArrayItem>,
        IGremlinQueryBase<TArray>
    {
        /// <summary>
        /// Downcasts the query to a general query returning the array type.
        /// </summary>
        /// <returns>A query that returns the array type.</returns>
        new IGremlinQuery<TArray> Lower();
    }

    /// <summary>
    /// Represents a recursive strongly-typed array query with explicit array and item types.
    /// </summary>
    /// <typeparam name="TArray">The array type.</typeparam>
    /// <typeparam name="TArrayItem">The type of items in the arrays.</typeparam>
    /// <typeparam name="TSelf">The concrete query type for method chaining.</typeparam>
    public interface IArrayGremlinQueryBaseRec<TArray, TArrayItem, TSelf> :
        IArrayGremlinQueryBase<TArray, TArrayItem>,
        IArrayGremlinQueryBaseRec<TArrayItem, TSelf>,
        IGremlinQueryBaseRec<TArray, TSelf> where TSelf : IArrayGremlinQueryBaseRec<TArray, TArrayItem, TSelf>;

    /// <summary>
    /// Represents a strongly-typed array query that can return to the original query type after operations.
    /// </summary>
    /// <typeparam name="TArray">The array type.</typeparam>
    /// <typeparam name="TArrayItem">The type of items in the arrays.</typeparam>
    /// <typeparam name="TOriginalQuery">The original query type to return to after array operations.</typeparam>
    public interface IArrayGremlinQueryBase<TArray, TArrayItem, out TOriginalQuery> :
        IArrayGremlinQueryBase<TArray, TArrayItem>
    {
        /// <summary>
        /// Sums the elements within each array locally and returns the original query type.
        /// </summary>
        /// <returns>The original query type with summed results.</returns>
        new TOriginalQuery SumLocal();

        /// <summary>
        /// Finds the minimum element within each array locally and returns the original query type.
        /// </summary>
        /// <returns>The original query type with minimum results.</returns>
        new TOriginalQuery MinLocal();

        /// <summary>
        /// Finds the maximum element within each array locally and returns the original query type.
        /// </summary>
        /// <returns>The original query type with maximum results.</returns>
        new TOriginalQuery MaxLocal();

        /// <summary>
        /// Calculates the mean of elements within each array locally and returns the original query type.
        /// </summary>
        /// <returns>The original query type with mean results.</returns>
        new TOriginalQuery MeanLocal();

        /// <summary>
        /// Unrolls the array elements and returns the original query type.
        /// </summary>
        /// <returns>The original query type with unfolded elements.</returns>
        new TOriginalQuery Unfold();
    }

    /// <summary>
    /// Represents a recursive strongly-typed array query that can return to the original query type.
    /// </summary>
    /// <typeparam name="TArray">The array type.</typeparam>
    /// <typeparam name="TArrayItem">The type of items in the arrays.</typeparam>
    /// <typeparam name="TOriginalQuery">The original query type to return to after array operations.</typeparam>
    /// <typeparam name="TSelf">The concrete query type for method chaining.</typeparam>
    public interface IArrayGremlinQueryBaseRec<TArray, TArrayItem, out TOriginalQuery, TSelf> :
        IArrayGremlinQueryBase<TArray, TArrayItem, TOriginalQuery>,
        IArrayGremlinQueryBaseRec<TArray, TArrayItem, TSelf>
            where TOriginalQuery : IGremlinQueryBase
            where TSelf : IArrayGremlinQueryBaseRec<TArray, TArrayItem,  TOriginalQuery, TSelf>;

    /// <summary>
    /// Represents a query for strongly-typed arrays with full array operations.
    /// </summary>
    /// <typeparam name="TArray">The array type.</typeparam>
    /// <typeparam name="TArrayItem">The type of items in the arrays.</typeparam>
    /// <typeparam name="TOriginalQuery">The original query type to return to after array operations.</typeparam>
    public interface IArrayGremlinQuery<TArray, TArrayItem, TOriginalQuery> :
        IArrayGremlinQueryBaseRec<TArray, TArrayItem, TOriginalQuery, IArrayGremlinQuery<TArray, TArrayItem, TOriginalQuery>>
            where TOriginalQuery : IGremlinQueryBase;
}
