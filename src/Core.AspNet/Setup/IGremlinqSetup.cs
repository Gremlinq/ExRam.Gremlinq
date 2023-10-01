using Microsoft.Extensions.DependencyInjection;

namespace ExRam.Gremlinq.Core.AspNet
{
    public interface IGremlinqSetup
    {
        IGremlinqSetup ConfigureQuerySource<TTransformation>()
            where TTransformation : class, IGremlinQuerySourceTransformation;

        IGremlinqSetup UseConfigurationSection(string sectionName);
        
        IGremlinqSetup ConfigureQuerySource(Func<IGremlinQuerySource, IGremlinQuerySource> sourceTranformation);

        IServiceCollection Services { get; }
    }
}
