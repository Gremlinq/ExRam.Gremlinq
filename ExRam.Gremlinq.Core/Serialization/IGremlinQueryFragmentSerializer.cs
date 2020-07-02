namespace ExRam.Gremlinq.Core
{
    public interface IGremlinQueryFragmentSerializer
    {
        object Serialize<TFragment>(TFragment fragment, IGremlinQueryEnvironment gremlinQueryEnvironment);

        IGremlinQueryFragmentSerializer Override<TFragment>(GremlinQueryFragmentSerializerDelegate<TFragment> serializer);
    }
}
