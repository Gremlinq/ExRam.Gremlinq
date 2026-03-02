namespace ExRam.Gremlinq.Core
{
    public interface IChooseBuilderWithCase<out TSourceQuery, in TElement, TTargetQuery> : IChooseBuilderWithCaseOrDefault<TTargetQuery>
        where TSourceQuery : IGremlinQueryBase where TTargetQuery : IGremlinQueryBase
    {
        /// <summary>
        /// Adds a case branch that is taken when the choice traversal produces a value equal to <paramref name="element"/>.
        /// Corresponds to the Gremlin <c>option()</c> modulator on a <c>choose()</c> step.
        /// </summary>
        /// <param name="element">The value to match.</param>
        /// <param name="continuation">The traversal to execute for this case.</param>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#choose-step">Reference Documentation - Choose Step</seealso>
        IChooseBuilderWithCase<TSourceQuery, TElement, TTargetQuery> Case(TElement element, Func<TSourceQuery, TTargetQuery> continuation);

        /// <summary>
        /// Adds a default branch that is taken when no case matches.
        /// Corresponds to the Gremlin <c>option(none, ...)</c> modulator on a <c>choose()</c> step.
        /// </summary>
        /// <param name="continuation">The traversal to execute as default.</param>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#choose-step">Reference Documentation - Choose Step</seealso>
        IChooseBuilderWithCaseOrDefault<TTargetQuery> Default(Func<TSourceQuery, TTargetQuery> continuation);
    }
}
