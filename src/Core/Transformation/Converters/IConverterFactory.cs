namespace ExRam.Gremlinq.Core.Transformation
{
    /// <summary>
    /// A factory that creates <see cref="IConverter{TSource, TTarget}"/> instances for specific source and target type pairs.
    /// </summary>
    public interface IConverterFactory
    {
        /// <summary>
        /// Attempts to create a converter for the specified source and target types.
        /// </summary>
        /// <typeparam name="TSource">The source type.</typeparam>
        /// <typeparam name="TTarget">The target type.</typeparam>
        /// <param name="environment">The query environment.</param>
        /// <returns>A converter, or <c>null</c> if this factory cannot handle the specified types.</returns>
        IConverter<TSource, TTarget>? TryCreate<TSource, TTarget>(IGremlinQueryEnvironment environment);
    }
}
