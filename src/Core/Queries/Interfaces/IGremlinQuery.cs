using System.Linq.Expressions;
using System.Runtime.CompilerServices;

using Path = ExRam.Gremlinq.Core.GraphElements.Path;

namespace ExRam.Gremlinq.Core
{
    /// <summary>
    /// Represents the base interface for all Gremlin queries with common operations.
    /// </summary>
    public interface IGremlinQueryBase : IStartGremlinQuery
    {
        /// <summary>
        /// Gets an awaiter that returns nothing when the query completes. This enables using await on queries.
        /// </summary>
        /// <returns>A task awaiter for the query execution.</returns>
        TaskAwaiter GetAwaiter();

        /// <summary>
        /// Converts the query results to strings.
        /// </summary>
        /// <returns>A string query.</returns>
        IStringGremlinQuery<string> AsString();

        /// <summary>
        /// Casts the query results to a different type.
        /// </summary>
        /// <typeparam name="TResult">The target type to cast to.</typeparam>
        /// <returns>A query returning the cast type.</returns>
        IGremlinQuery<TResult> Cast<TResult>();
        
        /// <summary>
        /// Counts the number of results in the traversal.
        /// </summary>
        /// <returns>A query that returns the count.</returns>
        IGremlinQuery<long> Count();
        
        /// <summary>
        /// Counts elements within each collection locally.
        /// </summary>
        /// <returns>A query that returns local counts.</returns>
        IGremlinQuery<long> CountLocal();
        
        /// <summary>
        /// Injects a constant value into the traversal.
        /// </summary>
        /// <typeparam name="TValue">The type of the constant value.</typeparam>
        /// <param name="constant">The constant value to inject.</param>
        /// <returns>A query that returns the constant value.</returns>
        IGremlinQuery<TValue> Constant<TValue>(TValue constant);

        /// <summary>
        /// Gets a string representation of the query's bytecode for debugging purposes.
        /// </summary>
        /// <returns>A string representation of the query bytecode.</returns>
        string Debug();

        /// <summary>
        /// Drops (deletes) all elements matched by the query.
        /// </summary>
        /// <returns>A query representing the drop operation.</returns>
        IGremlinQuery<object> Drop();

        /// <summary>
        /// Gets an explanation of how the query will be executed by the graph database.
        /// </summary>
        /// <returns>A query that returns the execution plan as a string.</returns>
        IGremlinQuery<string> Explain();

        /// <summary>
        /// Forces the query to fail with an optional message.
        /// </summary>
        /// <param name="message">The failure message.</param>
        /// <returns>A query that will fail when executed.</returns>
        IGremlinQuery<object> Fail(string? message = null);

        /// <summary>
        /// Downcasts the query to a less specific type.
        /// </summary>
        /// <returns>A query with the base type.</returns>
        IGremlinQuery<object> Lower();

        /// <summary>
        /// Gets the path taken by the traversal from the start to the current element.
        /// </summary>
        /// <returns>A query that returns traversal paths.</returns>
        IGremlinQuery<Path> Path();

        /// <summary>
        /// Profiles the query execution to gather performance metrics.
        /// </summary>
        /// <returns>A query that returns profiling information as a string.</returns>
        IGremlinQuery<string> Profile();

        /// <summary>
        /// Selects a previously labeled step from the traversal.
        /// </summary>
        /// <typeparam name="TStepElement">The type of the labeled step.</typeparam>
        /// <param name="label">The step label to select.</param>
        /// <returns>A query that returns the labeled step element.</returns>
        IGremlinQuery<TStepElement> Select<TStepElement>(StepLabel<TStepElement> label);
        
        /// <summary>
        /// Selects two previously labeled steps as a tuple.
        /// </summary>
        IMapGremlinQuery<(T1, T2)> Select<T1, T2>(StepLabel<T1> label1, StepLabel<T2> label2);
        
        /// <summary>
        /// Selects three previously labeled steps as a tuple.
        /// </summary>
        IMapGremlinQuery<(T1, T2, T3)> Select<T1, T2, T3>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3);
        
