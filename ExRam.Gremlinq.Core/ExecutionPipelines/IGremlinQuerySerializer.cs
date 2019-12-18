namespace ExRam.Gremlinq.Core
{
    public interface IGremlinQuerySerializer
    {
        IGremlinQuerySerializer OverrideFragmentSerializer<TFragment>(QueryFragmentSerializer<TFragment> queryFragmentSerializer);

        object? Serialize(IGremlinQueryBase query);
    }
}
