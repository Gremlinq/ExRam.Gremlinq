using System.Buffers;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace ExRam.Gremlinq.Core.Transformation
{
    /// <summary>
    /// Provides factory members for creating <see cref="ITransformer"/> instances.
    /// </summary>
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

                bool IConverter<TStaticSource, TTarget>.TryConvert(TStaticSource source, ITransformer defer, ITransformer recurse, [NotNullWhen(true)] out TTarget? value)
                {
                    ArgumentNullException.ThrowIfNull(defer);
                    ArgumentNullException.ThrowIfNull(recurse);

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
            private TransformerImpl?[]? _deferTransformers;
            private readonly FastImmutableList<IConverterFactory> _converterFactories;
            private readonly ConcurrentDictionary<ConverterKey, object> _unifiedConverters = new();

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
                    // The key fixes the unified converter's type: UnifiedConverter<TSource, TActualSource, TTarget>.
                    var unifiedConverter = Unsafe.As<IConverter<TSource, TTarget>>(_unifiedConverters
                        .GetOrAdd(
                            new ConverterKey(environment, typeof(TSource), actualSerialized.GetType(), typeof(TTarget)),
                            static (key, @this) => GetConverterMethodInfo
                                .MakeGenericMethod(key.StaticSourceType, key.ActualSourceType, key.TargetType)
                                .Invoke(@this, [key.Environment])!,
                            this));

                    if (unifiedConverter.TryConvert(source, this, this, out var optionValue) && optionValue is not null)
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
                var tupleCount = 0;
                var deferTransformers = _deferTransformers;
                var arrayPool = ArrayPool<(IConverter<TActualSource, TTarget> converter, TransformerImpl overridden)>.Shared;

                var tuples = arrayPool
                    .Rent(_converterFactories.Count);

                try
                {
                    for (var i = _converterFactories.Count - 1; i >= 0; i--)
                    {
                        if (_converterFactories[i].TryCreate<TActualSource, TTarget>(environment) is { } converter)
                        {
                            TransformerImpl? deferTransformer;

                            while (true)
                            {
                                if (deferTransformers is { } existingDeferTransformers)
                                {
                                    if (existingDeferTransformers[i] is { } existingDeferTransformer)
                                        deferTransformer = existingDeferTransformer;
                                    else
                                    {
                                        deferTransformer = new TransformerImpl(_converterFactories[..i], _recurse);

                                        if (Interlocked.CompareExchange(ref deferTransformers[i], deferTransformer, null) is { } otherDeferTransformer)
                                            deferTransformer = otherDeferTransformer;
                                    }

                                    break;
                                }

                                deferTransformers = new TransformerImpl[_converterFactories.Count];

                                if (Interlocked.CompareExchange(ref _deferTransformers, deferTransformers, null) is { } otherDeferTransformers)
                                    deferTransformers = otherDeferTransformers;
                            }

                            tuples[tupleCount++] = (converter, deferTransformer);
                        }
                    }

                    return new UnifiedConverter<TStaticSource, TActualSource, TTarget>([.. tuples[..tupleCount]], _recurse);
                }
                finally
                {
                    arrayPool
                        .Return(tuples);
                }
            }
        }

        // The key of a unified converter. Its parts are compared by reference: an environment is the
        // instance the converters captured, and typeof and GetType() give one Type instance per type.
        internal readonly struct ConverterKey(IGremlinQueryEnvironment environment, Type staticSourceType, Type actualSourceType, Type targetType) : IEquatable<ConverterKey>
        {
            public bool Equals(ConverterKey other) => ReferenceEquals(Environment, other.Environment) && ReferenceEquals(StaticSourceType, other.StaticSourceType) && ReferenceEquals(ActualSourceType, other.ActualSourceType) && ReferenceEquals(TargetType, other.TargetType);

            public override bool Equals(object? obj) => obj is ConverterKey other && Equals(other);

            public override int GetHashCode() => HashCode.Combine(RuntimeHelpers.GetHashCode(Environment), RuntimeHelpers.GetHashCode(StaticSourceType), RuntimeHelpers.GetHashCode(ActualSourceType), RuntimeHelpers.GetHashCode(TargetType));

            public IGremlinQueryEnvironment Environment { get; } = environment;

            public Type StaticSourceType { get; } = staticSourceType;

            public Type ActualSourceType { get; } = actualSourceType;

            public Type TargetType { get; } = targetType;
        }

        /// <summary>
        /// An empty transformer with no converter factories registered.
        /// </summary>
        public static readonly ITransformer Empty = new TransformerImpl(FastImmutableList<IConverterFactory>.Empty);

        internal static readonly ITransformer SerializerEmpty = new TransformerImpl(FastImmutableList<IConverterFactory>.Empty.EnsureCapacity(160));

        internal static readonly ITransformer DeserializerEmpty = new TransformerImpl(FastImmutableList<IConverterFactory>.Empty.EnsureCapacity(32));
    }
}