        /// <summary>
        /// Selects four previously labeled steps as a tuple.
        /// </summary>
        IMapGremlinQuery<(T1, T2, T3, T4)> Select<T1, T2, T3, T4>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3, StepLabel<T4> label4);
        
        /// <summary>
        /// Selects five previously labeled steps as a tuple.
        /// </summary>
        IMapGremlinQuery<(T1, T2, T3, T4, T5)> Select<T1, T2, T3, T4, T5>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3, StepLabel<T4> label4, StepLabel<T5> label5);
        
        /// <summary>
        /// Selects six previously labeled steps as a tuple.
        /// </summary>
        IMapGremlinQuery<(T1, T2, T3, T4, T5, T6)> Select<T1, T2, T3, T4, T5, T6>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3, StepLabel<T4> label4, StepLabel<T5> label5, StepLabel<T6> label6);
        
        /// <summary>
        /// Selects seven previously labeled steps as a tuple.
        /// </summary>
        IMapGremlinQuery<(T1, T2, T3, T4, T5, T6, T7)> Select<T1, T2, T3, T4, T5, T6, T7>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3, StepLabel<T4> label4, StepLabel<T5> label5, StepLabel<T6> label6, StepLabel<T7> label7);
        
        /// <summary>
        /// Selects eight previously labeled steps as a tuple.
        /// </summary>
        IMapGremlinQuery<(T1, T2, T3, T4, T5, T6, T7, T8)> Select<T1, T2, T3, T4, T5, T6, T7, T8>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3, StepLabel<T4> label4, StepLabel<T5> label5, StepLabel<T6> label6, StepLabel<T7> label7, StepLabel<T8> label8);
        
        /// <summary>
        /// Selects nine previously labeled steps as a tuple.
        /// </summary>
        IMapGremlinQuery<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3, StepLabel<T4> label4, StepLabel<T5> label5, StepLabel<T6> label6, StepLabel<T7> label7, StepLabel<T8> label8, StepLabel<T9> label9);
        
        /// <summary>
        /// Selects ten previously labeled steps as a tuple.
        /// </summary>
        IMapGremlinQuery<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3, StepLabel<T4> label4, StepLabel<T5> label5, StepLabel<T6> label6, StepLabel<T7> label7, StepLabel<T8> label8, StepLabel<T9> label9, StepLabel<T10> label10);
        
        /// <summary>
        /// Selects eleven previously labeled steps as a tuple.
        /// </summary>
        IMapGremlinQuery<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3, StepLabel<T4> label4, StepLabel<T5> label5, StepLabel<T6> label6, StepLabel<T7> label7, StepLabel<T8> label8, StepLabel<T9> label9, StepLabel<T10> label10, StepLabel<T11> label11);
        
        /// <summary>
        /// Selects twelve previously labeled steps as a tuple.
        /// </summary>
        IMapGremlinQuery<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3, StepLabel<T4> label4, StepLabel<T5> label5, StepLabel<T6> label6, StepLabel<T7> label7, StepLabel<T8> label8, StepLabel<T9> label9, StepLabel<T10> label10, StepLabel<T11> label11, StepLabel<T12> label12);
        
        /// <summary>
        /// Selects thirteen previously labeled steps as a tuple.
        /// </summary>
        IMapGremlinQuery<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13)> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3, StepLabel<T4> label4, StepLabel<T5> label5, StepLabel<T6> label6, StepLabel<T7> label7, StepLabel<T8> label8, StepLabel<T9> label9, StepLabel<T10> label10, StepLabel<T11> label11, StepLabel<T12> label12, StepLabel<T13> label13);
        
        /// <summary>
        /// Selects fourteen previously labeled steps as a tuple.
        /// </summary>
        IMapGremlinQuery<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14)> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3, StepLabel<T4> label4, StepLabel<T5> label5, StepLabel<T6> label6, StepLabel<T7> label7, StepLabel<T8> label8, StepLabel<T9> label9, StepLabel<T10> label10, StepLabel<T11> label11, StepLabel<T12> label12, StepLabel<T13> label13, StepLabel<T14> label14);
        
        /// <summary>
        /// Selects fifteen previously labeled steps as a tuple.
        /// </summary>
        IMapGremlinQuery<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15)> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3, StepLabel<T4> label4, StepLabel<T5> label5, StepLabel<T6> label6, StepLabel<T7> label7, StepLabel<T8> label8, StepLabel<T9> label9, StepLabel<T10> label10, StepLabel<T11> label11, StepLabel<T12> label12, StepLabel<T13> label13, StepLabel<T14> label14, StepLabel<T15> label15);
        
        /// <summary>
        /// Selects sixteen previously labeled steps as a tuple.
        /// </summary>
        IMapGremlinQuery<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16)> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3, StepLabel<T4> label4, StepLabel<T5> label5, StepLabel<T6> label6, StepLabel<T7> label7, StepLabel<T8> label8, StepLabel<T9> label9, StepLabel<T10> label10, StepLabel<T11> label11, StepLabel<T12> label12, StepLabel<T13> label13, StepLabel<T14> label14, StepLabel<T15> label15, StepLabel<T16> label16);

        /// <summary>
        /// Selects a previously labeled query step and returns it with its original query type.
        /// </summary>
        /// <typeparam name="TQuery">The type of the labeled query.</typeparam>
        /// <typeparam name="TElement">The element type of the labeled query.</typeparam>
        /// <param name="label">The query step label to select.</param>
        /// <returns>The labeled query with its original type.</returns>
        TQuery Select<TQuery, TElement>(StepLabel<TQuery, TElement> label) where TQuery : IGremlinQueryBase;

        /// <summary>
        /// Creates a tree structure from the traversal results.
        /// </summary>
        /// <returns>A query that returns a tree with object nodes.</returns>
        IGremlinQuery<Tree<object>> Tree();

        /// <summary>
        /// Creates a typed tree structure from the traversal results.
        /// </summary>
        /// <typeparam name="TNode">The type of nodes in the tree.</typeparam>
        /// <returns>A query that returns a tree with typed nodes.</returns>
        IGremlinQuery<Tree<TNode>> Tree<TNode>() where TNode : notnull;

        /// <summary>
        /// Creates a custom tree structure using a tree builder.
        /// </summary>
        /// <typeparam name="TTree">The type of tree to create.</typeparam>
        /// <param name="continuation">A function that configures the tree builder.</param>
        /// <returns>A query that returns the configured tree structure.</returns>
        IGremlinQuery<TTree> Tree<TTree>(Func<ITreeBuilder, ITreeBuilderResult<TTree>> continuation) where TTree : ITree;

        /// <summary>
        /// Captures the side effect of a previous labeled array aggregation step.
        /// </summary>
        /// <typeparam name="TElement">The array element type.</typeparam>
        /// <typeparam name="TArrayItem">The type of items in the array.</typeparam>
        /// <typeparam name="TOriginalQuery">The original query type.</typeparam>
        /// <param name="label">The step label of the aggregation to capture.</param>
        /// <returns>An array query containing the captured aggregated elements.</returns>
        IArrayGremlinQuery<TElement, TArrayItem, TOriginalQuery> Cap<TElement, TArrayItem, TOriginalQuery>(StepLabel<IArrayGremlinQuery<TElement, TArrayItem, TOriginalQuery>, TElement> label) where TOriginalQuery : IGremlinQueryBase;
    }

    /// <summary>
    /// Represents a strongly-typed base interface for Gremlin queries.
    /// </summary>
    /// <typeparam name="TElement">The type of elements returned by the query.</typeparam>
    public interface IGremlinQueryBase<TElement> : IGremlinQueryBase
    {
        /// <summary>
        /// Gets an awaiter that returns an array of results when the query completes. This enables using await on queries.
        /// </summary>
        /// <returns>A task awaiter that returns the query results as an array.</returns>
        new TaskAwaiter<TElement[]> GetAwaiter();

        /// <summary>
        /// Formats elements as strings using string interpolation.
        /// </summary>
        /// <param name="stringInterpolationExpression">An interpolated string expression defining the format.</param>
        /// <returns>A string query with formatted results.</returns>
        IStringGremlinQuery<string> Format(Expression<Func<TElement, string>> stringInterpolationExpression);

        /// <summary>
        /// Groups elements by themselves with their duplicates.
        /// </summary>
        /// <returns>A map query that returns dictionaries where keys are elements and values are arrays of duplicate elements.</returns>
        IMapGremlinQuery<IDictionary<TElement, TElement[]>> Group();

        /// <summary>
        /// Forces the query to be treated as a base query with no specific element type semantics.
        /// </summary>
        /// <returns>A base query for the element type.</returns>
        IGremlinQuery<TElement> ForceBase();
        
        /// <summary>
        /// Forces the query to be treated as an edge query.
        /// </summary>
        /// <returns>An edge query for the element type.</returns>
        IEdgeGremlinQuery<TElement> ForceEdge();
        
        /// <summary>
        /// Forces the query to be treated as a value query.
        /// </summary>
        /// <returns>A value query for the element type.</returns>
        IGremlinQuery<TElement> ForceValue();
        
        /// <summary>
        /// Forces the query to be treated as a vertex query.
        /// </summary>
        /// <returns>A vertex query for the element type.</returns>
        IVertexGremlinQuery<TElement> ForceVertex();
        
        /// <summary>
        /// Forces the query to be treated as an element query.
        /// </summary>
        /// <returns>An element query for the element type.</returns>
        IElementGremlinQuery<TElement> ForceElement();
        
        /// <summary>
        /// Forces the query to be treated as a property query.
        /// </summary>
        /// <returns>A property query for the element type.</returns>
        IPropertyGremlinQuery<TElement> ForceProperty();
        
        /// <summary>
        /// Forces the query to be treated as a value tuple query.
        /// </summary>
        /// <returns>A map query for the element type.</returns>
        IMapGremlinQuery<TElement> ForceValueTuple();
        
        /// <summary>
        /// Forces the query to be treated as an incoming edge query with a specific incoming vertex type.
        /// </summary>
        /// <typeparam name="TInVertex">The type of the incoming vertex.</typeparam>
        /// <returns>An incoming edge query.</returns>
        IInEdgeGremlinQuery<TElement, TInVertex> ForceInEdge<TInVertex>();
        
        /// <summary>
        /// Forces the query to be treated as an outgoing edge query with a specific outgoing vertex type.
        /// </summary>
        /// <typeparam name="TOutVertex">The type of the outgoing vertex.</typeparam>
        /// <returns>An outgoing edge query.</returns>
        IOutEdgeGremlinQuery<TElement, TOutVertex> ForceOutEdge<TOutVertex>();
        
        /// <summary>
        /// Forces the query to be treated as a vertex property query with a specific value type.
        /// </summary>
        /// <typeparam name="TValue">The type of the property value.</typeparam>
        /// <returns>A vertex property query.</returns>
        IVertexPropertyGremlinQuery<TElement, TValue> ForceVertexProperty<TValue>();
        
        /// <summary>
        /// Forces the query to be treated as an array query.
        /// </summary>
        /// <returns>An array query for the element type.</returns>
        IArrayGremlinQuery<TElement[], TElement, IGremlinQuery<TElement>> ForceArray();
        
        /// <summary>
        /// Forces the query to be treated as a vertex property query with specific value and metadata types.
        /// </summary>
        /// <typeparam name="TValue">The type of the property value.</typeparam>
        /// <typeparam name="TMeta">The type of the property metadata.</typeparam>
        /// <returns>A vertex property query with metadata.</returns>
        IVertexPropertyGremlinQuery<TElement, TValue, TMeta> ForceVertexProperty<TValue, TMeta>();
        
        /// <summary>
        /// Forces the query to be treated as an edge query with specific outgoing and incoming vertex types.
        /// </summary>
        /// <typeparam name="TOutVertex">The type of the outgoing vertex.</typeparam>
        /// <typeparam name="TInVertex">The type of the incoming vertex.</typeparam>
        /// <returns>An edge query with vertex type information.</returns>
        IEdgeGremlinQuery<TElement, TOutVertex, TInVertex> ForceEdge<TOutVertex, TInVertex>();

        /// <summary>
        /// Downcasts the query to a less specific typed query.
        /// </summary>
        /// <returns>A query for the element type.</returns>
        new IGremlinQuery<TElement> Lower();

        /// <summary>
        /// Converts the query results to an asynchronous enumerable for streaming results.
        /// </summary>
        /// <returns>An asynchronous enumerable of query results.</returns>
        IAsyncEnumerable<TElement> ToAsyncEnumerable();
    }

    /// <summary>
    /// Represents a recursive Gremlin query interface that supports method chaining with self-returning methods.
    /// </summary>
    /// <typeparam name="TSelf">The concrete query type for method chaining.</typeparam>
    public interface IGremlinQueryBaseRec<TSelf> : IGremlinQueryBase
        where TSelf : IGremlinQueryBaseRec<TSelf>
    {
        /// <summary>
        /// Filters the traversal by ensuring all the specified sub-traversals return results (logical AND).
        /// </summary>
        /// <param name="andTraversals">Sub-traversals that must all match.</param>
        /// <returns>The query with the AND filter applied.</returns>
        TSelf And(params Func<TSelf, IGremlinQueryBase>[] andTraversals);
        
        /// <summary>
        /// Filters the traversal by ensuring all the specified sub-traversals return results (logical AND).
        /// </summary>
        /// <param name="andTraversals">Sub-traversals that must all match.</param>
        /// <returns>The query with the AND filter applied.</returns>
        TSelf And(params ReadOnlySpan<Func<TSelf, IGremlinQueryBase>> andTraversals);

        /// <summary>
        /// Randomly filters elements with the specified probability.
        /// </summary>
        /// <param name="probability">The probability (0.0 to 1.0) that an element passes the filter.</param>
        /// <returns>The query with probabilistic filtering.</returns>
        TSelf Coin(double probability);

        /// <summary>
        /// Creates a synchronization point in the traversal, forcing all prior steps to complete before continuing.
        /// </summary>
        /// <returns>The query with a barrier.</returns>
        TSelf Barrier();

        /// <summary>
        /// Implements conditional branching based on a traversal predicate.
        /// </summary>
        /// <typeparam name="TTargetQuery">The target query type for both branches.</typeparam>
        /// <param name="traversalPredicate">A traversal that determines which branch to take.</param>
        /// <param name="trueChoice">The traversal to execute if the predicate succeeds.</param>
        /// <param name="falseChoice">The traversal to execute if the predicate fails.</param>
        /// <returns>The target query from the chosen branch.</returns>
        TTargetQuery Choose<TTargetQuery>(Func<TSelf, IGremlinQueryBase> traversalPredicate, Func<TSelf, TTargetQuery> trueChoice, Func<TSelf, TTargetQuery> falseChoice) where TTargetQuery : IGremlinQueryBase;
        
        /// <summary>
        /// Implements conditional branching based on a traversal predicate with only a true branch.
        /// </summary>
        /// <param name="traversalPredicate">A traversal that determines whether to execute the true branch.</param>
        /// <param name="trueChoice">The traversal to execute if the predicate succeeds.</param>
        /// <returns>The query with conditional logic applied.</returns>
        TSelf Choose(Func<TSelf, IGremlinQueryBase> traversalPredicate, Func<TSelf, TSelf> trueChoice);
        
        /// <summary>
        /// Implements conditional branching based on a traversal predicate with only a true branch.
        /// </summary>
        /// <param name="traversalPredicate">A traversal that determines whether to execute the true branch.</param>
        /// <param name="trueChoice">The traversal to execute if the predicate succeeds.</param>
        /// <returns>A query with conditional logic applied.</returns>
        IGremlinQuery<object> Choose(Func<TSelf, IGremlinQueryBase> traversalPredicate, Func<TSelf, IGremlinQueryBase> trueChoice);

        /// <summary>
        /// Implements switch/case logic using a builder pattern.
        /// </summary>
        /// <typeparam name="TTargetQuery">The target query type.</typeparam>
        /// <param name="continuation">A function that configures the choose builder.</param>
        /// <returns>The target query from the chosen case.</returns>
        TTargetQuery Choose<TTargetQuery>(Func<IChooseBuilder<TSelf>, IChooseBuilderWithCaseOrDefault<TTargetQuery>> continuation) where TTargetQuery : IGremlinQueryBase;

        /// <summary>
        /// Evaluates traversals in order and returns the first non-empty result (like SQL COALESCE).
        /// </summary>
        /// <typeparam name="TTargetQuery">The target query type.</typeparam>
        /// <param name="traversals">Traversals to evaluate in order.</param>
        /// <returns>The first non-empty result.</returns>
        TTargetQuery Coalesce<TTargetQuery>(params Func<TSelf, TTargetQuery>[] traversals) where TTargetQuery : IGremlinQueryBase;
        
        /// <summary>
        /// Evaluates traversals in order and returns the first non-empty result (like SQL COALESCE).
        /// </summary>
        /// <typeparam name="TTargetQuery">The target query type.</typeparam>
        /// <param name="traversals">Traversals to evaluate in order.</param>
        /// <returns>The first non-empty result.</returns>
        TTargetQuery Coalesce<TTargetQuery>(params ReadOnlySpan<Func<TSelf, TTargetQuery>> traversals) where TTargetQuery : IGremlinQueryBase;

        /// <summary>
        /// Evaluates traversals in order and returns the first non-empty result.
        /// </summary>
        /// <param name="traversals">Traversals to evaluate in order.</param>
        /// <returns>The first non-empty result.</returns>
        IGremlinQuery<object> Coalesce(params Func<TSelf, IGremlinQueryBase>[] traversals);
        
        /// <summary>
        /// Evaluates traversals in order and returns the first non-empty result.
        /// </summary>
        /// <param name="traversals">Traversals to evaluate in order.</param>
        /// <returns>The first non-empty result.</returns>
        IGremlinQuery<object> Coalesce(params ReadOnlySpan<Func<TSelf, IGremlinQueryBase>> traversals);

        /// <summary>
        /// Filters out elements that have already been traversed in a cyclic path.
        /// </summary>
        /// <returns>The query filtering only cyclic paths.</returns>
        TSelf CyclicPath();

        /// <summary>
        /// Removes duplicate elements from the traversal.
        /// </summary>
        /// <returns>The query with duplicates removed.</returns>
        TSelf Dedup();
        
        /// <summary>
        /// Removes duplicate elements within each collection locally.
        /// </summary>
        /// <returns>The query with local deduplication.</returns>
        TSelf DedupLocal();

        /// <summary>
        /// Maps elements to a new traversal and flattens the results.
        /// </summary>
        /// <typeparam name="TTargetQuery">The target query type.</typeparam>
        /// <param name="mapping">A function that maps elements to a new traversal.</param>
        /// <returns>The flattened result of the mapping.</returns>
        TTargetQuery FlatMap<TTargetQuery>(Func<TSelf, TTargetQuery> mapping) where TTargetQuery : IGremlinQueryBase;

        /// <summary>
        /// Returns the elements unchanged (identity function).
        /// </summary>
        /// <returns>The same query.</returns>
        TSelf Identity();

        /// <summary>
        /// Limits the number of results to the specified count.
        /// </summary>
        /// <param name="count">The maximum number of results to return.</param>
        /// <returns>The query with the limit applied.</returns>
        TSelf Limit(long count);
        
        /// <summary>
        /// Executes a traversal locally within each element's scope.
        /// </summary>
        /// <typeparam name="TTargetQuery">The target query type.</typeparam>
        /// <param name="localTraversal">The traversal to execute locally.</param>
        /// <returns>The result of the local traversal.</returns>
        TTargetQuery Local<TTargetQuery>(Func<TSelf, TTargetQuery> localTraversal) where TTargetQuery : IGremlinQueryBase;

        /// <summary>
        /// Maps elements to a new traversal.
        /// </summary>
        /// <typeparam name="TTargetQuery">The target query type.</typeparam>
        /// <param name="mapping">A function that maps elements to a new traversal.</param>
        /// <returns>The result of the mapping.</returns>
        TTargetQuery Map<TTargetQuery>(Func<TSelf, TTargetQuery> mapping) where TTargetQuery : IGremlinQueryBase;

        /// <summary>
        /// Finds the maximum value in the traversal.
        /// </summary>
        /// <returns>The query with the maximum value.</returns>
        TSelf Max();

        /// <summary>
        /// Calculates the mean (average) value in the traversal.
        /// </summary>
        /// <returns>The query with the mean value.</returns>
        TSelf Mean();

        /// <summary>
        /// Finds the minimum value in the traversal.
        /// </summary>
        /// <returns>The query with the minimum value.</returns>
        TSelf Min();

        /// <summary>
        /// Filters elements by negating a sub-traversal (logical NOT).
        /// </summary>
        /// <param name="notTraversal">A sub-traversal to negate.</param>
        /// <returns>The query with the NOT filter applied.</returns>
        TSelf Not(Func<TSelf, IGremlinQueryBase> notTraversal);
        
        /// <summary>
        /// Filters to return no elements (always empty result).
        /// </summary>
        /// <returns>An empty query.</returns>
        TSelf None();

        /// <summary>
        /// Executes an optional traversal that may or may not yield results.
        /// </summary>
        /// <param name="optionalTraversal">The optional traversal to attempt.</param>
        /// <returns>The query with optional traversal applied.</returns>
        TSelf Optional(Func<TSelf, TSelf> optionalTraversal);

        /// <summary>
        /// Filters the traversal by ensuring at least one of the specified sub-traversals returns results (logical OR).
        /// </summary>
        /// <param name="orTraversals">Sub-traversals where at least one must match.</param>
        /// <returns>The query with the OR filter applied.</returns>
        TSelf Or(params Func<TSelf, IGremlinQueryBase>[] orTraversals);
        
        /// <summary>
        /// Filters the traversal by ensuring at least one of the specified sub-traversals returns results (logical OR).
        /// </summary>
        /// <param name="orTraversals">Sub-traversals where at least one must match.</param>
        /// <returns>The query with the OR filter applied.</returns>
        TSelf Or(params ReadOnlySpan<Func<TSelf, IGremlinQueryBase>> orTraversals);

        /// <summary>
        /// Orders the results using an order builder.
        /// </summary>
        /// <param name="projection">A function that configures the order builder.</param>
        /// <returns>The query with ordering applied.</returns>
        TSelf Order(Func<IOrderBuilder<TSelf>, IOrderBuilderWithBy<TSelf>> projection);
        
        /// <summary>
        /// Orders elements within each collection locally.
        /// </summary>
        /// <param name="projection">A function that configures the order builder.</param>
        /// <returns>The query with local ordering applied.</returns>
        TSelf OrderLocal(Func<IOrderBuilder<TSelf>, IOrderBuilderWithBy<TSelf>> projection);

        /// <summary>
        /// Returns a range of results between the specified indices.
        /// </summary>
        /// <param name="low">The starting index (inclusive).</param>
        /// <param name="high">The ending index (exclusive).</param>
        /// <returns>The query with the range applied.</returns>
        TSelf Range(long low, long high);

        /// <summary>
        /// Creates a loop (repeat/until/emit) using a builder pattern.
        /// </summary>
        /// <param name="loopBuilderTransformation">A function that configures the loop builder.</param>
        /// <returns>The query with the loop applied.</returns>
        TSelf Loop(Func<IStartLoopBuilder<TSelf>, IFinalLoopBuilder<TSelf>> loopBuilderTransformation);

        /// <summary>
        /// Executes a side-effect traversal without modifying the main traversal results.
        /// </summary>
        /// <param name="sideEffectTraversal">The side-effect traversal to execute.</param>
        /// <returns>The query with the side effect registered.</returns>
        TSelf SideEffect(Func<TSelf, IGremlinQueryBase> sideEffectTraversal);

        /// <summary>
        /// Filters to only include elements that have been traversed in a simple (non-repeating) path.
        /// </summary>
        /// <returns>The query filtering only simple paths.</returns>
        TSelf SimplePath();

        /// <summary>
        /// Skips the specified number of results.
        /// </summary>
        /// <param name="count">The number of results to skip.</param>
        /// <returns>The query with skipped results.</returns>
        TSelf Skip(long count);

        /// <summary>
        /// Sums all values in the traversal.
        /// </summary>
        /// <returns>The query with the sum value.</returns>
        TSelf Sum();

        /// <summary>
        /// Takes the last specified number of results.
        /// </summary>
        /// <param name="count">The number of results to take from the end.</param>
        /// <returns>The query with the tail applied.</returns>
        TSelf Tail(long count);

        /// <summary>
        /// Merges the results of multiple traversals into a single stream (union).
        /// </summary>
        /// <typeparam name="TTargetQuery">The target query type.</typeparam>
        /// <param name="unionTraversals">Traversals to union.</param>
        /// <returns>The unified query results.</returns>
        TTargetQuery Union<TTargetQuery>(params Func<TSelf, TTargetQuery>[] unionTraversals) where TTargetQuery : IGremlinQueryBase;
        
        /// <summary>
        /// Merges the results of multiple traversals into a single stream (union).
        /// </summary>
        /// <typeparam name="TTargetQuery">The target query type.</typeparam>
        /// <param name="unionTraversals">Traversals to union.</param>
        /// <returns>The unified query results.</returns>
        TTargetQuery Union<TTargetQuery>(params ReadOnlySpan<Func<TSelf, TTargetQuery>> unionTraversals) where TTargetQuery : IGremlinQueryBase;

        /// <summary>
        /// Merges the results of multiple traversals into a single stream.
        /// </summary>
        /// <param name="traversals">Traversals to union.</param>
        /// <returns>The unified query results.</returns>
        IGremlinQuery<object> Union(params Func<TSelf, IGremlinQueryBase>[] traversals);
        
        /// <summary>
        /// Merges the results of multiple traversals into a single stream.
        /// </summary>
        /// <param name="traversals">Traversals to union.</param>
        /// <returns>The unified query results.</returns>
        IGremlinQuery<object> Union(params ReadOnlySpan<Func<TSelf, IGremlinQueryBase>> traversals);

        /// <summary>
        /// Filters elements using a traversal that must yield results.
        /// </summary>
        /// <param name="filterTraversal">A traversal that defines the filter condition.</param>
        /// <returns>The query with the filter applied.</returns>
        TSelf Where(Func<TSelf, IGremlinQueryBase> filterTraversal);
    }

    /// <summary>
    /// Represents a strongly-typed recursive Gremlin query interface with element-specific operations.
    /// </summary>
    /// <typeparam name="TElement">The type of elements returned by the query.</typeparam>
    /// <typeparam name="TSelf">The concrete query type for method chaining.</typeparam>
    public interface IGremlinQueryBaseRec<TElement, TSelf> :
        IGremlinQueryBaseRec<TSelf>,
        IGremlinQueryBase<TElement>
        where TSelf : IGremlinQueryBaseRec<TElement, TSelf>
    {
        /// <summary>
        /// Aggregates elements into a side-effect collection and continues with a new traversal.
        /// </summary>
        /// <typeparam name="TTargetQuery">The target query type.</typeparam>
        /// <param name="continuation">A function that receives the aggregation step label and returns a new traversal.</param>
        /// <returns>The result of the continuation.</returns>
        TTargetQuery Aggregate<TTargetQuery>(Func<TSelf, StepLabel<IArrayGremlinQuery<TElement[], TElement, TSelf>, TElement[]>, TTargetQuery> continuation) where TTargetQuery : IGremlinQueryBase;
        
        /// <summary>
        /// Aggregates elements locally within each collection and continues with a new traversal.
        /// </summary>
        /// <typeparam name="TTargetQuery">The target query type.</typeparam>
        /// <param name="continuation">A function that receives the aggregation step label and returns a new traversal.</param>
        /// <returns>The result of the continuation.</returns>
        TTargetQuery AggregateLocal<TTargetQuery>(Func<TSelf, StepLabel<IArrayGremlinQuery<TElement[], TElement, TSelf>, TElement[]>, TTargetQuery> continuation) where TTargetQuery : IGremlinQueryBase;

        /// <summary>
        /// Aggregates elements into a side-effect collection with the specified label.
        /// </summary>
        /// <param name="stepLabel">The label to assign to the aggregated collection.</param>
        /// <returns>The query with aggregation applied.</returns>
        TSelf Aggregate(StepLabel<TElement[]> stepLabel);
        
        /// <summary>
        /// Aggregates elements locally into a side-effect collection with the specified label.
        /// </summary>
        /// <param name="stepLabel">The label to assign to the aggregated collection.</param>
        /// <returns>The query with local aggregation applied.</returns>
        TSelf AggregateLocal(StepLabel<TElement[]> stepLabel);

        /// <summary>
        /// Labels the current step for later reference.
        /// </summary>
        /// <param name="stepLabel">The label to assign to this step.</param>
        /// <returns>The query with the label applied.</returns>
        TSelf As(StepLabel<TElement> stepLabel);
        
        /// <summary>
        /// Labels the current step and continues with a new traversal that can reference it.
        /// </summary>
        /// <typeparam name="TTargetQuery">The target query type.</typeparam>
        /// <param name="continuation">A function that receives the step label and returns a new traversal.</param>
        /// <returns>The result of the continuation.</returns>
        TTargetQuery As<TTargetQuery>(Func<TSelf, StepLabel<TSelf, TElement>, TTargetQuery> continuation) where TTargetQuery : IGremlinQueryBase;

        /// <summary>
        /// Implements conditional branching based on an element predicate.
        /// </summary>
        /// <typeparam name="TTargetQuery">The target query type for both branches.</typeparam>
        /// <param name="predicate">A predicate expression that determines which branch to take.</param>
        /// <param name="trueChoice">The traversal to execute if the predicate is true.</param>
        /// <param name="falseChoice">The traversal to execute if the predicate is false.</param>
        /// <returns>The target query from the chosen branch.</returns>
        TTargetQuery Choose<TTargetQuery>(Expression<Func<TElement, bool>> predicate, Func<TSelf, TTargetQuery> trueChoice, Func<TSelf, TTargetQuery> falseChoice) where TTargetQuery : IGremlinQueryBase;
        
        /// <summary>
        /// Implements conditional branching based on an element predicate with only a true branch.
        /// </summary>
        /// <param name="predicate">A predicate expression that determines whether to execute the true branch.</param>
        /// <param name="trueChoice">The traversal to execute if the predicate is true.</param>
        /// <returns>The query with conditional logic applied.</returns>
        TSelf Choose(Expression<Func<TElement, bool>> predicate, Func<TSelf, TSelf> trueChoice);
        
        /// <summary>
        /// Implements conditional branching based on an element predicate with only a true branch.
        /// </summary>
        /// <param name="predicate">A predicate expression that determines whether to execute the true branch.</param>
        /// <param name="trueChoice">The traversal to execute if the predicate is true.</param>
        /// <returns>A query with conditional logic applied.</returns>
        IGremlinQuery<object> Choose(Expression<Func<TElement, bool>> predicate, Func<TSelf, IGremlinQueryBase> trueChoice);

        /// <summary>
        /// Collects all elements into an array.
        /// </summary>
        /// <returns>An array query containing all collected elements.</returns>
        IArrayGremlinQuery<TElement[], TElement, TSelf> Fold();

        /// <summary>
        /// Forces the query to be treated as an array query.
        /// </summary>
        /// <returns>An array query for the elements.</returns>
        new IArrayGremlinQuery<TElement[], TElement, TSelf> ForceArray();

        /// <summary>
        /// Groups elements using a group builder with custom key and value selectors.
        /// </summary>
        /// <typeparam name="TNewKey">The type of the grouping key.</typeparam>
        /// <typeparam name="TNewValue">The type of the grouped values.</typeparam>
        /// <param name="groupBuilder">A function that configures the group builder.</param>
        /// <returns>A map query that returns dictionaries with grouped results.</returns>
        IMapGremlinQuery<IDictionary<TNewKey, TNewValue>> Group<TNewKey, TNewValue>(Func<IGroupBuilder<TSelf>, IGroupBuilderWithKeyAndValue<TNewKey, TNewValue>> groupBuilder);
        
        /// <summary>
        /// Groups elements using a group builder with a custom key selector.
        /// </summary>
        /// <typeparam name="TNewKey">The type of the grouping key.</typeparam>
        /// <param name="groupBuilder">A function that configures the group builder.</param>
        /// <returns>A map query that returns dictionaries where keys are custom and values are element arrays.</returns>
        IMapGremlinQuery<IDictionary<TNewKey, TElement[]>> Group<TNewKey>(Func<IGroupBuilder<TSelf>, IGroupBuilderWithKey<TSelf, TNewKey>> groupBuilder);

        /// <summary>
        /// Injects additional elements into the traversal.
        /// </summary>
        /// <param name="elements">Elements to inject.</param>
        /// <returns>The query with injected elements.</returns>
        TSelf Inject(params TElement[] elements);
        
        /// <summary>
        /// Injects additional elements into the traversal.
        /// </summary>
        /// <param name="elements">Elements to inject.</param>
        /// <returns>The query with injected elements.</returns>
        TSelf Inject(params ReadOnlySpan<TElement> elements);

        /// <summary>
        /// Projects elements to dynamic objects using a project builder.
        /// </summary>
        /// <param name="continuation">A function that configures the project builder.</param>
        /// <returns>A query that returns dynamic objects with projected properties.</returns>
        IGremlinQuery<dynamic> Project(Func<IProjectBuilder<TSelf, TElement>, IProjectDynamicResult> continuation);
        
        /// <summary>
        /// Projects elements to strongly-typed objects using a project builder.
        /// </summary>
        /// <typeparam name="TResult">The target projection type.</typeparam>
        /// <param name="continuation">A function that configures the project builder.</param>
        /// <returns>A map query that returns objects of the target type.</returns>
        IMapGremlinQuery<TResult> Project<TResult>(Func<IProjectBuilder<TSelf, TElement>, IProjectMapResult<TResult>> continuation);
        
        /// <summary>
        /// Projects elements to tuples using a project builder.
        /// </summary>
        /// <typeparam name="TResult">The target tuple type.</typeparam>
        /// <param name="continuation">A function that configures the project builder.</param>
        /// <returns>A map query that returns tuples.</returns>
        IMapGremlinQuery<TResult> Project<TResult>(Func<IProjectBuilder<TSelf, TElement>, IProjectTupleResult<TResult>> continuation) where TResult : ITuple;

        /// <summary>
        /// Orders the results using an order builder with element-specific projections.
        /// </summary>
        /// <param name="projection">A function that configures the order builder.</param>
        /// <returns>The query with ordering applied.</returns>
        TSelf Order(Func<IOrderBuilder<TElement, TSelf>, IOrderBuilderWithBy<TElement, TSelf>> projection);
        
        /// <summary>
        /// Orders elements within each collection locally using an order builder.
        /// </summary>
        /// <param name="projection">A function that configures the order builder.</param>
        /// <returns>The query with local ordering applied.</returns>
        TSelf OrderLocal(Func<IOrderBuilder<TElement, TSelf>, IOrderBuilderWithBy<TElement, TSelf>> projection);

        /// <summary>
        /// Filters elements using a predicate expression.
        /// </summary>
        /// <param name="predicate">A predicate expression that defines the filter condition.</param>
        /// <returns>The query with the filter applied.</returns>
        TSelf Where(Expression<Func<TElement, bool>> predicate);
    }

    /// <summary>
    /// Represents a concrete Gremlin query for strongly-typed elements.
    /// </summary>
    /// <typeparam name="TElement">The type of elements returned by the query.</typeparam>
    public interface IGremlinQuery<TElement> : IGremlinQueryBaseRec<TElement, IGremlinQuery<TElement>>;
}
