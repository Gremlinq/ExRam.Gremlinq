using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExRam.Gremlinq.Core.AspNet
{
    public static class GremlinqSetupExtensions
    {
        private sealed class UseModelTransformation : IGremlinQueryEnvironmentTransformation
        {
            private readonly IGraphModel _model;

            public UseModelTransformation(IGraphModel model)
            {
                this._model = model;
            }

            public IGremlinQueryEnvironment Transform(IGremlinQueryEnvironment environment)
            {
                return environment.UseModel(_model);
            }
        }

        private sealed class EnvironmentTransformation : IGremlinQueryEnvironmentTransformation
        {
            private readonly Func<IGremlinQueryEnvironment, IGremlinQueryEnvironment> _environmentTransformation;

            public EnvironmentTransformation(Func<IGremlinQueryEnvironment, IGremlinQueryEnvironment> environmentTransformation)
            {
                _environmentTransformation = environmentTransformation;
            }

            public IGremlinQueryEnvironment Transform(IGremlinQueryEnvironment environment)
            {
                return _environmentTransformation(environment);
            }
        }

        public static GremlinqSetup UseConfigurationSection(this GremlinqSetup setup, string sectionName)
        {
            return setup.RegisterTypes(serviceCollection => serviceCollection
                .AddSingleton<IGremlinqConfiguration>(serviceProvider => new GremlinqConfiguration(serviceProvider
                    .GetServiceOrThrow<IConfiguration>()
                    .GetSection(sectionName)
                    .GetSection("Gremlinq"))));
        }

        public static GremlinqSetup UseModel(this GremlinqSetup setup, IGraphModel model)
        {
            return setup.RegisterTypes(serviceCollection => serviceCollection
                .AddSingleton(model)
                .AddSingleton<IGremlinQueryEnvironmentTransformation, UseModelTransformation>());
        }

        public static GremlinqSetup RegisterTypes(this GremlinqSetup setup, Action<IServiceCollection> registration)
        {
            registration(setup.ServiceCollection);

            return setup;
        }

        public static GremlinqSetup ConfigureEnvironment(this GremlinqSetup setup, Func<IGremlinQueryEnvironment, IGremlinQueryEnvironment> environmentTransformation)
        {
            return setup.RegisterTypes(serviceCollection => serviceCollection
                .AddSingleton<IGremlinQueryEnvironmentTransformation>(new EnvironmentTransformation(environmentTransformation)));
        }
    }
}
