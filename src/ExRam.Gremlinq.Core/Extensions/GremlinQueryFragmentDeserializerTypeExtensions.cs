using System.Reflection;
using ExRam.Gremlinq.Core.Deserialization;

namespace ExRam.Gremlinq.Core
{
    public static class GremlinQueryFragmentDeserializerTypeExtensions
    {
        public readonly struct FluentForType
        {
            private readonly Type _type;
            private readonly IGremlinQueryFragmentDeserializer _deserializer;

            public FluentForType(IGremlinQueryFragmentDeserializer deserializer, Type type)
            {
                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                    type = type.GetGenericArguments()[0];

                _type = type;
                _deserializer = deserializer;
            }

            public object? From<TSerialized>(TSerialized serialized, IGremlinQueryEnvironment environment)
            {
                var methodName = _type.IsValueType
                    ? nameof(FromStruct)
                    : nameof(FromClass);

                try
                {
                    return typeof(FluentForType)
                        .GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic)!
                        .MakeGenericMethod(typeof(TSerialized), _type)
                        .Invoke(this, new object?[] { serialized, environment });
                }
                catch(TargetInvocationException ex) //TODO: This is to be made into a delegate and stuff.
                {
                    throw ex.InnerException!;
                }
            }

            private object? FromClass<TSerialized, TFragment>(TSerialized serialized, IGremlinQueryEnvironment environment)
                where TFragment : class
            {
                return _deserializer.TryDeserialize<TFragment>().From(serialized, environment);
            }

            private object? FromStruct<TSerialized, TFragment>(TSerialized serialized, IGremlinQueryEnvironment environment)
                where TFragment : struct
            {
                return _deserializer.TryDeserialize<TFragment>().From(serialized, environment);
            }
        }

        public static FluentForType TryDeserialize(this IGremlinQueryFragmentDeserializer deserializer, Type type)
        {
            return new FluentForType(deserializer, type);
        }
    }
}
