using ExRam.Gremlinq.Core.Transformation;

namespace ExRam.Gremlinq.Core
{
    public static class TransformerExtensions
    {
        public readonly struct TransformToBuilder<TTarget>
        {
            private readonly ITransformer _transformer;

            public TransformToBuilder(ITransformer transformer)
            {
                ArgumentNullException.ThrowIfNull(transformer);

                _transformer = transformer;
            }

            public TTarget From<TSource>(TSource source, IGremlinQueryEnvironment environment)
            {
                ArgumentNullException.ThrowIfNull(environment);

                return _transformer.TryTransform<TSource, TTarget>(source, environment, out var value)
                    ? value
                    : throw new InvalidCastException($"Cannot convert {source?.GetType() ?? typeof(TSource)} to {typeof(TTarget)}.");
            }
        }

        public static TransformToBuilder<TTarget> TransformTo<TTarget>(this ITransformer transformer)
        {
            ArgumentNullException.ThrowIfNull(transformer);

            return new(transformer);
        }
    }
}
