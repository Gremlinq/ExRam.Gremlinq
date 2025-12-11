namespace ExRam.Gremlinq.Core
{
    /// <summary>
    /// Represents a query for string values with string manipulation operations.
    /// </summary>
    /// <typeparam name="TString">The string type.</typeparam>
    public interface IStringGremlinQuery<TString> : IGremlinQueryBaseRec<TString, IStringGremlinQuery<TString>>
    {
        /// <summary>
        /// Concatenates the specified strings to the string values.
        /// </summary>
        /// <param name="strings">The strings to concatenate.</param>
        /// <returns>The query with concatenated strings.</returns>
        IStringGremlinQuery<TString> Concat(params string[] strings);

        /// <summary>
        /// Concatenates the specified strings to the string values.
        /// </summary>
        /// <param name="strings">The strings to concatenate.</param>
        /// <returns>The query with concatenated strings.</returns>
        IStringGremlinQuery<TString> Concat(params ReadOnlySpan<string> strings);

        /// <summary>
        /// Concatenates strings produced by traversals to the string values.
        /// </summary>
        /// <param name="stringTraversals">Traversals that produce strings to concatenate.</param>
        /// <returns>The query with concatenated strings.</returns>
        IStringGremlinQuery<TString> Concat(params Func<IStringGremlinQuery<TString>, IGremlinQueryBase<TString>>[] stringTraversals);

        /// <summary>
        /// Concatenates strings produced by traversals to the string values.
        /// </summary>
        /// <param name="stringTraversals">Traversals that produce strings to concatenate.</param>
        /// <returns>The query with concatenated strings.</returns>
        IStringGremlinQuery<TString> Concat(params ReadOnlySpan<Func<IStringGremlinQuery<TString>, IGremlinQueryBase<TString>>> stringTraversals);

        /// <summary>
        /// Gets the length of the string values.
        /// </summary>
        /// <returns>A query that returns the string lengths.</returns>
        IGremlinQuery<int> Length();

        /// <summary>
        /// Replaces all occurrences of a specified string with another string.
        /// </summary>
        /// <param name="oldValue">The string to be replaced.</param>
        /// <param name="newValue">The string to replace all occurrences of oldValue.</param>
        /// <returns>The query with replaced strings.</returns>
        IStringGremlinQuery<TString> Replace(string oldValue, string newValue);

        /// <summary>
        /// Reverses the string values.
        /// </summary>
        /// <returns>The query with reversed strings.</returns>
        IStringGremlinQuery<TString> Reverse();

        /// <summary>
        /// Extracts a substring from each string value starting at the specified index.
        /// </summary>
        /// <param name="startIndex">The zero-based starting character position.</param>
        /// <returns>The query with substring results.</returns>
        IStringGremlinQuery<TString> Substring(int startIndex);

        /// <summary>
        /// Extracts a substring from each string value starting at the specified index with the specified length.
        /// </summary>
        /// <param name="startIndex">The zero-based starting character position.</param>
        /// <param name="length">The number of characters in the substring.</param>
        /// <returns>The query with substring results.</returns>
        IStringGremlinQuery<TString> Substring(int startIndex, int length);

        /// <summary>
        /// Extracts a substring from each string value using a range.
        /// </summary>
        /// <param name="range">The range of characters to extract.</param>
        /// <returns>The query with substring results.</returns>
        IStringGremlinQuery<TString> Substring(Range range);

        /// <summary>
        /// Converts the string values to lowercase.
        /// </summary>
        /// <returns>The query with lowercase strings.</returns>
        IStringGremlinQuery<TString> ToLower();

        /// <summary>
        /// Converts the string values to uppercase.
        /// </summary>
        /// <returns>The query with uppercase strings.</returns>
        IStringGremlinQuery<TString> ToUpper();

        /// <summary>
        /// Removes all leading and trailing white-space characters from the string values.
        /// </summary>
        /// <returns>The query with trimmed strings.</returns>
        IStringGremlinQuery<TString> Trim();

        /// <summary>
        /// Removes all leading white-space characters from the string values.
        /// </summary>
        /// <returns>The query with strings that have leading whitespace removed.</returns>
        IStringGremlinQuery<TString> TrimStart();

        /// <summary>
        /// Removes all trailing white-space characters from the string values.
        /// </summary>
        /// <returns>The query with strings that have trailing whitespace removed.</returns>
        IStringGremlinQuery<TString> TrimEnd();
    }
}
