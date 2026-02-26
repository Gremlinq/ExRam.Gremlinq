namespace ExRam.Gremlinq.Core
{
    public static class DateGremlinQueryExtensions
    {
        public static IDateGremlinQuery<DateTimeOffset> Add(this IGremlinQueryBase<DateTimeOffset> query, TimeSpan duration)
        {
            ArgumentNullException.ThrowIfNull(query);

            return query
                .AsAdmin()
                .ChangeQueryType<IDateGremlinQuery<DateTimeOffset>>()
                .Add(duration);
        }

        public static IGremlinQuery<long> Diff(this IGremlinQueryBase<DateTimeOffset> query, DateTimeOffset other)
        {
            ArgumentNullException.ThrowIfNull(query);

            return query
                .AsAdmin()
                .ChangeQueryType<IDateGremlinQuery<DateTimeOffset>>()
                .Diff(other);
        }

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
