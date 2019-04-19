namespace ExRam.Gremlinq.Core
{
    public interface IElementBuilder
    {
        IMetadataBuilder<TElement> Element<TElement>();
    }
}
