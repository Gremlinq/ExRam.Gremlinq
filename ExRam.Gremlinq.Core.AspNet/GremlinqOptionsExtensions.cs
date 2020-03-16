using Microsoft.Extensions.DependencyInjection;

namespace ExRam.Gremlinq.Core.AspNet
{
    public static class GremlinqOptionsExtensions
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

        public static GremlinqOptions UseModel(this GremlinqOptions options, IGraphModel model)
        {
            return new GremlinqOptions(options.ServiceCollection
                .AddSingleton(model)
                .AddSingleton<IGremlinQueryEnvironmentTransformation, UseModelTransformation>());
        }
    }
}