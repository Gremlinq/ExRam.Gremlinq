using System.Diagnostics.CodeAnalysis;

namespace ExRam.Gremlinq.Core.Transformation
{
    /// <summary>
    /// Converts values of type <typeparamref name="TSource"/> to <typeparamref name="TTarget"/>.
    /// </summary>
    /// <typeparam name="TSource">The source type.</typeparam>
    /// <typeparam name="TTarget">The target type.</typeparam>
    public interface IConverter<in TSource, TTarget>
    {
        /// <summary>
        /// Attempts to convert the source value to the target type.
        /// </summary>
        /// <param name="source">The source value.</param>
        /// <param name="defer">A transformer that delegates to the previous converters in the chain.</param>
        /// <param name="recurse">A transformer that delegates to all converters in the chain.</param>
        /// <param name="value">When this method returns <c>true</c>, contains the converted value.</param>
        /// <returns><c>true</c> if the conversion succeeded; otherwise, <c>false</c>.</returns>
        bool TryConvert(TSource source, ITransformer defer, ITransformer recurse, [NotNullWhen(true)] out TTarget? value);
    }
}
