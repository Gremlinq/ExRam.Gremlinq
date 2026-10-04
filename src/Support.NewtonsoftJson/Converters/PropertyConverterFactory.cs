using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;

using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.GraphElements;
using ExRam.Gremlinq.Core.Transformation;

using Newtonsoft.Json.Linq;

namespace ExRam.Gremlinq.Support.NewtonsoftJson
{
    // Reads a Property<T>, a VertexProperty<T> or a VertexProperty<T, TMeta> the caller asks for from
    // a JObject, or a type of the caller's own that derives from one and is made from its value. A
    // property never holds a null, and its constructors throw for one, so a property whose value
    // cannot be read - a null included - is declined before a constructor is called. Newtonsoft's
    // serializer used to build these, and it called the constructor with whatever it got. A member
    // is found under the name TinkerPop gives it, in lower case, and only without that under the
    // first name spelled another way.
    internal sealed class PropertyConverterFactory : IConverterFactory
    {
        private class PropertyConverter<TProperty, TValue> : IConverter<JObject, TProperty>
            where TProperty : Property<TValue>
        {
            private static readonly Action<Property, string?> KeySetter = typeof(Property)
                .GetProperty(nameof(Property.Key))!
                .SetMethod!
                .CreateDelegate<Action<Property, string?>>();

            private readonly Func<TValue, TProperty> _constructor;

            public PropertyConverter(IGremlinQueryEnvironment environment, ConstructorInfo constructor)
            {
                var valueParameter = Expression.Parameter(typeof(TValue));

                Environment = environment;
                _constructor = Expression
                    .Lambda<Func<TValue, TProperty>>(Expression.New(constructor, valueParameter), valueParameter)
                    .Compile();
            }

            public bool TryConvert(JObject source, ITransformer defer, ITransformer recurse, [NotNullWhen(true)] out TProperty? value)
            {
                ArgumentNullException.ThrowIfNull(source);
                ArgumentNullException.ThrowIfNull(defer);
                ArgumentNullException.ThrowIfNull(recurse);

                if (source.TryGetValue("value", StringComparison.OrdinalIgnoreCase, out var valueToken) && recurse.TryTransform<JToken, TValue>(valueToken, Environment, out var propertyValue))
                {
                    value = _constructor(propertyValue);

                    if (source.TryGetValue("key", StringComparison.OrdinalIgnoreCase, out var keyToken))
                        KeySetter(value, recurse.TryTransformTo<string>().From(keyToken, Environment));

                    SetMembers(value, source, recurse);

                    return true;
                }

                // Declined, used to throw: ArgumentNullException from the constructor of Property<TValue> for { "key": "k", "value": null } as a Property<string> and for { "id": 1, "label": "k", "value": null } as a VertexProperty<string>.
                value = null;
                return false;
            }

            protected virtual void SetMembers(TProperty property, JObject source, ITransformer recurse)
            {
            }

            protected IGremlinQueryEnvironment Environment { get; }
        }

        private sealed class VertexPropertyConverter<TProperty, TValue, TMeta> : PropertyConverter<TProperty, TValue>
            where TProperty : VertexProperty<TValue, TMeta>
        {
            private static readonly Action<VertexProperty<TValue, TMeta>, object?> IdSetter = typeof(VertexProperty<TValue, TMeta>)
                .GetProperty(nameof(VertexProperty<,>.Id))!
                .SetMethod!
                .CreateDelegate<Action<VertexProperty<TValue, TMeta>, object?>>();

            private static readonly Action<VertexProperty<TValue, TMeta>, string?> LabelSetter = typeof(VertexProperty<TValue, TMeta>)
                .GetProperty(nameof(VertexProperty<,>.Label))!
                .SetMethod!
                .CreateDelegate<Action<VertexProperty<TValue, TMeta>, string?>>();

            public VertexPropertyConverter(IGremlinQueryEnvironment environment, ConstructorInfo constructor) : base(environment, constructor)
            {
            }

            protected override void SetMembers(TProperty property, JObject source, ITransformer recurse)
            {
                if (source.TryGetValue("id", StringComparison.OrdinalIgnoreCase, out var idToken))
                    IdSetter(property, recurse.TryTransformTo<object>().From(idToken, Environment));

                if (source.TryGetValue("label", StringComparison.OrdinalIgnoreCase, out var labelToken))
                    LabelSetter(property, recurse.TryTransformTo<string>().From(labelToken, Environment));

                if (source.TryGetValue("properties", StringComparison.OrdinalIgnoreCase, out var propertiesToken) && recurse.TryTransform<JToken, TMeta>(propertiesToken, Environment, out var properties))
                    property.Properties = properties;
            }
        }

        IConverter<TSource, TTarget>? IConverterFactory.TryCreate<TSource, TTarget>(IGremlinQueryEnvironment environment)
        {
            ArgumentNullException.ThrowIfNull(environment);

            if (typeof(TSource) == typeof(JObject) && typeof(TTarget).TryGetGenericArgumentsOf(typeof(Property<>)) is [var valueType] && typeof(TTarget).GetConstructor([valueType]) is { } constructor)
            {
                var converterType = typeof(TTarget).TryGetGenericArgumentsOf(typeof(VertexProperty<,>)) is [_, var metaType]
                    ? typeof(VertexPropertyConverter<,,>).MakeGenericType(typeof(TTarget), valueType, metaType)
                    : typeof(PropertyConverter<,>).MakeGenericType(typeof(TTarget), valueType);

                return (IConverter<TSource, TTarget>?)Activator.CreateInstance(converterType, environment, constructor);
            }

            return null;
        }
    }
}
