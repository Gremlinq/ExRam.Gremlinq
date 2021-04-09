namespace ExRam.Gremlinq.Core
{
    public interface IGremlinQuerySourceTransformation
    {
        IConfigurableGremlinQuerySource Transform(IConfigurableGremlinQuerySource source);
    }
}
