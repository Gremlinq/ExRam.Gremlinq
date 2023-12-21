using System.Diagnostics.CodeAnalysis;

using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Transformation;

namespace ExRam.Gremlinq.Providers.CosmosDb
{
    internal static class TransformerExtensions
    {
        private sealed class GuardConverterFactory<TStaticSource> : IConverterFactory
        {
            private sealed class GuardConverter<TSource, TTarget> : IConverter<TSource, TTarget>
            {
                private readonly Action<TStaticSource> _filter;

                public GuardConverter(Action<TStaticSource> filter)
                {
                    _filter = filter;
                }

                public bool TryConvert(TSource source, ITransformer defer, ITransformer recurse, [NotNullWhen(true)] out TTarget? value)
                {
                    if (source is TStaticSource staticSource)
                        _filter(staticSource);

                    value = default;
                    return false;
                }
            }

            private readonly Action<TStaticSource> _filter;

            public GuardConverterFactory(Action<TStaticSource> filter)
            {
                _filter = filter;
            }

            public IConverter<TSource, TTarget>? TryCreate<TSource, TTarget>(IGremlinQueryEnvironment environment) => typeof(TStaticSource).IsAssignableFrom(typeof(TSource)) || typeof(TSource).IsAssignableFrom(typeof(TStaticSource))
                ? new GuardConverter<TSource, TTarget>(_filter)
                : null;
        }

        public static ITransformer Guard<TStaticSource>(this ITransformer transformer, Action<TStaticSource> guard) => transformer
            .Add(new GuardConverterFactory<TStaticSource>(guard));
    }
}
