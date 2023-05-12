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

            public ScalarToPropertyConverter(IGremlinQueryEnvironment environment)
            {
                _environment = environment;
            }

            public bool TryConvert(JValue serialized, ITransformer recurse, [NotNullWhen(true)] out TTargetProperty? value)
            {
                if (recurse.TryTransform<JValue, TTargetPropertyValue>(serialized, _environment, out var propertyValue))
                {
                    //TODO: Improvement opportunity.
                    if (Activator.CreateInstance(typeof(TTargetProperty), propertyValue) is TTargetProperty requestedProperty)
                    {
                        value = requestedProperty;
                        return true;
                    }
                }

                value = default;
                return false;
            }
        }

        public IConverter<TSource, TTarget>? TryCreate<TSource, TTarget>(IGremlinQueryEnvironment environment)
        {
            return typeof(TSource) == typeof(JValue) && typeof(Property).IsAssignableFrom(typeof(TTarget)) && typeof(TTarget).IsGenericType
                ? (IConverter<TSource, TTarget>?)Activator.CreateInstance(typeof(ScalarToPropertyConverter<,>).MakeGenericType(typeof(TTarget), typeof(TTarget).GetGenericArguments()[0]), environment)
                : default;
        }
    }
}
