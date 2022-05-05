namespace ExRam.Gremlinq.Core.Serialization
{
    public delegate object BaseGremlinQueryFragmentSerializerDelegate<in TFragment>(TFragment fragment, IGremlinQueryEnvironment environment, IGremlinQueryFragmentSerializer recurse);

    public delegate object GremlinQueryFragmentSerializerDelegate<TFragment>(TFragment fragment, IGremlinQueryEnvironment environment, BaseGremlinQueryFragmentSerializerDelegate<TFragment> overridden, IGremlinQueryFragmentSerializer recurse);
}
