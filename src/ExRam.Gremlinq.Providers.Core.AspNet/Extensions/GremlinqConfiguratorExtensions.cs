using Microsoft.Extensions.Configuration;

namespace ExRam.Gremlinq.Providers.Core.AspNet.Extensions
{
    public static class GremlinqConfiguratorExtensions
    {
        public static TConfigurator ConfigureBaseFrom<TConfigurator>(
            this TConfigurator configurator,
            IConfiguration configuration) where TConfigurator : IProviderConfigurator<TConfigurator>
        {
            //if (configuration["Alias"] is { } alias)
            //    configurator = configurator.SetAlias(alias);

            return configurator;
        }
    }
}
