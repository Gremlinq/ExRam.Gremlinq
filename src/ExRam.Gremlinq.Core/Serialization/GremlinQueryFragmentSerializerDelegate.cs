namespace ExRam.Gremlinq.Core.Serialization
{
    public delegate object? BaseGremlinQueryFragmentSerializerDelegate<in TFragment>(TFragment fragment, IGremlinQueryEnvironment environment, IGremlinQuerySerializer recurse);

    public delegate object? GremlinQueryFragmentSerializerDelegate<TFragment>(TFragment fragment, IGremlinQueryEnvironment environment, BaseGremlinQueryFragmentSerializerDelegate<TFragment> overridden, IGremlinQuerySerializer recurse);
}
