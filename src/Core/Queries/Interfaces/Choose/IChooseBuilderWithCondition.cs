namespace ExRam.Gremlinq.Core
{
    public interface IChooseBuilderWithCondition<out TSourceQuery, in TElement>
        where TSourceQuery : IGremlinQueryBase
    {
        /// <summary>
        /// Adds a case branch that is taken when the choice traversal produces a value equal to <paramref name="element"/>.
        /// Corresponds to the Gremlin <c>option()</c> modulator on a <c>choose()</c> step.
        /// </summary>
        /// <typeparam name="TTargetQuery">The query type produced by the branch.</typeparam>
        /// <param name="element">The value to match.</param>
        /// <param name="continuation">The traversal to execute for this case.</param>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#choose-step">Reference Documentation - Choose Step</seealso>
        IChooseBuilderWithCase<TSourceQuery, TElement, TTargetQuery> Case<TTargetQuery>(TElement element, Func<TSourceQuery, TTargetQuery> continuation) where TTargetQuery : IGremlinQueryBase;

        /// <summary>
        /// Adds a default branch that is taken when no case matches.
        /// Corresponds to the Gremlin <c>option(none, ...)</c> modulator on a <c>choose()</c> step.
        /// </summary>
        /// <typeparam name="TTargetQuery">The query type produced by the default branch.</typeparam>
        /// <param name="continuation">The traversal to execute as default.</param>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#choose-step">Reference Documentation - Choose Step</seealso>
        IChooseBuilderWithCaseOrDefault<TTargetQuery> Default<TTargetQuery>(Func<TSourceQuery, TTargetQuery> continuation) where TTargetQuery : IGremlinQueryBase;
    }
}
