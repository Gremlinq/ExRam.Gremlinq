namespace ExRam.Gremlinq.Core
{
    public interface IGremlinqConfigurator<out TConfigurator> : IGremlinQuerySourceTransformation
        where TConfigurator : IGremlinqConfigurator<TConfigurator>
    {
    }
}
