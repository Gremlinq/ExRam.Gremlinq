// ReSharper disable HeapView.PossibleBoxingAllocation
using ExRam.Gremlinq.Providers.Core.AspNet;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ExRam.Gremlinq.Core.AspNet
{
    public static class GremlinqProviderServicesBuilderExtensions
    {
        public static IGremlinqServicesBuilder<TConfigurator> FromSection<TConfigurator>(this IGremlinqServicesBuilder<TConfigurator> builder, string sectionName)
            where TConfigurator : IGremlinqConfigurator<TConfigurator>
        {
            builder.Services
                .AddSingleton(s => new ProviderConfigurationSection<TConfigurator>(s.GetRequiredService<IGremlinqConfigurationSection>(), sectionName))
                .AddSingleton<IProviderConfigurationSection>(s => s.GetRequiredService<ProviderConfigurationSection<TConfigurator>>())
                .TryAddTransient<IEffectiveGremlinqConfigurationSection>(s => s.GetRequiredService<ProviderConfigurationSection<TConfigurator>>());

            return builder;
        }

    }
}
