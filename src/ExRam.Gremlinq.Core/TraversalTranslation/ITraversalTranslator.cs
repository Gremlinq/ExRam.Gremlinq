namespace ExRam.Gremlinq.Core
{
    public interface ITraversalTranslator
    {
        Traversal Translate(IGremlinQueryBase query);
    }
}
