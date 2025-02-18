namespace ExRam.Gremlinq.Core
{
    public static class StringGremlinQueryExtensions
    {
        public static IStringGremlinQuery<string> Concat(this IGremlinQueryBase<string> query, params string[] strings) => query
            .AsAdmin()
            .ChangeQueryType<IStringGremlinQuery<string>>()
            .Concat(strings);

        public static IStringGremlinQuery<string> Concat(this IGremlinQueryBase<string> query, params Func<IStringGremlinQuery<string>, IGremlinQueryBase<string>>[] stringTraversals) => query
            .AsAdmin()
            .ChangeQueryType<IStringGremlinQuery<string>>()
            .Concat(stringTraversals);

        public static IStringGremlinQuery<string> Substring(this IGremlinQueryBase<string> query, int startIndex) => query
            .AsAdmin()
            .ChangeQueryType<IStringGremlinQuery<string>>()
            .Substring(startIndex);

        public static IStringGremlinQuery<string> Substring(this IGremlinQueryBase<string> query, int startIndex, int length) => query
            .AsAdmin()
            .ChangeQueryType<IStringGremlinQuery<string>>()
            .Substring(startIndex, length);

        public static IStringGremlinQuery<string> Substring(this IGremlinQueryBase<string> query, Range range) => query
            .AsAdmin()
            .ChangeQueryType<IStringGremlinQuery<string>>()
            .Substring(range);
    }
}
