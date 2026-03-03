namespace ExRam.Gremlinq.Core
{
    /// <summary>A query over date/time values, providing date arithmetic steps.</summary>
    /// <typeparam name="TDate">The date/time type.</typeparam>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#datetime-steps">Reference Documentation - DateTime Steps</seealso>
    public interface IDateGremlinQuery<TDate> : IGremlinQueryBaseRec<TDate, IDateGremlinQuery<TDate>>
    {
        /// <summary>
        /// Adds the specified duration to the date/time value.
        /// </summary>
        /// <param name="duration">The duration to add. Only Days, Hours, Minutes, and Seconds are supported by the underlying Gremlin dateAdd() step; milliseconds and smaller time units will be ignored.</param>
        /// <returns>A query with the updated date/time value.</returns>
        IDateGremlinQuery<TDate> Add(TimeSpan duration);

        /// <summary>
        /// Returns the difference between the traverser's date/time value and <paramref name="other"/> in epoch seconds.
        /// Corresponds to the Gremlin <c>dateDiff()</c> step.
        /// </summary>
        /// <param name="other">The date/time value to subtract.</param>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#datediff-step">Reference Documentation - DateDiff Step</seealso>
        IGremlinQuery<long> Diff(DateTimeOffset other);

        /// <summary>
        /// Returns the difference between the traverser's date/time value and the result of <paramref name="other"/> in epoch seconds.
        /// Corresponds to the Gremlin <c>dateDiff()</c> step.
        /// </summary>
        /// <param name="other">A traversal that produces the date/time value to subtract.</param>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#datediff-step">Reference Documentation - DateDiff Step</seealso>
        IGremlinQuery<long> Diff(Func<IDateGremlinQuery<TDate>, IGremlinQueryBase<DateTimeOffset>> other);
    }
}
