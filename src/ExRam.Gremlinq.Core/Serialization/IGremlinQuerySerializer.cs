namespace ExRam.Gremlinq.Core.Serialization
{
    public interface IGremlinQuerySerializer
    {
        object Serialize<TFragment>(TFragment fragment, IGremlinQueryEnvironment gremlinQueryEnvironment);

        IGremlinQuerySerializer Override<TFragment>(GremlinQueryFragmentSerializerDelegate<TFragment> serializer);
    }
}
