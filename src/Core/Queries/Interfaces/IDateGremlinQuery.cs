namespace ExRam.Gremlinq.Core
{
    public interface IDateGremlinQuery<TDate> : IGremlinQueryBaseRec<TDate, IDateGremlinQuery<TDate>>
    {
        /// <summary>
        /// Adds the specified duration to the date/time value.
        /// </summary>
        /// <param name="duration">The duration to add. Only Days, Hours, Minutes, and Seconds are supported by the underlying Gremlin dateAdd() step; milliseconds and smaller time units will be ignored.</param>
        /// <returns>A query with the updated date/time value.</returns>
        IDateGremlinQuery<TDate> Add(TimeSpan duration);

        IGremlinQuery<long> Diff(DateTimeOffset other);

        IGremlinQuery<long> Diff(Func<IDateGremlinQuery<TDate>, IGremlinQueryBase<DateTimeOffset>> other);
    }
}
