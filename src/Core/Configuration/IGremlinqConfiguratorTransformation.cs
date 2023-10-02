namespace ExRam.Gremlinq.Core
{
    public interface IGremlinqConfiguratorTransformation<TConfigurator>
        where TConfigurator : IGremlinqConfigurator<TConfigurator>
    {
        TConfigurator Transform(TConfigurator configurator);
    }
}
