using ExRam.Gremlinq.Core.Transformation;

namespace ExRam.Gremlinq.Core
{
    /// <summary>Extension methods for <see cref="ITransformer"/>.</summary>
    public static class TransformerClassExtensions
    {
        /// <summary>
        /// A builder that attempts to transform a source value to a target reference type.
        /// </summary>
        /// <typeparam name="TTarget">The target reference type.</typeparam>
        public readonly struct TryTransformToBuilder<TTarget>
            where TTarget : class
        {
            private readonly ITransformer _transformer;

            /// <summary>
            /// Initializes a new instance of <see cref="TryTransformToBuilder{TTarget}"/>.
            /// </summary>
            /// <param name="transformer">The transformer to use.</param>
            public TryTransformToBuilder(ITransformer transformer)
            {
                ArgumentNullException.ThrowIfNull(transformer);

                _transformer = transformer;
            }

            /// <summary>
            /// Attempts the transformation from a given source.
            /// </summary>
            /// <typeparam name="TSource">The type of the source value.</typeparam>
            /// <param name="source">The source value.</param>
            /// <param name="environment">The query environment.</param>
            /// <returns>The transformed value, or <c>null</c> if the transformation is not supported.</returns>
            public TTarget? From<TSource>(TSource source, IGremlinQueryEnvironment environment)
            {
                ArgumentNullException.ThrowIfNull(environment);

                return _transformer.TryTransform<TSource, TTarget>(source, environment, out var value)
                    ? value
                    : null;
            }
        }

        /// <summary>
        /// Creates a builder for attempting a transformation to the specified target type.
        /// </summary>
        /// <typeparam name="TTarget">The target reference type.</typeparam>
        /// <param name="transformer">The transformer to use.</param>
        public static TryTransformToBuilder<TTarget> TryTransformTo<TTarget>(this ITransformer transformer)
            where TTarget : class
        {
            ArgumentNullException.ThrowIfNull(transformer);

            return new(transformer);
        }
    }
}
