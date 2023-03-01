namespace ExRam.Gremlinq.Core.Serialization
{
    public delegate object? GremlinQueryFragmentSerializerDelegate<in TFragment>(TFragment fragment, IGremlinQueryEnvironment environment, IGremlinQuerySerializer recurse);
}
