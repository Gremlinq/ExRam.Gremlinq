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

                public bool TryConvert(TSource source, IGremlinQueryEnvironment environment, ITransformer recurse, out TStaticTarget value)
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

        private sealed class AutoRecurseConverterFactory<TStaticTarget> : IConverterFactory
        {
            private sealed class AutoRecurseConverter<TSource, TTarget> : IConverter<TSource, TTarget>
            {
                private readonly IConverter<TSource, TStaticTarget> _baseConverter;

                public AutoRecurseConverter(IConverter<TSource, TStaticTarget> baseConverter)
                {
                    _baseConverter = baseConverter;
                }

                public bool TryConvert(TSource source, IGremlinQueryEnvironment environment, ITransformer recurse, [NotNullWhen(true)] out TTarget? value)
                {
                    if (_baseConverter.TryConvert(source, environment, recurse, out var staticTargetValue))
                    {
                        if (staticTargetValue is TTarget targetValue)
                        {
                            value = targetValue;
                            return true;
                        }

                        return recurse.TryTransform(staticTargetValue, environment, out value);
                    }

                    value = default;
                    return false;
                }
            }

            private readonly IConverterFactory _baseFactory;

            public AutoRecurseConverterFactory(IConverterFactory baseFactory)
            {
                _baseFactory = baseFactory;
            }

            public IConverter<TSource, TTarget>? TryCreate<TSource, TTarget>()
            {
                return _baseFactory.TryCreate<TSource, TStaticTarget>() is { } baseConverter
                    ? new AutoRecurseConverter<TSource, TTarget>(baseConverter)
                    : default;
            }
        }

        private sealed class FinallyConverterFactory : IConverterFactory
        {
            private sealed class FinallyConverter<TSource, TTarget> : IConverter<TSource, TTarget>
            {
                private readonly Action _finallyAction;
                private readonly IConverter<TSource, TTarget> _baseConverter;

                public FinallyConverter(IConverter<TSource, TTarget> baseConverter, Action finallyAction)
                {
                    _baseConverter = baseConverter;
                    _finallyAction = finallyAction;
                }

                public bool TryConvert(TSource source, IGremlinQueryEnvironment environment, ITransformer recurse, [NotNullWhen(true)] out TTarget? value)
                {
                    try
                    {
                        return _baseConverter.TryConvert(source, environment, recurse, out value);
                    }
                    finally
                    {
                        _finallyAction();
                    }
                }
            }

            private readonly Action _finallyAction;
            private readonly IConverterFactory _baseConverterFactory;

            public FinallyConverterFactory(IConverterFactory baseConverterFactory, Action finallyAction)
            {
                _finallyAction = finallyAction;
                _baseConverterFactory = baseConverterFactory;
            }

            public IConverter<TSource, TTarget>? TryCreate<TSource, TTarget>()
            {
                return _baseConverterFactory.TryCreate<TSource, TTarget>() is { } converter
                    ? new FinallyConverter<TSource, TTarget>(converter, _finallyAction)
                    : default;
            }
        }

        private sealed class GuardConverterFactory<TStaticSource> : IConverterFactory
        {
            private sealed class GuardConverter<TSource, TTarget> : IConverter<TSource, TTarget>
            {
                private readonly Action<TStaticSource> _filter;

                public GuardConverter(Action<TStaticSource> filter)
                {
                    _filter = filter;
                }

                public bool TryConvert(TSource source, IGremlinQueryEnvironment environment, ITransformer recurse, [NotNullWhen(true)] out TTarget? value)
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

            public IConverter<TSource, TTarget>? TryCreate<TSource, TTarget>() => typeof(TStaticSource).IsAssignableFrom(typeof(TSource)) || typeof(TSource).IsAssignableFrom(typeof(TStaticSource))
                ? new GuardConverter<TSource, TTarget>(_filter)
                : null;
        }

        public static IConverterFactory Create<TStaticSource, TStaticTarget>(Func<TStaticSource, IGremlinQueryEnvironment, ITransformer, TStaticTarget?> func)
            where TStaticTarget : struct => new StructFuncConverterFactory<TStaticSource, TStaticTarget>(func);

        public static IConverterFactory Create<TStaticSource, TStaticTarget>(Func<TStaticSource, IGremlinQueryEnvironment, ITransformer, TStaticTarget?> func)
            where TStaticTarget : class => new ClassFuncConverterFactory<TStaticSource, TStaticTarget>(func);

        public static IConverterFactory Guard<TStaticSource>(Action<TStaticSource> guard) => new GuardConverterFactory<TStaticSource>(guard);

        public static IConverterFactory AutoRecurse<TStaticTarget>(this IConverterFactory baseFactory) => new AutoRecurseConverterFactory<TStaticTarget>(baseFactory);

        public static IConverterFactory Finally(this IConverterFactory factory, Action finallyAction) => new FinallyConverterFactory(factory, finallyAction);
    }
}
