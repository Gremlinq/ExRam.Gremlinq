namespace ExRam.Gremlinq.Core
{
    /// <summary>Extension methods for provider configurators.</summary>
    public static class GremlinqConfiguratorExtensions
    {
        /// <summary>
        /// Sets the range of TinkerPop versions the server understands (<see cref="IGremlinQueryEnvironment.ServerCompatibilityRange"/>).
        /// It takes precedence over a provider's default.
        /// Once the lowest version of the range is TinkerPop 3.8 or higher, a <see cref="DateTimeOffset"/> is sent as
        /// <c>gx:OffsetDateTime</c> rather than <c>g:Date</c>. How a server stores and compares the two depends on the server, so check
        /// this before moving the range of a graph that already holds dates to 3.8.
        /// </summary>
        /// <typeparam name="TConfigurator">The configurator type.</typeparam>
        /// <param name="configurator">The configurator.</param>
        /// <param name="compatibility">The range of TinkerPop versions the server understands.</param>
        public static TConfigurator WithCompatibility<TConfigurator>(this TConfigurator configurator, VersionRange compatibility)
            where TConfigurator : IGremlinqConfigurator<TConfigurator> => configurator
                .ConfigureQuerySource(source => source
                    .ConfigureEnvironment(environment => environment
                        .ConfigureServerCompatibilityRange(_ => compatibility)));
    }
}
