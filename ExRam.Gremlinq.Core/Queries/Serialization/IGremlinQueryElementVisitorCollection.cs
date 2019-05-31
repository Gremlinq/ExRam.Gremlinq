using LanguageExt;

namespace ExRam.Gremlinq.Core.Serialization
{
    public interface IGremlinQueryElementVisitorCollection
    {
        IGremlinQueryElementVisitorCollection TryAdd<TSerializedQuery, TVisitor>() where TVisitor : IGremlinQueryElementVisitor<TSerializedQuery>, new();
        IGremlinQueryElementVisitorCollection Set<TSerializedQuery, TVisitor>() where TVisitor : IGremlinQueryElementVisitor<TSerializedQuery>, new();

        Option<IGremlinQueryElementVisitor<TSerializedQuery>> TryGet<TSerializedQuery>();
    }
}
