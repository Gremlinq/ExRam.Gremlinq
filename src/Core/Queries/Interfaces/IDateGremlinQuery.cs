namespace ExRam.Gremlinq.Core
{
    public interface IDateGremlinQuery<TDate> : IGremlinQueryBaseRec<TDate, IDateGremlinQuery<TDate>>
    {
        /// <summary>
        /// Adds the specified duration to the date/time value.
        /// </summary>
        /// <param name="duration">The duration to add. Note: Milliseconds and smaller time units are not supported by the underlying Gremlin dateAdd() step and will be ignored.</param>
        /// <returns>A query with the updated date/time value.</returns>
        IDateGremlinQuery<TDate> Add(TimeSpan duration);
    }
}
