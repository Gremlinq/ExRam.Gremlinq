using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExRam.Gremlinq.Core.AspNet
{
    public static class GremlinqSetupExtensions
    {
        private sealed class SourceTransformation : IGremlinQuerySourceTransformation
        {
            private readonly Func<IGremlinQuerySource, IGremlinQuerySource> _sourceTransformation;

            public SourceTransformation(Func<IGremlinQuerySource, IGremlinQuerySource> sourceTransformation)
            {
                _sourceTransformation = sourceTransformation;
            }

            public IGremlinQuerySource Transform(IGremlinQuerySource source)
            {
                return _sourceTransformation(source);
            }
        }

        private sealed class EnvironmentTransformation : IGremlinQuerySourceTransformation
        {
            private readonly Func<IGremlinQueryEnvironment, IGremlinQueryEnvironment> _environmentTransformation;

            public EnvironmentTransformation(Func<IGremlinQueryEnvironment, IGremlinQueryEnvironment> environmentTransformation)
            {
                _environmentTransformation = environmentTransformation;
            }

            public IGremlinQuerySource Transform(IGremlinQuerySource source)
            {
                return source.ConfigureEnvironment(_environmentTransformation);
            }
        }

        public static GremlinqSetup UseConfigurationSection(this GremlinqSetup setup, string sectionName)
        {
            return setup.RegisterTypes(serviceCollection => serviceCollection
                .AddSingleton<IGremlinqConfiguration>(serviceProvider => new GremlinqConfiguration(serviceProvider
                    .GetRequiredService<IConfiguration>()
                    .GetSection(sectionName)
                    .GetSection("Gremlinq"))));
        }

        public static GremlinqSetup RegisterTypes(this GremlinqSetup setup, Action<IServiceCollection> registration)
        {
            registration(setup.ServiceCollection);

            return setup;
        }

        public static GremlinqSetup ConfigureQuerySource(this GremlinqSetup setup, Func<IGremlinQuerySource, IGremlinQuerySource> sourceTranformation)
        {
            return setup.RegisterTypes(serviceCollection => serviceCollection
                .AddSingleton<IGremlinQuerySourceTransformation>(new SourceTransformation(sourceTranformation)));
        }

        public static GremlinqSetup ConfigureEnvironment(this GremlinqSetup setup, Func<IGremlinQueryEnvironment, IGremlinQueryEnvironment> environmentTransformation)
        {
            return setup.RegisterTypes(serviceCollection => serviceCollection
                .AddSingleton<IGremlinQuerySourceTransformation>(new EnvironmentTransformation(environmentTransformation)));
        }
    }
}
