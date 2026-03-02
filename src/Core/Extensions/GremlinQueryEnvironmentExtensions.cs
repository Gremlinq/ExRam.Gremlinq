namespace ExRam.Gremlinq.Core
{
    public static class GremlinQueryEnvironmentExtensions
    {
        /// <summary>
        /// Determines whether the environment supports the given type, either natively or via a known conversion.
        /// </summary>
        /// <param name="environment">The query environment.</param>
        /// <param name="type">The type to check.</param>
        public static bool SupportsType(this IGremlinQueryEnvironment environment, Type type)
        {
            ArgumentNullException.ThrowIfNull(environment);
            ArgumentNullException.ThrowIfNull(type);

            if (environment.SupportsTypeNatively(type))
                return true;

            if (type == typeof(byte[]))
                return environment.SupportsTypeNatively(typeof(string));

            if (type == typeof(TimeSpan))
                return environment.SupportsTypeNatively(typeof(double));

            return false;
        }

        /// <summary>
        /// Determines whether the environment natively supports the given type.
        /// </summary>
        /// <param name="environment">The query environment.</param>
        /// <param name="type">The type to check.</param>
        public static bool SupportsTypeNatively(this IGremlinQueryEnvironment environment, Type type)
        {
            ArgumentNullException.ThrowIfNull(environment);
            ArgumentNullException.ThrowIfNull(type);

            return environment.NativeTypes.Contains(type);
        }
    }
}
