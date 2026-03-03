namespace ExRam.Gremlinq.Core
{
    /// <summary>Extension methods for string-typed Gremlin queries.</summary>
    public static class StringGremlinQueryExtensions
    {
        /// <inheritdoc cref="IStringGremlinQuery{TString}.Concat(string[])" />
        public static IStringGremlinQuery<string> Concat(this IGremlinQueryBase<string> query, params string[] strings)
        {
            ArgumentNullException.ThrowIfNull(query);
            ArgumentNullException.ThrowIfNull(strings);

            return query
                .AsAdmin()
                .ChangeQueryType<IStringGremlinQuery<string>>()
                .Concat(strings);
        }

        /// <inheritdoc cref="IStringGremlinQuery{TString}.Concat(Func{IStringGremlinQuery{TString}, IGremlinQueryBase{TString}}[])" />
        public static IStringGremlinQuery<string> Concat(this IGremlinQueryBase<string> query, params Func<IStringGremlinQuery<string>, IGremlinQueryBase<string>>[] stringTraversals)
        {
            ArgumentNullException.ThrowIfNull(query);
            ArgumentNullException.ThrowIfNull(stringTraversals);

            return query
                .AsAdmin()
                .ChangeQueryType<IStringGremlinQuery<string>>()
                .Concat(stringTraversals);
        }

        /// <inheritdoc cref="IStringGremlinQuery{TString}.Concat(Func{IStringGremlinQuery{TString}, IGremlinQueryBase{TString}}[])" />
        public static IStringGremlinQuery<string> Concat(this IGremlinQueryBase<string> query, params ReadOnlySpan<Func<IStringGremlinQuery<string>, IGremlinQueryBase<string>>> stringTraversals)
        {
            ArgumentNullException.ThrowIfNull(query);

            return query
                .AsAdmin()
                .ChangeQueryType<IStringGremlinQuery<string>>()
                .Concat(stringTraversals);
        }

        /// <inheritdoc cref="IStringGremlinQuery{TString}.Length" />
        public static IGremlinQuery<int> Length(this IGremlinQueryBase<string> query)
        {
            ArgumentNullException.ThrowIfNull(query);

            return query
                .AsAdmin()
                .ChangeQueryType<IStringGremlinQuery<string>>()
                .Length();
        }

        /// <inheritdoc cref="IStringGremlinQuery{TString}.Replace" />
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

        /// <inheritdoc cref="IStringGremlinQuery{TString}.Reverse" />
        public static IStringGremlinQuery<string> Reverse(this IGremlinQueryBase<string> query)
        {
            ArgumentNullException.ThrowIfNull(query);

            return query
                .AsAdmin()
                .ChangeQueryType<IStringGremlinQuery<string>>()
                .Reverse();
        }

        /// <inheritdoc cref="IStringGremlinQuery{TString}.Substring(int)" />
        public static IStringGremlinQuery<string> Substring(this IGremlinQueryBase<string> query, int startIndex)
        {
            ArgumentNullException.ThrowIfNull(query);

            return query
                .AsAdmin()
                .ChangeQueryType<IStringGremlinQuery<string>>()
                .Substring(startIndex);
        }

        /// <inheritdoc cref="IStringGremlinQuery{TString}.Substring(int, int)" />
        public static IStringGremlinQuery<string> Substring(this IGremlinQueryBase<string> query, int startIndex, int length)
        {
            ArgumentNullException.ThrowIfNull(query);

            return query
                .AsAdmin()
                .ChangeQueryType<IStringGremlinQuery<string>>()
                .Substring(startIndex, length);
        }

        /// <inheritdoc cref="IStringGremlinQuery{TString}.Substring(Range)" />
        public static IStringGremlinQuery<string> Substring(this IGremlinQueryBase<string> query, Range range)
        {
            ArgumentNullException.ThrowIfNull(query);

            return query
                .AsAdmin()
                .ChangeQueryType<IStringGremlinQuery<string>>()
                .Substring(range);
        }

        /// <inheritdoc cref="IStringGremlinQuery{TString}.ToLower" />
        public static IStringGremlinQuery<string> ToLower(this IGremlinQueryBase<string> query)
        {
            ArgumentNullException.ThrowIfNull(query);

            return query
                .AsAdmin()
                .ChangeQueryType<IStringGremlinQuery<string>>()
                .ToLower();
        }

        /// <inheritdoc cref="IStringGremlinQuery{TString}.ToUpper" />
        public static IStringGremlinQuery<string> ToUpper(this IGremlinQueryBase<string> query)
        {
            ArgumentNullException.ThrowIfNull(query);

            return query
                .AsAdmin()
                .ChangeQueryType<IStringGremlinQuery<string>>()
                .ToUpper();
        }

        /// <inheritdoc cref="IStringGremlinQuery{TString}.Trim" />
        public static IStringGremlinQuery<string> Trim(this IGremlinQueryBase<string> query)
        {
            ArgumentNullException.ThrowIfNull(query);

            return query
                .AsAdmin()
                .ChangeQueryType<IStringGremlinQuery<string>>()
                .Trim();
        }

        /// <inheritdoc cref="IStringGremlinQuery{TString}.TrimStart" />
        public static IStringGremlinQuery<string> TrimStart(this IGremlinQueryBase<string> query)
        {
            ArgumentNullException.ThrowIfNull(query);

            return query
                .AsAdmin()
                .ChangeQueryType<IStringGremlinQuery<string>>()
                .TrimStart();
        }

        /// <inheritdoc cref="IStringGremlinQuery{TString}.TrimEnd" />
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
