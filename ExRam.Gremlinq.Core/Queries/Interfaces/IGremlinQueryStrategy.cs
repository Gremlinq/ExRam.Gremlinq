namespace ExRam.Gremlinq.Core
{
    public interface IGremlinQueryStrategy
    {
        IGremlinQueryBase Apply(IGremlinQueryBase query);
    }
}
