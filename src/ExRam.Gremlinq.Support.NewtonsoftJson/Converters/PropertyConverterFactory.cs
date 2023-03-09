using Newtonsoft.Json.Linq;
using ExRam.Gremlinq.Core.GraphElements;
using System.Diagnostics.CodeAnalysis;
using ExRam.Gremlinq.Core.Transformation;
using ExRam.Gremlinq.Core;

namespace ExRam.Gremlinq.Support.NewtonsoftJson
{
    internal sealed class PropertyConverterFactory : IConverterFactory
    {
        private sealed class PropertyConverter<TTargetProperty, TTargetPropertyValue> : IConverter<JValue, TTargetProperty>
            where TTargetProperty : Property
        {
            public bool TryConvert(JValue serialized, IGremlinQueryEnvironment environment, ITransformer recurse, [NotNullWhen(true)] out TTargetProperty? value)
            {
                if (recurse.TryTransform<JValue, TTargetPropertyValue>(serialized, environment, out var propertyValue))
                    //TODO: Improvement opportunity.

                    if (Activator.CreateInstance(typeof(TTargetProperty), propertyValue) is TTargetProperty requestedProperty)
                    {
                        value = requestedProperty;
                        return true;
                    }

                value = default;
                return false;
            }
        }

        public IConverter<TSource, TTarget>? TryCreate<TSource, TTarget>()
        {
            return typeof(TSource) == typeof(JValue) && typeof(Property).IsAssignableFrom(typeof(TTarget)) && typeof(TTarget).IsGenericType
                ? (IConverter<TSource, TTarget>?)Activator.CreateInstance(typeof(PropertyConverter<,>).MakeGenericType(typeof(TTarget), typeof(TTarget).GetGenericArguments()[0]))
                : default;
        }
    }
}
