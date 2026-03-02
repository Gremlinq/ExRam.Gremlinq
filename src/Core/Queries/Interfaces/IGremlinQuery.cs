using System.Linq.Expressions;
using System.Runtime.CompilerServices;

using Path = ExRam.Gremlinq.Core.GraphElements.Path;

namespace ExRam.Gremlinq.Core
{
    public interface IGremlinQueryBase : IStartGremlinQuery
    {
        /// <summary>
        /// Get an awaiter for the query to enable <c>await</c> syntax.
        /// </summary>
        TaskAwaiter GetAwaiter();

        /// <summary>
        /// Treat the current traversal elements as strings.
        /// </summary>
        IStringGremlinQuery<string> AsString();

        /// <summary>
        /// Treat the current traversal elements as dates.
        /// </summary>
        IDateGremlinQuery<DateTimeOffset> AsDate();

        /// <summary>
        /// Cast the traversal element type to <typeparamref name="TResult"/>.
        /// </summary>
        /// <typeparam name="TResult">The target element type.</typeparam>
        IGremlinQuery<TResult> Cast<TResult>();

        /// <summary>
        /// Count the number of traversers in the stream.
        /// Corresponds to the Gremlin <c>count()</c> step.
        /// </summary>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#count-step">Reference Documentation - Count Step</seealso>
        IGremlinQuery<long> Count();

        /// <summary>
        /// Count the number of items within each local collection.
        /// Corresponds to the Gremlin <c>count(local)</c> step.
        /// </summary>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#count-step">Reference Documentation - Count Step</seealso>
        IGremlinQuery<long> CountLocal();

        /// <summary>
        /// Inject a constant value into the traversal.
        /// Corresponds to the Gremlin <c>constant()</c> step.
        /// </summary>
        /// <typeparam name="TValue">The type of the constant value.</typeparam>
        /// <param name="constant">The constant value to inject.</param>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#constant-step">Reference Documentation - Constant Step</seealso>
        IGremlinQuery<TValue> Constant<TValue>(TValue constant);

        /// <summary>
        /// Return a string representation of the query for debugging purposes.
        /// </summary>
        string Debug();

        /// <summary>
        /// Remove all traversed elements from the graph.
        /// Corresponds to the Gremlin <c>drop()</c> step.
        /// </summary>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#drop-step">Reference Documentation - Drop Step</seealso>
        IGremlinQuery<object> Drop();

        /// <summary>
        /// Return an explanation of the traversal execution plan.
        /// Corresponds to the Gremlin <c>explain()</c> step.
        /// </summary>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#explain-step">Reference Documentation - Explain Step</seealso>
        IGremlinQuery<string> Explain();

        /// <summary>
        /// Force the traversal to fail with an optional message.
        /// Corresponds to the Gremlin <c>fail()</c> step.
        /// </summary>
        /// <param name="message">An optional failure message.</param>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#fail-step">Reference Documentation - Fail Step</seealso>
        IGremlinQuery<object> Fail(string? message = null);

        /// <summary>
        /// Return a lower-typed version of the current query, discarding specific type information.
        /// </summary>
        IGremlinQuery<object> Lower();

        /// <summary>
        /// Map the traversers to their path information.
        /// Corresponds to the Gremlin <c>path()</c> step.
        /// </summary>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#path-step">Reference Documentation - Path Step</seealso>
        IGremlinQuery<Path> Path();

        /// <summary>
        /// Return a profiling result of the traversal execution.
        /// Corresponds to the Gremlin <c>profile()</c> step.
        /// </summary>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#profile-step">Reference Documentation - Profile Step</seealso>
        IGremlinQuery<string> Profile();

        /// <summary>
        /// Select a previously labeled step by its step label.
        /// Corresponds to the Gremlin <c>select()</c> step.
        /// </summary>
        /// <typeparam name="TStepElement">The type of the labeled element.</typeparam>
        /// <param name="label">The step label to select.</param>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#select-step">Reference Documentation - Select Step</seealso>
        IGremlinQuery<TStepElement> Select<TStepElement>(StepLabel<TStepElement> label);

        /// <summary>
        /// Select multiple previously labeled steps as a tuple.
        /// Corresponds to the Gremlin <c>select()</c> step with multiple labels.
        /// </summary>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#select-step">Reference Documentation - Select Step</seealso>
        IMapGremlinQuery<(T1, T2)> Select<T1, T2>(StepLabel<T1> label1, StepLabel<T2> label2);

        /// <inheritdoc cref="Select{T1, T2}(StepLabel{T1}, StepLabel{T2})" />
        IMapGremlinQuery<(T1, T2, T3)> Select<T1, T2, T3>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3);

