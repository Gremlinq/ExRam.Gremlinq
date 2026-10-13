using Newtonsoft.Json.Linq;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using ExRam.Gremlinq.Core.Transformation;
using ExRam.Gremlinq.Core;

namespace ExRam.Gremlinq.Support.NewtonsoftJson
{
    // What a value is when an object is asked for is one question, what it is when an
    // IEquatable<int> is asked for another: only an int can be one. A generic interface whose one
    // type argument is a scalar type implementing it - IEquatable<int>, IComparable<BigInteger>,
    // INumber<double> - therefore asks for that type, and the token is read as that type is read
    // when asked for itself, typed value or not. To everything else that could hold a scalar, a
    // bare 5 stays the long it is to an object.
    //
    // The scalar types are the ones Gremlinq reads natively, not every type that implements an
    // interface for itself. An interface with more type arguments than one asks for none of them,
    // and neither does one whose type argument does not implement it: an int is no IList<int>.
    //
    // The answer's type depends on the requested type only: an IEquatable<int> is an int or no
    // answer, never a long. Where the token cannot be read as the scalar type, this converter
    // declines and the others answer as they did before.
    internal sealed class ScalarInterfaceConverterFactory : IConverterFactory
    {
        private sealed class ScalarInterfaceConverter<TSource, TScalar, TTarget> : IConverter<TSource, TTarget>
            where TSource : JToken
            where TScalar : TTarget
        {
            private readonly IGremlinQueryEnvironment _environment;

            public ScalarInterfaceConverter(IGremlinQueryEnvironment environment)
            {
                _environment = environment;
            }

            bool IConverter<TSource, TTarget>.TryConvert(TSource serialized, ITransformer defer, ITransformer recurse, [NotNullWhen(true)] out TTarget? value)
            {
                ArgumentNullException.ThrowIfNull(defer);
                ArgumentNullException.ThrowIfNull(recurse);

                if (recurse.TryTransform(serialized, _environment, out TScalar? scalar))
                {
                    value = scalar;
                    return true;
                }

                value = default;
                return false;
            }
        }

        private static readonly HashSet<Type> ScalarTypes =
        [
            typeof(sbyte),
            typeof(byte),
            typeof(short),
            typeof(ushort),
            typeof(int),
            typeof(uint),
            typeof(long),
            typeof(ulong),
            typeof(float),
            typeof(double),
            typeof(decimal),
            typeof(BigInteger),
            typeof(bool),
            typeof(string),
            typeof(char),
            typeof(Guid),
            typeof(DateTime),
            typeof(DateTimeOffset),
            typeof(TimeSpan)
        ];

        IConverter<TSource, TTarget>? IConverterFactory.TryCreate<TSource, TTarget>(IGremlinQueryEnvironment environment)
        {
            ArgumentNullException.ThrowIfNull(environment);

            return typeof(JToken).IsAssignableFrom(typeof(TSource)) && TryGetScalarType(typeof(TTarget)) is { } scalarType
                ? (IConverter<TSource, TTarget>?)Activator.CreateInstance(typeof(ScalarInterfaceConverter<,,>).MakeGenericType(typeof(TSource), scalarType, typeof(TTarget)), environment)
                : null;
        }

        private static Type? TryGetScalarType(Type requestedType)
        {
            if (requestedType.IsInterface && requestedType.IsGenericType)
            {
                var typeArguments = requestedType.GetGenericArguments();

                if (typeArguments.Length == 1 && ScalarTypes.Contains(typeArguments[0]) && requestedType.IsAssignableFrom(typeArguments[0]))
                    return typeArguments[0];
            }

            return null;
        }
    }
}
