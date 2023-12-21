using System.Diagnostics.CodeAnalysis;

namespace ExRam.Gremlinq.Core.Transformation
{
    public static class ConverterFactory
    {
        private sealed class ClassFuncConverterFactory<TStaticSource, TStaticTarget> : IConverterFactory
           where TStaticTarget : class
        {
            private sealed class ClassFuncConverter<TSource, TTarget> : IConverter<TSource, TTarget>
                where TTarget : class
            {
                private readonly IGremlinQueryEnvironment _environment;
                private readonly Func<TStaticSource, IGremlinQueryEnvironment, ITransformer, ITransformer, TTarget?> _func;

                public ClassFuncConverter(Func<TStaticSource, IGremlinQueryEnvironment, ITransformer, ITransformer, TStaticTarget?> func, IGremlinQueryEnvironment environment)
                {
                    _environment = environment;
                    _func = (Func<TStaticSource, IGremlinQueryEnvironment, ITransformer, ITransformer, TTarget?>)(object)func;
                }

                public bool TryConvert(TSource source, ITransformer defer, ITransformer recurse, [NotNullWhen(true)] out TTarget? value)
                {
                    if (source is TStaticSource staticSerialized && _func(staticSerialized, _environment, defer, recurse) is { } requested)
                    {
                        value = requested;

                        return true;
                    }

                    value = default;

                    return false;
                }
            }

            private readonly Func<TStaticSource, IGremlinQueryEnvironment, ITransformer, ITransformer, TStaticTarget?> _func;

            public ClassFuncConverterFactory(Func<TStaticSource, IGremlinQueryEnvironment, ITransformer, ITransformer, TStaticTarget?> func)
            {
                _func = func;
            }

            public IConverter<TSource, TTarget>? TryCreate<TSource, TTarget>(IGremlinQueryEnvironment environment)
            {
                return (typeof(TSource).IsAssignableFrom(typeof(TStaticSource)) || typeof(TStaticSource).IsAssignableFrom(typeof(TSource))) && typeof(TTarget).IsAssignableFrom(typeof(TStaticTarget))
                    ? (IConverter<TSource, TTarget>?)Activator.CreateInstance(typeof(ClassFuncConverter<,>).MakeGenericType(typeof(TStaticSource), typeof(TStaticTarget), typeof(TSource), typeof(TTarget)), _func, environment)
                    : null;
            }
        }

