namespace ExRam.Gremlinq.Core
{
    public interface IGremlinqConfigurator<out TSelf> : IGremlinQuerySourceTransformation
        where TSelf : IGremlinqConfigurator<TSelf>
    {
        TSelf ConfigureQuerySource(Func<IGremlinQuerySource, IGremlinQuerySource> transformation);
    }
}
