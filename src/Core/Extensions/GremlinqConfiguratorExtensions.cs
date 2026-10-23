namespace ExRam.Gremlinq.Core
{
    /// <summary>Extension methods for provider configurators.</summary>
    public static class GremlinqConfiguratorExtensions
    {
        /// <summary>
        /// Configures the range of TinkerPop versions the server understands (<see cref="IGremlinQueryEnvironment.ServerCompatibilityRange"/>)
        /// by applying the specified transformation, as <see cref="IGremlinQueryEnvironment.ConfigureServerCompatibilityRange"/> does on
        /// the environment. The transformation is given the range set before it, a provider's default included, and takes precedence over it.
        /// Once the lowest version of the range is TinkerPop 3.8 or higher, a <see cref="DateTimeOffset"/> is sent as
        /// <c>gx:OffsetDateTime</c> rather than <c>g:Date</c>. How a server stores and compares the two depends on the server, so check
        /// this before moving the range of a graph that already holds dates to 3.8.
        /// </summary>
        /// <typeparam name="TConfigurator">The configurator type.</typeparam>
        /// <param name="configurator">The configurator.</param>
        /// <param name="compatibilityTransformation">A function that transforms the current range.</param>
        public static TConfigurator ConfigureServerCompatibilityRange<TConfigurator>(this TConfigurator configurator, Func<VersionRange, VersionRange> compatibilityTransformation)
            where TConfigurator : IGremlinqConfigurator<TConfigurator>
        {
            ArgumentNullException.ThrowIfNull(compatibilityTransformation);

            return configurator
                .ConfigureQuerySource(source => source
                    .ConfigureEnvironment(environment => environment
                        .ConfigureServerCompatibilityRange(compatibilityTransformation)));
        }
    }
}
