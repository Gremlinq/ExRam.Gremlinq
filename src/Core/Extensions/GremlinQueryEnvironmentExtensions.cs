using ExRam.Gremlinq.Core.Steps;

namespace ExRam.Gremlinq.Core
{
    /// <summary>Extension methods for <see cref="IGremlinQueryEnvironment"/>.</summary>
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

        // The step that filters out all traversers on the server: none() as long as the server understands
        // TinkerPop versions below 3.8, discard() once it does not.
        internal static Step GetDiscardStep(this IGremlinQueryEnvironment environment) => environment.ServerCompatibilityRange.IsAtLeast(TinkerPopVersions.V3_8_0)
            ? DiscardStep.Instance
            : NoneStep.Instance;
    }
}
