using ExRam.Gremlinq.Core.Transformation;

namespace ExRam.Gremlinq.Core
{
    public static class TransformerStructExtensions
    {
        public readonly struct TryTransformToBuilder<TTarget>
            where TTarget : struct
        {
            private readonly ITransformer _transformer;

            public TryTransformToBuilder(ITransformer transformer)
            {
                _transformer = transformer;
            }

            public TTarget? From<TSource>(TSource source, IGremlinQueryEnvironment environment) => _transformer.TryTransform<TSource, TTarget>(source, environment, out var value)
                ? value
                : default(TTarget?);
        }

        public static TryTransformToBuilder<TTarget> TryTransformTo<TTarget>(this ITransformer transformer)
            where TTarget : struct => new(transformer);
    }
}
