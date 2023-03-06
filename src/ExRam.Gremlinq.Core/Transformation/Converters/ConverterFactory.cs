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
                private readonly Func<TStaticSource, IGremlinQueryEnvironment, ITransformer, TTarget?> _func;

                public ClassFuncConverter(Func<TStaticSource, IGremlinQueryEnvironment, ITransformer, TStaticTarget?> func)
                {
                    _func = (Func<TStaticSource, IGremlinQueryEnvironment, ITransformer, TTarget?>)(object)func;
                }

                public bool TryConvert(TSource source, IGremlinQueryEnvironment environment, ITransformer recurse, [NotNullWhen(true)] out TTarget? value)
                {
                    if (source is TStaticSource staticSerialized && _func(staticSerialized, environment, recurse) is { } requested)
                    {
                        value = requested;

                        return true;
                    }

                    value = default;

                    return false;
                }
            }

            private readonly Func<TStaticSource, IGremlinQueryEnvironment, ITransformer, TStaticTarget?> _func;

            public ClassFuncConverterFactory(Func<TStaticSource, IGremlinQueryEnvironment, ITransformer, TStaticTarget?> func)
            {
                _func = func;
            }

            public IConverter<TSource, TTarget>? TryCreate<TSource, TTarget>()
            {
                return (typeof(TSource).IsAssignableFrom(typeof(TStaticSource)) || typeof(TStaticSource).IsAssignableFrom(typeof(TSource))) && typeof(TTarget).IsAssignableFrom(typeof(TStaticTarget))
                    ? (IConverter<TSource, TTarget>?)Activator.CreateInstance(typeof(ClassFuncConverter<,>).MakeGenericType(typeof(TStaticSource), typeof(TStaticTarget), typeof(TSource), typeof(TTarget)), _func)
                    : null;
            }
        }

        private sealed class StructFuncConverterFactory<TStaticSource, TStaticTarget> : IConverterFactory
            where TStaticTarget : struct
        {
            private sealed class StructToClassFuncConverter<TSource, TTarget> : IConverter<TSource, TTarget>
                where TTarget : class
            {
                private readonly Func<TStaticSource, IGremlinQueryEnvironment, ITransformer, TTarget?> _func;

                public StructToClassFuncConverter(Func<TStaticSource, IGremlinQueryEnvironment, ITransformer, TStaticTarget?> func)
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

            private sealed class StructToStructFuncConverter<TSource> : IConverter<TSource, TStaticTarget>
            {
                private readonly Func<TStaticSource, IGremlinQueryEnvironment, ITransformer, TStaticTarget?> _func;

                public StructToStructFuncConverter(Func<TStaticSource, IGremlinQueryEnvironment, ITransformer, TStaticTarget?> func)
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

            public StructFuncConverterFactory(Func<TStaticSource, IGremlinQueryEnvironment, ITransformer, TStaticTarget?> func)
            {
                _func = func;
            }

            public IConverter<TSource, TTarget>? TryCreate<TSource, TTarget>()
            {
                if ((typeof(TSource).IsAssignableFrom(typeof(TStaticSource)) || typeof(TStaticSource).IsAssignableFrom(typeof(TSource))) && typeof(TTarget).IsAssignableFrom(typeof(TStaticTarget)))
                {
                    if (typeof(TTarget).IsClass)
                        return (IConverter<TSource, TTarget>?)Activator.CreateInstance(typeof(StructToClassFuncConverter<,>).MakeGenericType(typeof(TStaticSource), typeof(TStaticTarget), typeof(TSource), typeof(TTarget)), _func);

                    if (typeof(TStaticTarget) == typeof(TTarget))
                        return (IConverter<TSource, TTarget>)(object)new StructToStructFuncConverter<TSource>(_func);

                    throw new NotSupportedException();
                }

                return null;
            }
        }

        public static IConverterFactory Create<TSource, TTarget>(Func<TSource, IGremlinQueryEnvironment, ITransformer, TTarget?> func)
            where TTarget : struct => new StructFuncConverterFactory<TSource, TTarget>(func);

        public static IConverterFactory Create<TSource, TTarget>(Func<TSource, IGremlinQueryEnvironment, ITransformer, TTarget?> func)
            where TTarget : class => new ClassFuncConverterFactory<TSource, TTarget>(func);
    }
}
