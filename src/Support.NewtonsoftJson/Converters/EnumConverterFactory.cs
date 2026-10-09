using Newtonsoft.Json.Linq;
using System.Diagnostics.CodeAnalysis;
using ExRam.Gremlinq.Core.Transformation;
using ExRam.Gremlinq.Core;

namespace ExRam.Gremlinq.Support.NewtonsoftJson
{
    // Newtonsoft's serializer reads a number as an enum with Enum.ToObject, which does not ask
    // whether the number fits the type underlying the enum but cuts it to that size: 4294967297
    // used to be the One of an int enum, and -1 the 255 of a byte enum. So the number is read as
    // the underlying type first, by the converters that read every other integer and decline one
    // that does not fit. Only what they read is made the enum's, by Enum.ToObject, which keeps a
    // value the enum has no name for - -1 of an int enum say. A ulong enum gets its
    // 18446744073709551615 this way too, which the serializer declined, its token holding a
    // BigInteger. A name in a string is still left to the serializer.
    internal sealed class EnumConverterFactory : IConverterFactory
    {
        private sealed class EnumConverter<TTarget, TUnderlying> : IConverter<JValue, TTarget>
        {
            private readonly IGremlinQueryEnvironment _environment;

            public EnumConverter(IGremlinQueryEnvironment environment)
            {
                _environment = environment;
            }

            bool IConverter<JValue, TTarget>.TryConvert(JValue serialized, ITransformer defer, ITransformer recurse, [NotNullWhen(true)] out TTarget? value)
            {
                ArgumentNullException.ThrowIfNull(serialized);
                ArgumentNullException.ThrowIfNull(defer);
                ArgumentNullException.ThrowIfNull(recurse);

                if (serialized.Type == JTokenType.Integer && recurse.TryTransform(serialized, _environment, out TUnderlying? underlying))
                {
                    value = (TTarget)Enum.ToObject(typeof(TTarget), underlying);
                    return true;
                }

                value = default;
                return false;
            }
        }

        IConverter<TSource, TTarget>? IConverterFactory.TryCreate<TSource, TTarget>(IGremlinQueryEnvironment environment)
        {
            ArgumentNullException.ThrowIfNull(environment);

            return typeof(TSource) == typeof(JValue) && typeof(TTarget).IsEnum
                ? (IConverter<TSource, TTarget>?)Activator.CreateInstance(typeof(EnumConverter<,>).MakeGenericType(typeof(TTarget), Enum.GetUnderlyingType(typeof(TTarget))), environment)
                : null;
        }
    }
}
