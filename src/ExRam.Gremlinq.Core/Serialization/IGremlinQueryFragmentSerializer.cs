namespace ExRam.Gremlinq.Core.Serialization
{
    public interface IGremlinQueryFragmentSerializer
    {
        object Serialize<TFragment>(TFragment fragment, IGremlinQueryEnvironment gremlinQueryEnvironment);

        IGremlinQueryFragmentSerializer Override<TFragment>(GremlinQueryFragmentSerializerDelegate<TFragment> serializer);
    }
}
