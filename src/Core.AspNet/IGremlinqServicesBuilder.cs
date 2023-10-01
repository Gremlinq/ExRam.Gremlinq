using Microsoft.Extensions.DependencyInjection;

namespace ExRam.Gremlinq.Core.AspNet
{
    public interface IGremlinqServicesBuilder
    {
        IGremlinqServicesBuilder ConfigureQuerySource<TTransformation>()
            where TTransformation : class, IGremlinQuerySourceTransformation;

        IGremlinqServicesBuilder UseConfigurationSection(string sectionName);
        
        IGremlinqServicesBuilder ConfigureQuerySource(Func<IGremlinQuerySource, IGremlinQuerySource> sourceTranformation);

        IServiceCollection Services { get; }
    }
}
