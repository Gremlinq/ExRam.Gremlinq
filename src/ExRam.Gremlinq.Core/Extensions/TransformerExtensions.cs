namespace ExRam.Gremlinq.Core.Transformation
{
    public static class TransformerExtensions
    {
        public readonly struct TransformToBuilder<TTarget>
        {
            private readonly ITransformer _transformer;

            public TransformToBuilder(ITransformer transformer)
            {
                _transformer = transformer;
            }

            public TTarget From<TSource>(TSource source, IGremlinQueryEnvironment environment) => _transformer.TryTransform<TSource, TTarget>(source, environment, out var value)
                ? value
                : throw new InvalidCastException($"Cannot convert {typeof(TSource)} to {typeof(TTarget)}.");
        }

        public static TransformToBuilder<TTarget> TransformTo<TTarget>(this ITransformer transformer) => new(transformer);
    }
}
