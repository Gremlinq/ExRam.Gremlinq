namespace ExRam.Gremlinq.Core
{
    public interface IGremlinQuerySerializer
    {
        IGremlinQuerySerializer OverrideAtomSerializer<TAtom>(AtomSerializer<TAtom> atomSerializer);

        object Serialize(IGremlinQuery query);
    }
}
