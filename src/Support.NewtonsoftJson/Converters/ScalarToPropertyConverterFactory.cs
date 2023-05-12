using Newtonsoft.Json.Linq;
using ExRam.Gremlinq.Core.GraphElements;
using System.Diagnostics.CodeAnalysis;
using ExRam.Gremlinq.Core.Transformation;
using ExRam.Gremlinq.Core;

namespace ExRam.Gremlinq.Support.NewtonsoftJson
{
    internal sealed class ScalarToPropertyConverterFactory : IConverterFactory
    {
        private sealed class ScalarToPropertyConverter<TTargetProperty, TTargetPropertyValue> : IConverter<JValue, TTargetProperty>
            where TTargetProperty : Property
        {
            private readonly IGremlinQueryEnvironment _environment;
            private readonly Func<TTargetPropertyValue, TTargetProperty?> _constructor;

            public ScalarToPropertyConverter(IGremlinQueryEnvironment environment)
            {
                if (typeof(TTargetProperty).GetConstructor(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public, new[] { typeof(TTargetPropertyValue) }) is { } constructor)
                {
                    _environment = environment;
                    _constructor = value => constructor.Invoke(new object?[] { value }) as TTargetProperty; //TODO: Create Func generically
                }
                else
                    throw new ArgumentException();
            }

            public bool TryConvert(JValue serialized, ITransformer recurse, [NotNullWhen(true)] out TTargetProperty? value)
            {
                if (recurse.TryTransform<JValue, TTargetPropertyValue>(serialized, _environment, out var propertyValue))
                {
                    if (_constructor(propertyValue) is { } requestedProperty)
                    {
                        value = requestedProperty;
                        return true;
                    }
                }

                value = default;
                return false;
            }
        }

        public IConverter<TSource, TTarget>? TryCreate<TSource, TTarget>(IGremlinQueryEnvironment environment) => typeof(TSource) == typeof(JValue) && typeof(Property).IsAssignableFrom(typeof(TTarget)) && typeof(TTarget).IsGenericType
            ? (IConverter<TSource, TTarget>?)Activator.CreateInstance(typeof(ScalarToPropertyConverter<,>).MakeGenericType(typeof(TTarget), typeof(TTarget).GetGenericArguments()[0]), environment)
            : default;
    }
}
