namespace ExRam.Gremlinq.Core
{
    public interface IGremlinQuerySourceTransformation
    {
        IGremlinQuerySource Transform(IGremlinQuerySource source);
    }
}
