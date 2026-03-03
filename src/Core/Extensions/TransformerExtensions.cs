using ExRam.Gremlinq.Core.Transformation;

namespace ExRam.Gremlinq.Core
{
    /// <summary>
    /// Extension methods for <see cref="ITransformer"/> that provide guaranteed (throwing) transformations.
    /// </summary>
    public static class TransformerExtensions
    {
        /// <summary>
        /// A builder that performs a guaranteed transformation from a source to a target type, throwing on failure.
        /// </summary>
        /// <typeparam name="TTarget">The target type.</typeparam>
        public readonly struct TransformToBuilder<TTarget>
        {
            private readonly ITransformer _transformer;

            /// <summary>
            /// Initializes a new instance of <see cref="TransformToBuilder{TTarget}"/>.
            /// </summary>
            /// <param name="transformer">The transformer to use.</param>
            public TransformToBuilder(ITransformer transformer)
            {
                ArgumentNullException.ThrowIfNull(transformer);

                _transformer = transformer;
            }

            /// <summary>
            /// Transforms the given source value. Throws <see cref="InvalidCastException"/> on failure.
            /// </summary>
            /// <typeparam name="TSource">The source type.</typeparam>
            /// <param name="source">The source value.</param>
            /// <param name="environment">The query environment.</param>
            public TTarget From<TSource>(TSource source, IGremlinQueryEnvironment environment)
            {
                ArgumentNullException.ThrowIfNull(environment);

                return _transformer.TryTransform<TSource, TTarget>(source, environment, out var value)
                    ? value
                    : throw new InvalidCastException($"Cannot convert {source?.GetType() ?? typeof(TSource)} to {typeof(TTarget)}.");
            }
        }

        /// <summary>
        /// Creates a <see cref="TransformToBuilder{TTarget}"/> for performing guaranteed transformations.
        /// </summary>
        /// <typeparam name="TTarget">The target type.</typeparam>
        /// <param name="transformer">The transformer to use.</param>
        public static TransformToBuilder<TTarget> TransformTo<TTarget>(this ITransformer transformer)
        {
            ArgumentNullException.ThrowIfNull(transformer);

            return new(transformer);
        }
    }
}
