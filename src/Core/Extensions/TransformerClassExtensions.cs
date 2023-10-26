using ExRam.Gremlinq.Core.Transformation;

namespace ExRam.Gremlinq.Core
{
    public static class TransformerClassExtensions
    {
        public readonly struct TryTransformToBuilder<TTarget>
            where TTarget : class
        {
            private readonly ITransformer _transformer;

            public TryTransformToBuilder(ITransformer transformer)
            {
                _transformer = transformer;
            }

            public TTarget? From<TSource>(TSource source, IGremlinQueryEnvironment environment) => _transformer.TryTransform<TSource, TTarget>(source, environment, out var value)
                ? value
                : default;
        }

        public static TryTransformToBuilder<TTarget> TryTransformTo<TTarget>(this ITransformer transformer)
            where TTarget : class => new(transformer);
    }
}
