// ReSharper disable HeapView.PossibleBoxingAllocation
using ExRam.Gremlinq.Providers.Core.AspNet;

using Microsoft.Extensions.DependencyInjection;

namespace ExRam.Gremlinq.Core.AspNet
{
    public static class GremlinqProviderServicesBuilderExtensions
    {
        public static IGremlinqProviderServicesBuilder<TConfigurator> FromSection<TConfigurator>(this IGremlinqProviderServicesBuilder<TConfigurator> builder, string sectionName)
            where TConfigurator : IGremlinqConfigurator<TConfigurator>
        {
            builder.Services
                .AddSingleton<IProviderConfigurationSection>(s => new ProviderConfigurationSection<TConfigurator>(s.GetRequiredService<IGremlinqConfigurationSection>(), sectionName));

            return builder;
        }

    }
}
