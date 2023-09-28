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

        public static GremlinqSetup UseConfigurationSection(this GremlinqSetup setup, string sectionName)
        {
            setup.ServiceCollection.AddSingleton(new GremlinqSetupInfo(sectionName));

            return setup;
        }

        public static GremlinqSetup ConfigureQuerySource(this GremlinqSetup setup, Func<IGremlinQuerySource, IGremlinQuerySource> sourceTranformation)
        {
            setup.ServiceCollection.AddSingleton<IGremlinQuerySourceTransformation>(new SourceTransformation(sourceTranformation));

            return setup;
        }
    }
}
