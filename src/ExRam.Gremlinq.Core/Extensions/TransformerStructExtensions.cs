using System.Diagnostics.CodeAnalysis;

namespace ExRam.Gremlinq.Core.Transformation
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

        private sealed class FixedTypeConverterFactory<TStaticSource, TStaticTarget> : IConverterFactory
            where TStaticTarget : struct
        {
            private sealed class FixedTypeToClassConverter<TSource, TTarget> : IConverter<TSource, TTarget>
                where TTarget : class
            {
                private readonly Func<TStaticSource, IGremlinQueryEnvironment, ITransformer, TTarget?> _func;

                public FixedTypeToClassConverter(Func<TStaticSource, IGremlinQueryEnvironment, ITransformer, TStaticTarget?> func)
                {
                    _func = (source, env, recurse) => (TTarget?)(object?)func(source, env, recurse);
                }

                public bool TryConvert(TSource source, IGremlinQueryEnvironment environment, ITransformer recurse, [NotNullWhen(true)] out TTarget? value)
                {
                    if (source is TStaticSource staticSource && _func(staticSource, environment, recurse) is { } requested)
                    {
                        value = requested;

                        return true;
                    }

                    value = default;

                    return false;
                }
            }

            private sealed class FixedTypeToStructConverter<TSource> : IConverter<TSource, TStaticTarget>
            {
                private readonly Func<TStaticSource, IGremlinQueryEnvironment, ITransformer, TStaticTarget?> _func;

                public FixedTypeToStructConverter(Func<TStaticSource, IGremlinQueryEnvironment, ITransformer, TStaticTarget?> func)
                {
                    _func = func;
                }

                public bool TryConvert(TSource source, IGremlinQueryEnvironment environment, ITransformer recurse, [NotNullWhen(true)] out TStaticTarget value)
                {
                    if (source is TStaticSource staticSource && _func(staticSource, environment, recurse) is { } requested)
                    {
                        value = requested;

                        return true;
                    }

                    value = default;

                    return false;
                }
            }

            private readonly Func<TStaticSource, IGremlinQueryEnvironment, ITransformer, TStaticTarget?> _func;

            public FixedTypeConverterFactory(Func<TStaticSource, IGremlinQueryEnvironment, ITransformer, TStaticTarget?> func)
            {
                _func = func;
            }

            public IConverter<TSource, TTarget>? TryCreate<TSource, TTarget>()
            {
                if ((typeof(TSource).IsAssignableFrom(typeof(TStaticSource)) || typeof(TStaticSource).IsAssignableFrom(typeof(TSource))) && typeof(TTarget).IsAssignableFrom(typeof(TStaticTarget)))
                {
                    if (typeof(TTarget).IsClass)
                        return (IConverter<TSource, TTarget>?)Activator.CreateInstance(typeof(FixedTypeToClassConverter<,>).MakeGenericType(typeof(TStaticSource), typeof(TStaticTarget), typeof(TSource), typeof(TTarget)), _func);

                    if (typeof(TStaticTarget) == typeof(TTarget))
                        return (IConverter<TSource, TTarget>)(object)new FixedTypeToStructConverter<TSource>(_func);

                    throw new NotSupportedException();
                }

                return null;
            }
        }

        public static TryTransformToBuilder<TTarget> TryTransformTo<TTarget>(this ITransformer transformer)
            where TTarget : struct => new(transformer);

        public static ITransformer Add<TSource, TTarget>(this ITransformer transformer, Func<TSource, IGremlinQueryEnvironment, ITransformer, TTarget?> func)
            where TTarget : struct => transformer.Add(new FixedTypeConverterFactory<TSource, TTarget>(func));
    }
}
