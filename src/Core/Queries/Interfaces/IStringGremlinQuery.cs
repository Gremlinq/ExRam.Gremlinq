namespace ExRam.Gremlinq.Core
{
    public interface IStringGremlinQuery<TString> : IGremlinQueryBaseRec<TString, IStringGremlinQuery<TString>>
    {
        /// <summary>
        /// Concatenates the specified string values to the incoming string traverser.
        /// Corresponds to the Gremlin <c>concat()</c> step.
        /// </summary>
        /// <param name="strings">The string values to concatenate.</param>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#concat-step">Reference Documentation - Concat Step</seealso>
        IStringGremlinQuery<TString> Concat(params string[] strings);

        /// <inheritdoc cref="Concat(string[])" />
        IStringGremlinQuery<TString> Concat(params ReadOnlySpan<string> strings);

        /// <summary>
        /// Concatenates the results of the specified string traversals to the incoming string traverser.
        /// Corresponds to the Gremlin <c>concat()</c> step.
        /// </summary>
        /// <param name="stringTraversals">The traversals producing string values to concatenate.</param>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#concat-step">Reference Documentation - Concat Step</seealso>
        IStringGremlinQuery<TString> Concat(params Func<IStringGremlinQuery<TString>, IGremlinQueryBase<TString>>[] stringTraversals);

        /// <inheritdoc cref="Concat(Func{IStringGremlinQuery{TString}, IGremlinQueryBase{TString}}[])" />
        IStringGremlinQuery<TString> Concat(params ReadOnlySpan<Func<IStringGremlinQuery<TString>, IGremlinQueryBase<TString>>> stringTraversals);

        /// <summary>
        /// Returns the length of the incoming string traverser.
        /// Corresponds to the Gremlin <c>length()</c> step.
        /// </summary>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#length-step">Reference Documentation - Length Step</seealso>
        IGremlinQuery<int> Length();

        /// <summary>
        /// Replaces all occurrences of <paramref name="oldValue"/> with <paramref name="newValue"/> in the incoming string.
        /// Corresponds to the Gremlin <c>replace()</c> step.
        /// </summary>
        /// <param name="oldValue">The string to be replaced.</param>
        /// <param name="newValue">The replacement string.</param>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#replace-step">Reference Documentation - Replace Step</seealso>
        IStringGremlinQuery<TString> Replace(string oldValue, string newValue);

        /// <summary>
        /// Returns the reverse of the incoming string traverser.
        /// Corresponds to the Gremlin <c>reverse()</c> step.
        /// </summary>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#reverse-step">Reference Documentation - Reverse Step</seealso>
        IStringGremlinQuery<TString> Reverse();

        /// <summary>
        /// Returns a substring of the incoming string starting at <paramref name="startIndex"/> to the end.
        /// Corresponds to the Gremlin <c>substring()</c> step.
        /// </summary>
        /// <param name="startIndex">The zero-based start index (inclusive).</param>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#substring-step">Reference Documentation - Substring Step</seealso>
        IStringGremlinQuery<TString> Substring(int startIndex);

        /// <summary>
        /// Returns a substring of the incoming string starting at <paramref name="startIndex"/> with the given <paramref name="length"/>.
        /// Corresponds to the Gremlin <c>substring()</c> step.
        /// </summary>
        /// <param name="startIndex">The zero-based start index (inclusive).</param>
        /// <param name="length">The number of characters to extract.</param>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#substring-step">Reference Documentation - Substring Step</seealso>
        IStringGremlinQuery<TString> Substring(int startIndex, int length);

        /// <summary>
        /// Returns a substring of the incoming string for the specified <paramref name="range"/>.
        /// Corresponds to the Gremlin <c>substring()</c> step.
        /// </summary>
        /// <param name="range">The range of the substring to extract.</param>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#substring-step">Reference Documentation - Substring Step</seealso>
        IStringGremlinQuery<TString> Substring(Range range);

        /// <summary>
        /// Returns the lowercase representation of the incoming string.
        /// Corresponds to the Gremlin <c>toLower()</c> step.
        /// </summary>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#toLower-step">Reference Documentation - ToLower Step</seealso>
        IStringGremlinQuery<TString> ToLower();

        /// <summary>
        /// Returns the uppercase representation of the incoming string.
        /// Corresponds to the Gremlin <c>toUpper()</c> step.
        /// </summary>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#toUpper-step">Reference Documentation - ToUpper Step</seealso>
        IStringGremlinQuery<TString> ToUpper();

        /// <summary>
        /// Returns the string with leading and trailing whitespace removed.
        /// Corresponds to the Gremlin <c>trim()</c> step.
        /// </summary>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#trim-step">Reference Documentation - Trim Step</seealso>
        IStringGremlinQuery<TString> Trim();

        /// <summary>
        /// Returns the string with leading whitespace removed.
        /// Corresponds to the Gremlin <c>lTrim()</c> step.
        /// </summary>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#lTrim-step">Reference Documentation - LTrim Step</seealso>
        IStringGremlinQuery<TString> TrimStart();

        /// <summary>
        /// Returns the string with trailing whitespace removed.
        /// Corresponds to the Gremlin <c>rTrim()</c> step.
        /// </summary>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#rTrim-step">Reference Documentation - RTrim Step</seealso>
        IStringGremlinQuery<TString> TrimEnd();
    }
}
