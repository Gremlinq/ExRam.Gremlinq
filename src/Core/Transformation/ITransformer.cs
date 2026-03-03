using System.Diagnostics.CodeAnalysis;

namespace ExRam.Gremlinq.Core.Transformation
{
    /// <summary>
    /// Transforms values from one type to another using a chain of <see cref="IConverterFactory"/> instances.
    /// Used for both serialization (query to bytecode) and deserialization (results to .NET types).
    /// </summary>
    public interface ITransformer
    {
        /// <summary>
        /// Attempts to transform the source value to the target type.
        /// </summary>
        /// <typeparam name="TSource">The type of the source value.</typeparam>
        /// <typeparam name="TTarget">The desired target type.</typeparam>
        /// <param name="source">The source value to transform.</param>
        /// <param name="environment">The query environment.</param>
        /// <param name="value">When this method returns <c>true</c>, contains the transformed value.</param>
        /// <returns><c>true</c> if the transformation succeeded; otherwise, <c>false</c>.</returns>
        bool TryTransform<TSource, TTarget>(TSource source, IGremlinQueryEnvironment environment, [NotNullWhen(true)] out TTarget? value);

        /// <summary>
        /// Returns a new transformer with the specified converter factory added to the chain.
        /// </summary>
        /// <param name="converterFactory">The converter factory to add.</param>
        ITransformer Add(IConverterFactory converterFactory);
    }
}
