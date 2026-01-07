namespace ExRam.Gremlinq.Core
{
    public static class DateGremlinQueryExtensions
    {
        public static IDateGremlinQuery<DateTimeOffset> Add(this IGremlinQueryBase<DateTimeOffset> query, TimeSpan duration) => query
            .AsAdmin()
            .ChangeQueryType<IDateGremlinQuery<DateTimeOffset>>()
            .Add(duration);

        public static IGremlinQuery<long> Diff(this IGremlinQueryBase<DateTimeOffset> query, DateTimeOffset other) => query
            .AsAdmin()
            .ChangeQueryType<IDateGremlinQuery<DateTimeOffset>>()
            .Diff(other);

        public static IGremlinQuery<long> Diff(this IGremlinQueryBase<DateTimeOffset> query, Func<IDateGremlinQuery<DateTimeOffset>, IGremlinQueryBase<DateTimeOffset>> other) => query
            .AsAdmin()
            .ChangeQueryType<IDateGremlinQuery<DateTimeOffset>>()
            .Diff(other);
    }
}
