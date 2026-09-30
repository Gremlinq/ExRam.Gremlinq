using Newtonsoft.Json.Linq;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using ExRam.Gremlinq.Core.Transformation;
using ExRam.Gremlinq.Core;
using Gremlin.Net.Process.Traversal;
using System.Runtime.CompilerServices;

namespace ExRam.Gremlinq.Support.NewtonsoftJson
{
    // Direction, Merge, Cardinality and the other Gremlin.Net enums are EnumWrappers rather than
    // enums: a private constructor and a handful of static instances, so Newtonsoft cannot build
    // one from "OUT" and used to leave the caller with the bare string. Each of them exposes a
    // static GetByValue, which is how GraphSON's @value turns back into the declared type.
    internal sealed class EnumWrapperConverterFactory : IConverterFactory
    {
        private sealed class EnumWrapperConverter<TTarget> : IConverter<JValue, TTarget>
        {
            // A static field on a generic type caches this per requested type.
            private static readonly MethodInfo? GetByValue = typeof(TTarget).GetMethod(nameof(GetByValue), BindingFlags.Public | BindingFlags.Static, [typeof(string)]);

            // A value GetByValue does not know - "ID" for a T, or one a newer server sends - is still a
            // value of that enumeration, just not one Gremlin.Net has a name for. Every Gremlin.Net
            // enumeration has a private constructor taking the value, and it builds one with that
            // value, equal to none of the known ones. GraphsonSupportTestBase says when an upgrade
            // takes that constructor away; until then, a missing one makes this converter decline.
            private static readonly ConstructorInfo? ValueConstructor = typeof(TTarget).GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, [typeof(string)]);

            bool IConverter<JValue, TTarget>.TryConvert(JValue serialized, ITransformer defer, ITransformer recurse, [NotNullWhen(true)] out TTarget? value)
            {
                ArgumentNullException.ThrowIfNull(serialized);
                ArgumentNullException.ThrowIfNull(defer);
                ArgumentNullException.ThrowIfNull(recurse);

                if (GetByValue is { } getByValue && serialized is { Type: JTokenType.String, Value: string enumValue })
                {
                    try
                    {
                        if (getByValue.Invoke(null, [enumValue]) is TTarget requested)
                        {
                            value = requested;

                            return true;
                        }
                    }
                    catch (TargetInvocationException ex) when (ex.InnerException is ArgumentException)
                    {
                        if (ValueConstructor?.Invoke([enumValue]) is TTarget unknown)
                        {
                            value = unknown;

                            return true;
                        }
                    }
                }

                value = default;

                return false;
            }
        }

        IConverter<TSource, TTarget>? IConverterFactory.TryCreate<TSource, TTarget>(IGremlinQueryEnvironment environment)
        {
            ArgumentNullException.ThrowIfNull(environment);

            return typeof(TSource) == typeof(JValue) && typeof(EnumWrapper).IsAssignableFrom(typeof(TTarget))
                ? Unsafe.As<IConverter<TSource, TTarget>>(new EnumWrapperConverter<TTarget>())
                : null;
        }
    }
}
