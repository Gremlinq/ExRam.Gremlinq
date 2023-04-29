namespace ExRam.Gremlinq.Core
{
    public static class GremlinQueryEnvironmentExtensions
    {
        public static bool SupportsType(this IGremlinQueryEnvironment environment, Type type)
        {
            if (environment.SupportsTypeNatively(type))
                return true;

            if (type == typeof(byte[]))
                return environment.SupportsTypeNatively(typeof(string));

            if (type == typeof(TimeSpan))
                return environment.SupportsTypeNatively(typeof(double));

            return false;
        }

        public static bool SupportsTypeNatively(this IGremlinQueryEnvironment environment, Type type) => environment.NativeTypes.Contains(type);
    }
}
