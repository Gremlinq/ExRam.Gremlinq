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
    }
}
