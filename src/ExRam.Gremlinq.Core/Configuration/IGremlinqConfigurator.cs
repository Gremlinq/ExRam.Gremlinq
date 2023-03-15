namespace ExRam.Gremlinq.Core
{
    public interface IGremlinqConfigurator<out TConfigurator> : IGremlinQuerySourceTransformation
        where TConfigurator : IGremlinqConfigurator<TConfigurator>
    {
        TConfigurator ConfigureQuerySource(Func<IGremlinQuerySource, IGremlinQuerySource> transformation);
    }
}
