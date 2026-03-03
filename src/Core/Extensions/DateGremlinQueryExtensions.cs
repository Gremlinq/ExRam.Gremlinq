namespace ExRam.Gremlinq.Core
{
    /// <summary>Extension methods for date-typed Gremlin queries.</summary>
    public static class DateGremlinQueryExtensions
    {
        /// <inheritdoc cref="IDateGremlinQuery{TDate}.Add" />
        public static IDateGremlinQuery<DateTimeOffset> Add(this IGremlinQueryBase<DateTimeOffset> query, TimeSpan duration)
        {
            ArgumentNullException.ThrowIfNull(query);

            return query
                .AsAdmin()
                .ChangeQueryType<IDateGremlinQuery<DateTimeOffset>>()
                .Add(duration);
        }

        /// <inheritdoc cref="IDateGremlinQuery{TDate}.Diff(DateTimeOffset)" />
        public static IGremlinQuery<long> Diff(this IGremlinQueryBase<DateTimeOffset> query, DateTimeOffset other)
        {
            ArgumentNullException.ThrowIfNull(query);

            return query
                .AsAdmin()
                .ChangeQueryType<IDateGremlinQuery<DateTimeOffset>>()
                .Diff(other);
        }

        /// <inheritdoc cref="IDateGremlinQuery{TDate}.Diff(Func{IDateGremlinQuery{TDate}, IGremlinQueryBase{DateTimeOffset}})" />
        public static IGremlinQuery<long> Diff(this IGremlinQueryBase<DateTimeOffset> query, Func<IDateGremlinQuery<DateTimeOffset>, IGremlinQueryBase<DateTimeOffset>> other)
        {
            ArgumentNullException.ThrowIfNull(query);
            ArgumentNullException.ThrowIfNull(other);

            return query
                .AsAdmin()
                .ChangeQueryType<IDateGremlinQuery<DateTimeOffset>>()
                .Diff(other);
        }
    }
}
