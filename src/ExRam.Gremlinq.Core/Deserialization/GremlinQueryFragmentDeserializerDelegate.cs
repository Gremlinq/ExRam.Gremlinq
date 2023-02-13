namespace ExRam.Gremlinq.Core.Deserialization
{
    public delegate object? BaseGremlinQueryFragmentDeserializerDelegate<in TSerialized>(TSerialized serializedData, Type requestedType, IGremlinQueryEnvironment environment, IGremlinQueryFragmentDeserializer recurse);

    public abstract class GremlinQueryFragmentDeserializerDelegate
    {

    }

    public sealed class GremlinQueryFragmentDeserializerDelegate<TSerialized> : GremlinQueryFragmentDeserializerDelegate
    {
        private readonly Func<TSerialized, Type, IGremlinQueryEnvironment, BaseGremlinQueryFragmentDeserializerDelegate<TSerialized>, IGremlinQueryFragmentDeserializer, object?> _func;

        private GremlinQueryFragmentDeserializerDelegate(Func<TSerialized, Type, IGremlinQueryEnvironment, BaseGremlinQueryFragmentDeserializerDelegate<TSerialized>, IGremlinQueryFragmentDeserializer, object?> func)
        {
            _func = func;
        }

        public static GremlinQueryFragmentDeserializerDelegate<TSerialized> From(Func<TSerialized, Type, IGremlinQueryEnvironment, BaseGremlinQueryFragmentDeserializerDelegate<TSerialized>, IGremlinQueryFragmentDeserializer, object?> func)
        {
            return new GremlinQueryFragmentDeserializerDelegate<TSerialized>(func);
        }

        public object? Execute(TSerialized serializedData, Type requestedType, IGremlinQueryEnvironment environment, BaseGremlinQueryFragmentDeserializerDelegate<TSerialized> overridden, IGremlinQueryFragmentDeserializer recurse)
        {
            return _func(serializedData, requestedType, environment, overridden, recurse);
        }
    }
}
