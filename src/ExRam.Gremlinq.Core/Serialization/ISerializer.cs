namespace ExRam.Gremlinq.Core.Serialization
{
    public interface ISerializer
    {
        object Serialize<TFragment>(TFragment fragment, IGremlinQueryEnvironment gremlinQueryEnvironment);

        ISerializer Override<TFragment>(GremlinQueryFragmentSerializerDelegate<TFragment> serializer);
    }
}