        private sealed class StructFuncConverterFactory<TStaticSource, TStaticTarget> : IConverterFactory
            where TStaticTarget : struct
        {
            private sealed class StructToClassFuncConverter<TSource, TTarget> : IConverter<TSource, TTarget>
                where TTarget : class
            {
                private readonly IGremlinQueryEnvironment _environment;
                private readonly Func<TStaticSource, IGremlinQueryEnvironment, ITransformer, ITransformer, TTarget?> _func;

                public StructToClassFuncConverter(Func<TStaticSource, IGremlinQueryEnvironment, ITransformer, ITransformer, TStaticTarget?> func, IGremlinQueryEnvironment environment)
                {
                    _environment = environment;
                    _func = (source, env, defer, recurse) => (TTarget?)(object?)func(source, env, defer, recurse);
                }

                public bool TryConvert(TSource source, ITransformer defer, ITransformer recurse, [NotNullWhen(true)] out TTarget? value)
                {
                    if (source is TStaticSource staticSource && _func(staticSource, _environment, defer, recurse) is { } requested)
                    {
                        value = requested;

                        return true;
                    }

                    value = default;

                    return false;
                }
            }

            private sealed class StructToStructFuncConverter<TSource> : IConverter<TSource, TStaticTarget>
            {
                private readonly IGremlinQueryEnvironment _environment;
                private readonly Func<TStaticSource, IGremlinQueryEnvironment, ITransformer, ITransformer, TStaticTarget?> _func;

                public StructToStructFuncConverter(Func<TStaticSource, IGremlinQueryEnvironment, ITransformer, ITransformer, TStaticTarget?> func, IGremlinQueryEnvironment environment)
                {
                    _func = func;
                    _environment = environment;
                }

                public bool TryConvert(TSource source, ITransformer defer, ITransformer recurse, out TStaticTarget value)
                {
                    if (source is TStaticSource staticSource && _func(staticSource, _environment, defer, recurse) is { } requested)
                    {
                        value = requested;

                        return true;
                    }

                    value = default;

                    return false;
                }
            }

            private readonly Func<TStaticSource, IGremlinQueryEnvironment, ITransformer, ITransformer, TStaticTarget?> _func;

            public StructFuncConverterFactory(Func<TStaticSource, IGremlinQueryEnvironment, ITransformer, ITransformer, TStaticTarget?> func)
            {
                _func = func;
            }

            public IConverter<TSource, TTarget>? TryCreate<TSource, TTarget>(IGremlinQueryEnvironment environment)
            {
                if ((typeof(TSource).IsAssignableFrom(typeof(TStaticSource)) || typeof(TStaticSource).IsAssignableFrom(typeof(TSource))) && typeof(TTarget).IsAssignableFrom(typeof(TStaticTarget)))
                {
                    if (typeof(TTarget).IsClass)
                        return (IConverter<TSource, TTarget>?)Activator.CreateInstance(typeof(StructToClassFuncConverter<,>).MakeGenericType(typeof(TStaticSource), typeof(TStaticTarget), typeof(TSource), typeof(TTarget)), _func, environment);

                    if (typeof(TStaticTarget) == typeof(TTarget))
                        return (IConverter<TSource, TTarget>)(object)new StructToStructFuncConverter<TSource>(_func, environment);

                    throw new NotSupportedException($"Unable to create an instance of {nameof(IConverter<TSource, TTarget>)} for {typeof(TSource).FullName} and {typeof(TTarget).FullName}.");
                }

                return null;
            }
        }

        private sealed class ChainConverterFactory<TStaticSource, TIntermediateSource, TStaticTarget> : IConverterFactory
        {
            private sealed class ChainConverter<TSource, TTarget> : IConverter<TSource, TTarget>
            {
                private readonly IGremlinQueryEnvironment _environment;

                public ChainConverter(IGremlinQueryEnvironment environment)
                {
                    _environment = environment;
                }

                public bool TryConvert(TSource source, ITransformer defer, ITransformer recurse, [NotNullWhen(true)] out TTarget? value)
                {
                    value = default;

                    if (recurse.TryTransform(source, _environment, out TIntermediateSource? intermediate))
                    {
                        if (recurse.TryTransform(intermediate, _environment, out value))
                        {
                            return true;
                        }
                    }

                    return false;
                }
            }

            public IConverter<TSource, TTarget>? TryCreate<TSource, TTarget>(IGremlinQueryEnvironment environment)
            {
                return (typeof(TStaticSource).IsAssignableFrom(typeof(TSource)) && (typeof(TTarget).IsAssignableFrom(typeof(TStaticTarget))))
                    ? (IConverter<TSource, TTarget>?)Activator.CreateInstance(typeof(ChainConverter<,>).MakeGenericType(typeof(TStaticSource), typeof(TIntermediateSource), typeof(TStaticTarget), typeof(TSource), typeof(TTarget)), environment)
                    : default;
            }
        }

        public static IConverterFactory Create<TStaticSource, TStaticTarget>(Func<TStaticSource, IGremlinQueryEnvironment, ITransformer, ITransformer, TStaticTarget?> func)
            where TStaticTarget : struct => new StructFuncConverterFactory<TStaticSource, TStaticTarget>(func);

        public static IConverterFactory Create<TStaticSource, TStaticTarget>(Func<TStaticSource, IGremlinQueryEnvironment, ITransformer, ITransformer, TStaticTarget?> func)
            where TStaticTarget : class => new ClassFuncConverterFactory<TStaticSource, TStaticTarget>(func);

        internal static IConverterFactory Chain<TStaticSource, TIntermediateSource, TStaticTarget>() => new ChainConverterFactory<TStaticSource, TIntermediateSource, TStaticTarget>();
    }
}