        /// <inheritdoc cref="Select{T1, T2}(StepLabel{T1}, StepLabel{T2})" />
        IMapGremlinQuery<(T1, T2, T3, T4)> Select<T1, T2, T3, T4>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3, StepLabel<T4> label4);

        /// <inheritdoc cref="Select{T1, T2}(StepLabel{T1}, StepLabel{T2})" />
        IMapGremlinQuery<(T1, T2, T3, T4, T5)> Select<T1, T2, T3, T4, T5>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3, StepLabel<T4> label4, StepLabel<T5> label5);

        /// <inheritdoc cref="Select{T1, T2}(StepLabel{T1}, StepLabel{T2})" />
        IMapGremlinQuery<(T1, T2, T3, T4, T5, T6)> Select<T1, T2, T3, T4, T5, T6>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3, StepLabel<T4> label4, StepLabel<T5> label5, StepLabel<T6> label6);

        /// <inheritdoc cref="Select{T1, T2}(StepLabel{T1}, StepLabel{T2})" />
        IMapGremlinQuery<(T1, T2, T3, T4, T5, T6, T7)> Select<T1, T2, T3, T4, T5, T6, T7>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3, StepLabel<T4> label4, StepLabel<T5> label5, StepLabel<T6> label6, StepLabel<T7> label7);

        /// <inheritdoc cref="Select{T1, T2}(StepLabel{T1}, StepLabel{T2})" />
        IMapGremlinQuery<(T1, T2, T3, T4, T5, T6, T7, T8)> Select<T1, T2, T3, T4, T5, T6, T7, T8>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3, StepLabel<T4> label4, StepLabel<T5> label5, StepLabel<T6> label6, StepLabel<T7> label7, StepLabel<T8> label8);

        /// <inheritdoc cref="Select{T1, T2}(StepLabel{T1}, StepLabel{T2})" />
        IMapGremlinQuery<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3, StepLabel<T4> label4, StepLabel<T5> label5, StepLabel<T6> label6, StepLabel<T7> label7, StepLabel<T8> label8, StepLabel<T9> label9);

        /// <inheritdoc cref="Select{T1, T2}(StepLabel{T1}, StepLabel{T2})" />
        IMapGremlinQuery<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3, StepLabel<T4> label4, StepLabel<T5> label5, StepLabel<T6> label6, StepLabel<T7> label7, StepLabel<T8> label8, StepLabel<T9> label9, StepLabel<T10> label10);

        /// <inheritdoc cref="Select{T1, T2}(StepLabel{T1}, StepLabel{T2})" />
        IMapGremlinQuery<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3, StepLabel<T4> label4, StepLabel<T5> label5, StepLabel<T6> label6, StepLabel<T7> label7, StepLabel<T8> label8, StepLabel<T9> label9, StepLabel<T10> label10, StepLabel<T11> label11);

        /// <inheritdoc cref="Select{T1, T2}(StepLabel{T1}, StepLabel{T2})" />
        IMapGremlinQuery<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3, StepLabel<T4> label4, StepLabel<T5> label5, StepLabel<T6> label6, StepLabel<T7> label7, StepLabel<T8> label8, StepLabel<T9> label9, StepLabel<T10> label10, StepLabel<T11> label11, StepLabel<T12> label12);

        /// <inheritdoc cref="Select{T1, T2}(StepLabel{T1}, StepLabel{T2})" />
        IMapGremlinQuery<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13)> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3, StepLabel<T4> label4, StepLabel<T5> label5, StepLabel<T6> label6, StepLabel<T7> label7, StepLabel<T8> label8, StepLabel<T9> label9, StepLabel<T10> label10, StepLabel<T11> label11, StepLabel<T12> label12, StepLabel<T13> label13);

        /// <inheritdoc cref="Select{T1, T2}(StepLabel{T1}, StepLabel{T2})" />
        IMapGremlinQuery<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14)> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3, StepLabel<T4> label4, StepLabel<T5> label5, StepLabel<T6> label6, StepLabel<T7> label7, StepLabel<T8> label8, StepLabel<T9> label9, StepLabel<T10> label10, StepLabel<T11> label11, StepLabel<T12> label12, StepLabel<T13> label13, StepLabel<T14> label14);

        /// <inheritdoc cref="Select{T1, T2}(StepLabel{T1}, StepLabel{T2})" />
        IMapGremlinQuery<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15)> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3, StepLabel<T4> label4, StepLabel<T5> label5, StepLabel<T6> label6, StepLabel<T7> label7, StepLabel<T8> label8, StepLabel<T9> label9, StepLabel<T10> label10, StepLabel<T11> label11, StepLabel<T12> label12, StepLabel<T13> label13, StepLabel<T14> label14, StepLabel<T15> label15);

        /// <inheritdoc cref="Select{T1, T2}(StepLabel{T1}, StepLabel{T2})" />
        IMapGremlinQuery<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16)> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3, StepLabel<T4> label4, StepLabel<T5> label5, StepLabel<T6> label6, StepLabel<T7> label7, StepLabel<T8> label8, StepLabel<T9> label9, StepLabel<T10> label10, StepLabel<T11> label11, StepLabel<T12> label12, StepLabel<T13> label13, StepLabel<T14> label14, StepLabel<T15> label15, StepLabel<T16> label16);

        /// <summary>
        /// Select a previously labeled step, returning it as a strongly-typed query.
        /// Corresponds to the Gremlin <c>select()</c> step.
        /// </summary>
        /// <typeparam name="TQuery">The query type associated with the step label.</typeparam>
        /// <typeparam name="TElement">The element type of the step label.</typeparam>
        /// <param name="label">The step label to select.</param>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#select-step">Reference Documentation - Select Step</seealso>
        TQuery Select<TQuery, TElement>(StepLabel<TQuery, TElement> label) where TQuery : IGremlinQueryBase;

        /// <summary>
        /// Collect all paths through a traversal as tree structures.
        /// Corresponds to the Gremlin <c>tree()</c> step.
        /// </summary>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#tree-step">Reference Documentation - Tree Step</seealso>
        IGremlinQuery<Tree<object>> Tree();

        /// <inheritdoc cref="Tree()" />
        IGremlinQuery<Tree<TNode>> Tree<TNode>() where TNode : notnull;

        /// <inheritdoc cref="Tree()" />
        IGremlinQuery<TTree> Tree<TTree>(Func<ITreeBuilder, ITreeBuilderResult<TTree>> continuation) where TTree : ITree;

        /// <summary>
        /// Retrieve the contents of a side-effect by its step label.
        /// Corresponds to the Gremlin <c>cap()</c> step.
        /// </summary>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#cap-step">Reference Documentation - Cap Step</seealso>
        IArrayGremlinQuery<TElement, TArrayItem, TOriginalQuery> Cap<TElement, TArrayItem, TOriginalQuery>(StepLabel<IArrayGremlinQuery<TElement, TArrayItem, TOriginalQuery>, TElement> label) where TOriginalQuery : IGremlinQueryBase;
    }

    public interface IGremlinQueryBase<TElement> : IGremlinQueryBase
    {
        /// <summary>
        /// Get a typed awaiter for the query to enable <c>await</c> syntax that returns an array of <typeparamref name="TElement"/>.
        /// </summary>
        new TaskAwaiter<TElement[]> GetAwaiter();

        /// <summary>
        /// Format element properties into a string using an interpolation expression.
        /// Corresponds to the Gremlin <c>format()</c> step.
        /// </summary>
        /// <param name="stringInterpolationExpression">An expression that produces the formatted string.</param>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#format-step">Reference Documentation - Format Step</seealso>
        IStringGremlinQuery<string> Format(Expression<Func<TElement, string>> stringInterpolationExpression);

        /// <summary>
        /// Group traversal elements into a dictionary keyed by the element itself with arrays of elements as values.
        /// Corresponds to the Gremlin <c>group()</c> step.
        /// </summary>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#group-step">Reference Documentation - Group Step</seealso>
        IMapGremlinQuery<IDictionary<TElement, TElement[]>> Group();

        /// <summary>
        /// Force the query to be treated as a base <see cref="IGremlinQuery{TElement}"/>.
        /// </summary>
        IGremlinQuery<TElement> ForceBase();

        /// <summary>
        /// Force the query to be treated as an <see cref="IEdgeGremlinQuery{TElement}"/>.
        /// </summary>
        IEdgeGremlinQuery<TElement> ForceEdge();

        /// <summary>
        /// Force the query to be treated as a value <see cref="IGremlinQuery{TElement}"/>.
        /// </summary>
        IGremlinQuery<TElement> ForceValue();

        /// <summary>
        /// Force the query to be treated as an <see cref="IVertexGremlinQuery{TElement}"/>.
        /// </summary>
        IVertexGremlinQuery<TElement> ForceVertex();

        /// <summary>
        /// Force the query to be treated as an <see cref="IElementGremlinQuery{TElement}"/>.
        /// </summary>
        IElementGremlinQuery<TElement> ForceElement();

        /// <summary>
        /// Force the query to be treated as an <see cref="IPropertyGremlinQuery{TElement}"/>.
        /// </summary>
        IPropertyGremlinQuery<TElement> ForceProperty();

        /// <summary>
        /// Force the query to be treated as an <see cref="IMapGremlinQuery{TElement}"/>.
        /// </summary>
        IMapGremlinQuery<TElement> ForceValueTuple();

        /// <summary>
        /// Force the query to be treated as an <see cref="IInEdgeGremlinQuery{TElement, TInVertex}"/>.
        /// </summary>
        IInEdgeGremlinQuery<TElement, TInVertex> ForceInEdge<TInVertex>();

        /// <summary>
        /// Force the query to be treated as an <see cref="IOutEdgeGremlinQuery{TElement, TOutVertex}"/>.
        /// </summary>
        IOutEdgeGremlinQuery<TElement, TOutVertex> ForceOutEdge<TOutVertex>();

        /// <summary>
        /// Force the query to be treated as an <see cref="IVertexPropertyGremlinQuery{TElement, TValue}"/>.
        /// </summary>
        IVertexPropertyGremlinQuery<TElement, TValue> ForceVertexProperty<TValue>();

        /// <summary>
        /// Force the query to be treated as an <see cref="IArrayGremlinQuery{TArray, TArrayItem, TOriginalQuery}"/>.
        /// </summary>
        IArrayGremlinQuery<TElement[], TElement, IGremlinQuery<TElement>> ForceArray();

        /// <summary>
        /// Force the query to be treated as an <see cref="IVertexPropertyGremlinQuery{TElement, TValue, TMeta}"/>.
        /// </summary>
        IVertexPropertyGremlinQuery<TElement, TValue, TMeta> ForceVertexProperty<TValue, TMeta>();

        /// <summary>
        /// Force the query to be treated as an <see cref="IEdgeGremlinQuery{TElement, TOutVertex, TInVertex}"/>.
        /// </summary>
        IEdgeGremlinQuery<TElement, TOutVertex, TInVertex> ForceEdge<TOutVertex, TInVertex>();

        /// <inheritdoc cref="IGremlinQueryBase.Lower" />
        new IGremlinQuery<TElement> Lower();

        /// <summary>
        /// Execute the query and return results as an <see cref="IAsyncEnumerable{TElement}"/>.
        /// </summary>
        IAsyncEnumerable<TElement> ToAsyncEnumerable();
    }

    public interface IGremlinQueryBaseRec<TSelf> : IGremlinQueryBase
        where TSelf : IGremlinQueryBaseRec<TSelf>
    {
        /// <summary>
        /// Ensure all provided traversals yield a result. Filters out traversers that do not satisfy all conditions.
        /// Corresponds to the Gremlin <c>and()</c> step.
        /// </summary>
        /// <param name="andTraversals">The traversals that must all yield a result.</param>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#and-step">Reference Documentation - And Step</seealso>
        TSelf And(params Func<TSelf, IGremlinQueryBase>[] andTraversals);

        /// <inheritdoc cref="And(Func{TSelf, IGremlinQueryBase}[])" />
        TSelf And(params ReadOnlySpan<Func<TSelf, IGremlinQueryBase>> andTraversals);

        /// <summary>
        /// Randomly filter traversers with the given probability.
        /// Corresponds to the Gremlin <c>coin()</c> step.
        /// </summary>
        /// <param name="probability">The probability (0.0 to 1.0) that a traverser passes through.</param>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#coin-step">Reference Documentation - Coin Step</seealso>
        TSelf Coin(double probability);

        /// <summary>
        /// Turn the lazy traversal pipeline into a bulk-synchronous pipeline.
        /// Corresponds to the Gremlin <c>barrier()</c> step.
        /// </summary>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#barrier-step">Reference Documentation - Barrier Step</seealso>
        TSelf Barrier();

        /// <summary>
        /// Route traversers to different traversals based on a boolean condition.
        /// Corresponds to the Gremlin <c>choose()</c> step with if/then/else semantics.
        /// </summary>
        /// <typeparam name="TTargetQuery">The result query type.</typeparam>
        /// <param name="traversalPredicate">A traversal used as the boolean predicate.</param>
        /// <param name="trueChoice">The traversal to execute when the predicate is true.</param>
        /// <param name="falseChoice">The traversal to execute when the predicate is false.</param>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#choose-step">Reference Documentation - Choose Step</seealso>
        TTargetQuery Choose<TTargetQuery>(Func<TSelf, IGremlinQueryBase> traversalPredicate, Func<TSelf, TTargetQuery> trueChoice, Func<TSelf, TTargetQuery> falseChoice) where TTargetQuery : IGremlinQueryBase;

        /// <inheritdoc cref="Choose{TTargetQuery}(Func{TSelf, IGremlinQueryBase}, Func{TSelf, TTargetQuery}, Func{TSelf, TTargetQuery})" />
        TSelf Choose(Func<TSelf, IGremlinQueryBase> traversalPredicate, Func<TSelf, TSelf> trueChoice);

        /// <inheritdoc cref="Choose{TTargetQuery}(Func{TSelf, IGremlinQueryBase}, Func{TSelf, TTargetQuery}, Func{TSelf, TTargetQuery})" />
        IGremlinQuery<object> Choose(Func<TSelf, IGremlinQueryBase> traversalPredicate, Func<TSelf, IGremlinQueryBase> trueChoice);

        /// <summary>
        /// Route traversers to different traversals based on a builder pattern with multiple cases.
        /// Corresponds to the Gremlin <c>choose()</c> step.
        /// </summary>
        /// <typeparam name="TTargetQuery">The result query type.</typeparam>
        /// <param name="continuation">A builder continuation that defines cases and an optional default.</param>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#choose-step">Reference Documentation - Choose Step</seealso>
        TTargetQuery Choose<TTargetQuery>(Func<IChooseBuilder<TSelf>, IChooseBuilderWithCaseOrDefault<TTargetQuery>> continuation) where TTargetQuery : IGremlinQueryBase;

        /// <summary>
        /// Evaluate the provided traversals and return the result of the first one that emits at least one element.
        /// Corresponds to the Gremlin <c>coalesce()</c> step.
        /// </summary>
        /// <typeparam name="TTargetQuery">The result query type.</typeparam>
        /// <param name="traversals">The traversals to attempt in order.</param>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#coalesce-step">Reference Documentation - Coalesce Step</seealso>
        TTargetQuery Coalesce<TTargetQuery>(params Func<TSelf, TTargetQuery>[] traversals) where TTargetQuery : IGremlinQueryBase;

        /// <inheritdoc cref="Coalesce{TTargetQuery}(Func{TSelf, TTargetQuery}[])" />
        TTargetQuery Coalesce<TTargetQuery>(params ReadOnlySpan<Func<TSelf, TTargetQuery>> traversals) where TTargetQuery : IGremlinQueryBase;

        /// <inheritdoc cref="Coalesce{TTargetQuery}(Func{TSelf, TTargetQuery}[])" />
        IGremlinQuery<object> Coalesce(params Func<TSelf, IGremlinQueryBase>[] traversals);

        /// <inheritdoc cref="Coalesce{TTargetQuery}(Func{TSelf, TTargetQuery}[])" />
        IGremlinQuery<object> Coalesce(params ReadOnlySpan<Func<TSelf, IGremlinQueryBase>> traversals);

        /// <summary>
        /// Filter traversers on cyclic paths. Only traversers whose path contains a repeated object are kept.
        /// Corresponds to the Gremlin <c>cyclicPath()</c> step.
        /// </summary>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#cyclicpath-step">Reference Documentation - CyclicPath Step</seealso>
        TSelf CyclicPath();

        /// <summary>
        /// Remove duplicate traversers from the stream.
        /// Corresponds to the Gremlin <c>dedup()</c> step.
        /// </summary>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#dedup-step">Reference Documentation - Dedup Step</seealso>
        TSelf Dedup();

        /// <summary>
        /// Remove duplicate items within each local collection.
        /// Corresponds to the Gremlin <c>dedup(local)</c> step.
        /// </summary>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#dedup-step">Reference Documentation - Dedup Step</seealso>
        TSelf DedupLocal();

        /// <summary>
        /// Map the traverser to some object and flatten the result into the traversal stream.
        /// Corresponds to the Gremlin <c>flatMap()</c> step.
        /// </summary>
        /// <typeparam name="TTargetQuery">The result query type.</typeparam>
        /// <param name="mapping">The mapping traversal.</param>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#flatmap-step">Reference Documentation - FlatMap Step</seealso>
        TTargetQuery FlatMap<TTargetQuery>(Func<TSelf, TTargetQuery> mapping) where TTargetQuery : IGremlinQueryBase;

        /// <summary>
        /// Map the traverser to itself. A no-op step that preserves the traverser.
        /// Corresponds to the Gremlin <c>identity()</c> step.
        /// </summary>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#identity-step">Reference Documentation - Identity Step</seealso>
        TSelf Identity();

        /// <summary>
        /// Limit the number of traversers in the stream.
        /// Corresponds to the Gremlin <c>limit()</c> step.
        /// </summary>
        /// <param name="count">The maximum number of traversers to allow.</param>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#limit-step">Reference Documentation - Limit Step</seealso>
        TSelf Limit(long count);

        /// <summary>
        /// Execute a traversal within a local scope (i.e., on each element individually).
        /// Corresponds to the Gremlin <c>local()</c> step.
        /// </summary>
        /// <typeparam name="TTargetQuery">The result query type.</typeparam>
        /// <param name="localTraversal">The local traversal to execute.</param>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#local-step">Reference Documentation - Local Step</seealso>
        TTargetQuery Local<TTargetQuery>(Func<TSelf, TTargetQuery> localTraversal) where TTargetQuery : IGremlinQueryBase;

        /// <summary>
        /// Map the traverser to some object of a different query type.
        /// Corresponds to the Gremlin <c>map()</c> step.
        /// </summary>
        /// <typeparam name="TTargetQuery">The result query type.</typeparam>
        /// <param name="mapping">The mapping traversal.</param>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#map-step">Reference Documentation - Map Step</seealso>
        TTargetQuery Map<TTargetQuery>(Func<TSelf, TTargetQuery> mapping) where TTargetQuery : IGremlinQueryBase;

        /// <summary>
        /// Determine the maximum value in the stream.
        /// Corresponds to the Gremlin <c>max()</c> step.
        /// </summary>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#max-step">Reference Documentation - Max Step</seealso>
        TSelf Max();

        /// <summary>
        /// Determine the mean value in the stream.
        /// Corresponds to the Gremlin <c>mean()</c> step.
        /// </summary>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#mean-step">Reference Documentation - Mean Step</seealso>
        TSelf Mean();

        /// <summary>
        /// Determine the minimum value in the stream.
        /// Corresponds to the Gremlin <c>min()</c> step.
        /// </summary>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#min-step">Reference Documentation - Min Step</seealso>
        TSelf Min();

        /// <summary>
        /// Filter traversers that match the provided traversal. Keeps traversers for which the traversal yields no result.
        /// Corresponds to the Gremlin <c>not()</c> step.
        /// </summary>
        /// <param name="notTraversal">The traversal whose match should be negated.</param>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#not-step">Reference Documentation - Not Step</seealso>
        TSelf Not(Func<TSelf, IGremlinQueryBase> notTraversal);

        /// <summary>
        /// Filter out all traversers. No traversers pass through this step.
        /// Corresponds to the Gremlin <c>none()</c> step.
        /// </summary>
        TSelf None();

        /// <summary>
        /// If the provided traversal yields a result, return that result; otherwise, return the original traverser.
        /// Corresponds to the Gremlin <c>optional()</c> step.
        /// </summary>
        /// <param name="optionalTraversal">The optional traversal to attempt.</param>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#optional-step">Reference Documentation - Optional Step</seealso>
        TSelf Optional(Func<TSelf, TSelf> optionalTraversal);

        /// <summary>
        /// Ensure at least one of the provided traversals yields a result. Filters out traversers that satisfy none of the conditions.
        /// Corresponds to the Gremlin <c>or()</c> step.
        /// </summary>
        /// <param name="orTraversals">The traversals of which at least one must yield a result.</param>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#or-step">Reference Documentation - Or Step</seealso>
        TSelf Or(params Func<TSelf, IGremlinQueryBase>[] orTraversals);

        /// <inheritdoc cref="Or(Func{TSelf, IGremlinQueryBase}[])" />
        TSelf Or(params ReadOnlySpan<Func<TSelf, IGremlinQueryBase>> orTraversals);

        /// <summary>
        /// Order the traversers in the stream.
        /// Corresponds to the Gremlin <c>order()</c> step.
        /// </summary>
        /// <param name="projection">A builder that specifies the ordering criteria.</param>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#order-step">Reference Documentation - Order Step</seealso>
        TSelf Order(Func<IOrderBuilder<TSelf>, IOrderBuilderWithBy<TSelf>> projection);

        /// <summary>
        /// Order items within each local collection.
        /// Corresponds to the Gremlin <c>order(local)</c> step.
        /// </summary>
        /// <param name="projection">A builder that specifies the ordering criteria.</param>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#order-step">Reference Documentation - Order Step</seealso>
        TSelf OrderLocal(Func<IOrderBuilder<TSelf>, IOrderBuilderWithBy<TSelf>> projection);

        /// <summary>
        /// Get a range of traversers from the stream.
        /// Corresponds to the Gremlin <c>range()</c> step.
        /// </summary>
        /// <param name="low">The inclusive low bound.</param>
        /// <param name="high">The exclusive high bound.</param>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#range-step">Reference Documentation - Range Step</seealso>
        TSelf Range(long low, long high);

        /// <summary>
        /// Define a looping construct over the traversal using repeat/until/emit semantics.
        /// Corresponds to the Gremlin <c>repeat()</c>, <c>until()</c>, and <c>emit()</c> steps.
        /// </summary>
        /// <param name="loopBuilderTransformation">A builder that defines the loop structure.</param>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#repeat-step">Reference Documentation - Repeat Step</seealso>
        TSelf Loop(Func<IStartLoopBuilder<TSelf>, IFinalLoopBuilder<TSelf>> loopBuilderTransformation);

        /// <summary>
        /// Execute a side-effect traversal without affecting the main traversal stream.
        /// Corresponds to the Gremlin <c>sideEffect()</c> step.
        /// </summary>
        /// <param name="sideEffectTraversal">The side-effect traversal.</param>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#sideeffect-step">Reference Documentation - SideEffect Step</seealso>
        TSelf SideEffect(Func<TSelf, IGremlinQueryBase> sideEffectTraversal);

        /// <summary>
        /// Filter traversers on simple (non-cyclic) paths.
        /// Corresponds to the Gremlin <c>simplePath()</c> step.
        /// </summary>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#simplepath-step">Reference Documentation - SimplePath Step</seealso>
        TSelf SimplePath();

        /// <summary>
        /// Skip a number of traversers in the stream.
        /// Corresponds to the Gremlin <c>skip()</c> step.
        /// </summary>
        /// <param name="count">The number of traversers to skip.</param>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#skip-step">Reference Documentation - Skip Step</seealso>
        TSelf Skip(long count);

        /// <summary>
        /// Compute the sum of all values in the stream.
        /// Corresponds to the Gremlin <c>sum()</c> step.
        /// </summary>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#sum-step">Reference Documentation - Sum Step</seealso>
        TSelf Sum();

        /// <summary>
        /// Keep the last traversers from the end of the stream.
        /// Corresponds to the Gremlin <c>tail()</c> step.
        /// </summary>
        /// <param name="count">The number of traversers to keep from the end.</param>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#tail-step">Reference Documentation - Tail Step</seealso>
        TSelf Tail(long count);

        /// <summary>
        /// Merge the results of multiple traversals into a single stream.
        /// Corresponds to the Gremlin <c>union()</c> step.
        /// </summary>
        /// <typeparam name="TTargetQuery">The result query type.</typeparam>
        /// <param name="unionTraversals">The traversals whose results will be merged.</param>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#union-step">Reference Documentation - Union Step</seealso>
        TTargetQuery Union<TTargetQuery>(params Func<TSelf, TTargetQuery>[] unionTraversals) where TTargetQuery : IGremlinQueryBase;

        /// <inheritdoc cref="Union{TTargetQuery}(Func{TSelf, TTargetQuery}[])" />
        TTargetQuery Union<TTargetQuery>(params ReadOnlySpan<Func<TSelf, TTargetQuery>> unionTraversals) where TTargetQuery : IGremlinQueryBase;

        /// <inheritdoc cref="Union{TTargetQuery}(Func{TSelf, TTargetQuery}[])" />
        IGremlinQuery<object> Union(params Func<TSelf, IGremlinQueryBase>[] traversals);

        /// <inheritdoc cref="Union{TTargetQuery}(Func{TSelf, TTargetQuery}[])" />
        IGremlinQuery<object> Union(params ReadOnlySpan<Func<TSelf, IGremlinQueryBase>> traversals);

        /// <summary>
        /// Filter traversers by applying a traversal-based predicate.
        /// Corresponds to the Gremlin <c>where()</c> step.
        /// </summary>
        /// <param name="filterTraversal">A traversal that acts as a filter; traversers that yield a result pass through.</param>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#where-step">Reference Documentation - Where Step</seealso>
        TSelf Where(Func<TSelf, IGremlinQueryBase> filterTraversal);
    }

    public interface IGremlinQueryBaseRec<TElement, TSelf> :
        IGremlinQueryBaseRec<TSelf>,
        IGremlinQueryBase<TElement>
        where TSelf : IGremlinQueryBaseRec<TElement, TSelf>
    {
        /// <summary>
        /// Eagerly collect traversers into a side-effect list and provide a label for later retrieval.
        /// Corresponds to the Gremlin <c>aggregate(global)</c> step.
        /// </summary>
        /// <typeparam name="TTargetQuery">The result query type.</typeparam>
        /// <param name="continuation">A function that receives the query and the generated step label.</param>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#aggregate-step">Reference Documentation - Aggregate Step</seealso>
        TTargetQuery Aggregate<TTargetQuery>(Func<TSelf, StepLabel<IArrayGremlinQuery<TElement[], TElement, TSelf>, TElement[]>, TTargetQuery> continuation) where TTargetQuery : IGremlinQueryBase;

        /// <summary>
        /// Eagerly collect traversers into a local side-effect list and provide a label for later retrieval.
        /// Corresponds to the Gremlin <c>aggregate(local)</c> step.
        /// </summary>
        /// <typeparam name="TTargetQuery">The result query type.</typeparam>
        /// <param name="continuation">A function that receives the query and the generated step label.</param>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#aggregate-step">Reference Documentation - Aggregate Step</seealso>
        TTargetQuery AggregateLocal<TTargetQuery>(Func<TSelf, StepLabel<IArrayGremlinQuery<TElement[], TElement, TSelf>, TElement[]>, TTargetQuery> continuation) where TTargetQuery : IGremlinQueryBase;

        /// <summary>
        /// Eagerly collect traversers into a side-effect list referenced by the given label.
        /// Corresponds to the Gremlin <c>aggregate(global)</c> step.
        /// </summary>
        /// <param name="stepLabel">The step label to store the aggregated results under.</param>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#aggregate-step">Reference Documentation - Aggregate Step</seealso>
        TSelf Aggregate(StepLabel<TElement[]> stepLabel);

        /// <summary>
        /// Eagerly collect traversers into a local side-effect list referenced by the given label.
        /// Corresponds to the Gremlin <c>aggregate(local)</c> step.
        /// </summary>
        /// <param name="stepLabel">The step label to store the aggregated results under.</param>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#aggregate-step">Reference Documentation - Aggregate Step</seealso>
        TSelf AggregateLocal(StepLabel<TElement[]> stepLabel);

        /// <summary>
        /// Label the current step for later reference within the traversal.
        /// Corresponds to the Gremlin <c>as()</c> step modulator.
        /// </summary>
        /// <param name="stepLabel">The label to assign to this step.</param>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#as-step">Reference Documentation - As Step</seealso>
        TSelf As(StepLabel<TElement> stepLabel);

        /// <summary>
        /// Label the current step and provide the label to a continuation.
        /// Corresponds to the Gremlin <c>as()</c> step modulator.
        /// </summary>
        /// <typeparam name="TTargetQuery">The result query type.</typeparam>
        /// <param name="continuation">A function that receives the query and the generated step label.</param>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#as-step">Reference Documentation - As Step</seealso>
        TTargetQuery As<TTargetQuery>(Func<TSelf, StepLabel<TSelf, TElement>, TTargetQuery> continuation) where TTargetQuery : IGremlinQueryBase;

        /// <summary>
        /// Route traversers to different traversals based on a predicate expression.
        /// Corresponds to the Gremlin <c>choose()</c> step with if/then/else semantics.
        /// </summary>
        /// <typeparam name="TTargetQuery">The result query type.</typeparam>
        /// <param name="predicate">An expression used as the boolean predicate.</param>
        /// <param name="trueChoice">The traversal to execute when the predicate is true.</param>
        /// <param name="falseChoice">The traversal to execute when the predicate is false.</param>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#choose-step">Reference Documentation - Choose Step</seealso>
        TTargetQuery Choose<TTargetQuery>(Expression<Func<TElement, bool>> predicate, Func<TSelf, TTargetQuery> trueChoice, Func<TSelf, TTargetQuery> falseChoice) where TTargetQuery : IGremlinQueryBase;

        /// <inheritdoc cref="Choose{TTargetQuery}(Expression{Func{TElement, bool}}, Func{TSelf, TTargetQuery}, Func{TSelf, TTargetQuery})" />
        TSelf Choose(Expression<Func<TElement, bool>> predicate, Func<TSelf, TSelf> trueChoice);

        /// <inheritdoc cref="Choose{TTargetQuery}(Expression{Func{TElement, bool}}, Func{TSelf, TTargetQuery}, Func{TSelf, TTargetQuery})" />
        IGremlinQuery<object> Choose(Expression<Func<TElement, bool>> predicate, Func<TSelf, IGremlinQueryBase> trueChoice);

        /// <summary>
        /// Collect all traversers into a single list.
        /// Corresponds to the Gremlin <c>fold()</c> step.
        /// </summary>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#fold-step">Reference Documentation - Fold Step</seealso>
        IArrayGremlinQuery<TElement[], TElement, TSelf> Fold();

        /// <inheritdoc cref="IGremlinQueryBase{TElement}.ForceArray" />
        new IArrayGremlinQuery<TElement[], TElement, TSelf> ForceArray();

        /// <summary>
        /// Organize elements into a dictionary using a key/value builder.
        /// Corresponds to the Gremlin <c>group()</c> step.
        /// </summary>
        /// <typeparam name="TNewKey">The key type of the resulting map.</typeparam>
        /// <typeparam name="TNewValue">The value type of the resulting map.</typeparam>
        /// <param name="groupBuilder">A builder that specifies key and value selectors.</param>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#group-step">Reference Documentation - Group Step</seealso>
        IMapGremlinQuery<IDictionary<TNewKey, TNewValue>> Group<TNewKey, TNewValue>(Func<IGroupBuilder<TSelf>, IGroupBuilderWithKeyAndValue<TNewKey, TNewValue>> groupBuilder);

        /// <summary>
        /// Organize elements into a dictionary using a key builder, with element arrays as values.
        /// Corresponds to the Gremlin <c>group()</c> step.
        /// </summary>
        /// <typeparam name="TNewKey">The key type of the resulting map.</typeparam>
        /// <param name="groupBuilder">A builder that specifies the key selector.</param>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#group-step">Reference Documentation - Group Step</seealso>
        IMapGremlinQuery<IDictionary<TNewKey, TElement[]>> Group<TNewKey>(Func<IGroupBuilder<TSelf>, IGroupBuilderWithKey<TSelf, TNewKey>> groupBuilder);

        /// <summary>
        /// Inject additional elements into the traversal stream.
        /// Corresponds to the Gremlin <c>inject()</c> step.
        /// </summary>
        /// <param name="elements">The elements to inject.</param>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#inject-step">Reference Documentation - Inject Step</seealso>
        TSelf Inject(params TElement[] elements);

        /// <inheritdoc cref="Inject(TElement[])" />
        TSelf Inject(params ReadOnlySpan<TElement> elements);

        /// <summary>
        /// Project elements to a dynamic result using a builder pattern.
        /// Corresponds to the Gremlin <c>project()</c> step.
        /// </summary>
        /// <param name="continuation">A builder that defines the projections.</param>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#project-step">Reference Documentation - Project Step</seealso>
        IGremlinQuery<dynamic> Project(Func<IProjectBuilder<TSelf, TElement>, IProjectDynamicResult> continuation);

        /// <summary>
        /// Project elements to a strongly-typed result using a builder pattern.
        /// Corresponds to the Gremlin <c>project()</c> step.
        /// </summary>
        /// <typeparam name="TResult">The target type for the projection.</typeparam>
        /// <param name="continuation">A builder that defines the projections.</param>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#project-step">Reference Documentation - Project Step</seealso>
        IMapGremlinQuery<TResult> Project<TResult>(Func<IProjectBuilder<TSelf, TElement>, IProjectMapResult<TResult>> continuation);

        /// <summary>
        /// Project elements to a tuple result using a builder pattern.
        /// Corresponds to the Gremlin <c>project()</c> step.
        /// </summary>
        /// <typeparam name="TResult">The tuple type for the projection.</typeparam>
        /// <param name="continuation">A builder that defines the projections.</param>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#project-step">Reference Documentation - Project Step</seealso>
        IMapGremlinQuery<TResult> Project<TResult>(Func<IProjectBuilder<TSelf, TElement>, IProjectTupleResult<TResult>> continuation) where TResult : ITuple;

        /// <inheritdoc cref="IGremlinQueryBaseRec{TSelf}.Order(Func{IOrderBuilder{TSelf}, IOrderBuilderWithBy{TSelf}})" />
        TSelf Order(Func<IOrderBuilder<TElement, TSelf>, IOrderBuilderWithBy<TElement, TSelf>> projection);

        /// <inheritdoc cref="IGremlinQueryBaseRec{TSelf}.OrderLocal(Func{IOrderBuilder{TSelf}, IOrderBuilderWithBy{TSelf}})" />
        TSelf OrderLocal(Func<IOrderBuilder<TElement, TSelf>, IOrderBuilderWithBy<TElement, TSelf>> projection);

        /// <summary>
        /// Filter traversers by an expression predicate on the element.
        /// Corresponds to the Gremlin <c>where()</c>/<c>has()</c> steps with predicate-based filtering.
        /// </summary>
        /// <param name="predicate">A boolean expression used to filter elements.</param>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#where-step">Reference Documentation - Where Step</seealso>
        TSelf Where(Expression<Func<TElement, bool>> predicate);
    }

    public interface IGremlinQuery<TElement> : IGremlinQueryBaseRec<TElement, IGremlinQuery<TElement>>;
}
