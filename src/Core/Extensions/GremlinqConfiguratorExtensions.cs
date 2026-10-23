namespace ExRam.Gremlinq.Core
{
    /// <summary>Extension methods for provider configurators.</summary>
    public static class GremlinqConfiguratorExtensions
    {
        /// <summary>
        /// Sets the range of TinkerPop versions the server understands (<see cref="IGremlinQueryEnvironment.ServerCompatibilityRange"/>).
        /// It takes precedence over a provider's default.
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
