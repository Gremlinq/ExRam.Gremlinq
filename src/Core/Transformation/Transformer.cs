using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace ExRam.Gremlinq.Core.Transformation
{
    public static class Transformer
    {
        private sealed class TransformerImpl : ITransformer
        {
            private sealed class UnifiedConverter<TStaticSource, TActualSource, TTarget> : IConverter<TStaticSource, TTarget>
                where TActualSource : TStaticSource
            {
                private readonly TransformerImpl _recurse;
                private readonly (IConverter<TActualSource, TTarget> converter, TransformerImpl overridden)[] _converters;

                public UnifiedConverter((IConverter<TActualSource, TTarget> converter, TransformerImpl overridden)[] converters, TransformerImpl recurse)
                {
                    _recurse = recurse;
                    _converters = converters;
                }

                public bool TryConvert(TStaticSource source, ITransformer _, ITransformer __, [NotNullWhen(true)] out TTarget? value)
                {
                    if (source is TActualSource actualSource)
                    {
                        foreach (var converter in _converters)
                        {
                            if (converter.converter.TryConvert(actualSource, converter.overridden, _recurse, out value))
                                return true;
                        }
                    }

                    value = default;
                    return false;
                }
            }

            private static readonly MethodInfo GetConverterMethodInfo = typeof(TransformerImpl).GetMethod(nameof(GetUnifiedConverter), BindingFlags.Instance | BindingFlags.NonPublic)!;

            private readonly TransformerImpl _recurse;
            private readonly FastImmutableList<IConverterFactory> _converterFactories;
            private readonly ConcurrentDictionary<(IGremlinQueryEnvironment, Type, Type, Type), object> _unifiedConverters = new();

            public TransformerImpl(FastImmutableList<IConverterFactory> converterFactories)
            {
                _recurse = this;
                _converterFactories = converterFactories;
            }

            private TransformerImpl(FastImmutableList<IConverterFactory> converterFactories, TransformerImpl recurse) : this(converterFactories)
            {
                _recurse = recurse;
            }

            public ITransformer Add(IConverterFactory converterFactory) => new TransformerImpl(_converterFactories.Push(converterFactory));

            public bool TryTransform<TSource, TTarget>(TSource source, IGremlinQueryEnvironment environment, [NotNullWhen(true)] out TTarget? value)
            {
                if (source is { } actualSerialized)
                {
                    var maybeUnifiedConverter = _unifiedConverters
                        .GetOrAdd(
                            (environment, typeof(TSource), actualSerialized.GetType(), typeof(TTarget)),
                            static (typeTuple, @this) =>
                            {
                                var (environment, staticSerializedType, actualSerializedType, requestedType) = typeTuple;

                                return GetConverterMethodInfo
                                    .MakeGenericMethod(staticSerializedType, actualSerializedType, requestedType)
                                    .Invoke(@this, [environment])!;
                            },
                            this);

                    if ((maybeUnifiedConverter as IConverter<TSource, TTarget>)?.TryConvert(source, this, this, out var optionValue) is true && optionValue is not null)
                    {
                        value = optionValue;
                        return true;
                    }

                    if (source is TTarget target)
                    {
                        value = target;
                        return true;
                    }
                }

                value = default;
                return false;
            }

            private IConverter<TStaticSource, TTarget> GetUnifiedConverter<TStaticSource, TActualSource, TTarget>(IGremlinQueryEnvironment environment)
                where TActualSource : TStaticSource
            {
                var list = new List<(IConverter<TActualSource, TTarget> converter, TransformerImpl overridden)>();

                for (var i = _converterFactories.Count - 1; i >= 0; i--)
                {
                    if (_converterFactories[i].TryCreate<TActualSource, TTarget>(environment) is { } converter)
                        list.Add((converter, new TransformerImpl(_converterFactories[0..i], this)));
                }

                return new UnifiedConverter<TStaticSource, TActualSource, TTarget>([.. list], _recurse);
            }
        }

        public static readonly ITransformer Empty = new TransformerImpl(FastImmutableList<IConverterFactory>.Empty);

        internal static readonly ITransformer SerializerEmpty = new TransformerImpl(FastImmutableList<IConverterFactory>.Empty.EnsureCapacity(160));

        internal static readonly ITransformer DeserializerEmpty = new TransformerImpl(FastImmutableList<IConverterFactory>.Empty.EnsureCapacity(32));
    }
}
