namespace ExRam.Gremlinq.Core
{
    public static class StringGremlinQueryExtensions
    {
        public static IStringGremlinQuery<string> Concat(this IGremlinQueryBase<string> query, params string[] strings)
        {
            ArgumentNullException.ThrowIfNull(query);
            ArgumentNullException.ThrowIfNull(strings);

            return query
                .AsAdmin()
                .ChangeQueryType<IStringGremlinQuery<string>>()
                .Concat(strings);
        }

        public static IStringGremlinQuery<string> Concat(this IGremlinQueryBase<string> query, params Func<IStringGremlinQuery<string>, IGremlinQueryBase<string>>[] stringTraversals)
        {
            ArgumentNullException.ThrowIfNull(query);
            ArgumentNullException.ThrowIfNull(stringTraversals);

            return query
                .AsAdmin()
                .ChangeQueryType<IStringGremlinQuery<string>>()
                .Concat(stringTraversals);
        }

        public static IStringGremlinQuery<string> Concat(this IGremlinQueryBase<string> query, params ReadOnlySpan<Func<IStringGremlinQuery<string>, IGremlinQueryBase<string>>> stringTraversals)
        {
            ArgumentNullException.ThrowIfNull(query);

            return query
                .AsAdmin()
                .ChangeQueryType<IStringGremlinQuery<string>>()
                .Concat(stringTraversals);
        }

        public static IGremlinQuery<int> Length(this IGremlinQueryBase<string> query)
        {
            ArgumentNullException.ThrowIfNull(query);

            return query
                .AsAdmin()
                .ChangeQueryType<IStringGremlinQuery<string>>()
                .Length();
        }

        public static IStringGremlinQuery<string> Replace(this IGremlinQueryBase<string> query, string oldValue, string newValue)
        {
            ArgumentNullException.ThrowIfNull(query);
            ArgumentNullException.ThrowIfNull(oldValue);
            ArgumentNullException.ThrowIfNull(newValue);

            return query
                .AsAdmin()
                .ChangeQueryType<IStringGremlinQuery<string>>()
                .Replace(oldValue, newValue);
        }

        public static IStringGremlinQuery<string> Reverse(this IGremlinQueryBase<string> query)
        {
            ArgumentNullException.ThrowIfNull(query);

            return query
                .AsAdmin()
                .ChangeQueryType<IStringGremlinQuery<string>>()
                .Reverse();
        }

        public static IStringGremlinQuery<string> Substring(this IGremlinQueryBase<string> query, int startIndex)
        {
            ArgumentNullException.ThrowIfNull(query);

            return query
                .AsAdmin()
                .ChangeQueryType<IStringGremlinQuery<string>>()
                .Substring(startIndex);
        }

        public static IStringGremlinQuery<string> Substring(this IGremlinQueryBase<string> query, int startIndex, int length)
        {
            ArgumentNullException.ThrowIfNull(query);

            return query
                .AsAdmin()
                .ChangeQueryType<IStringGremlinQuery<string>>()
                .Substring(startIndex, length);
        }

        public static IStringGremlinQuery<string> Substring(this IGremlinQueryBase<string> query, Range range)
        {
            ArgumentNullException.ThrowIfNull(query);

            return query
                .AsAdmin()
                .ChangeQueryType<IStringGremlinQuery<string>>()
                .Substring(range);
        }

        public static IStringGremlinQuery<string> ToLower(this IGremlinQueryBase<string> query)
        {
            ArgumentNullException.ThrowIfNull(query);

            return query
                .AsAdmin()
                .ChangeQueryType<IStringGremlinQuery<string>>()
                .ToLower();
        }

        public static IStringGremlinQuery<string> ToUpper(this IGremlinQueryBase<string> query)
        {
            ArgumentNullException.ThrowIfNull(query);

            return query
                .AsAdmin()
                .ChangeQueryType<IStringGremlinQuery<string>>()
                .ToUpper();
        }

        public static IStringGremlinQuery<string> Trim(this IGremlinQueryBase<string> query)
        {
            ArgumentNullException.ThrowIfNull(query);

            return query
                .AsAdmin()
                .ChangeQueryType<IStringGremlinQuery<string>>()
                .Trim();
        }

        public static IStringGremlinQuery<string> TrimStart(this IGremlinQueryBase<string> query)
        {
            ArgumentNullException.ThrowIfNull(query);

            return query
                .AsAdmin()
                .ChangeQueryType<IStringGremlinQuery<string>>()
                .TrimStart();
        }

        public static IStringGremlinQuery<string> TrimEnd(this IGremlinQueryBase<string> query)
        {
            ArgumentNullException.ThrowIfNull(query);

            return query
                .AsAdmin()
                .ChangeQueryType<IStringGremlinQuery<string>>()
                .TrimEnd();
        }
    }
}
