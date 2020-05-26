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
            return new GremlinqSetup(setup.ServiceCollection
                .AddSingleton<IGremlinqConfiguration>(serviceProvider => new GremlinqConfiguration(serviceProvider
                    .GetService<IConfiguration>()
                    .GetSection(sectionName)
                    .GetSection("Gremlinq"))));
        }

        public static GremlinqSetup UseModel(this GremlinqSetup setup, IGraphModel model)
        {
            return new GremlinqSetup(setup.ServiceCollection
                .AddSingleton(model)
                .AddSingleton<IGremlinQueryEnvironmentTransformation, UseModelTransformation>());
        }

        public static GremlinqSetup ConfigureEnvironment(this GremlinqSetup setup, Func<IGremlinQueryEnvironment, IGremlinQueryEnvironment> environmentTransformation)
        {
            return new GremlinqSetup(setup.ServiceCollection
                .AddSingleton<IGremlinQueryEnvironmentTransformation>(new EnvironmentTransformation(environmentTransformation)));
        }
    }
}
