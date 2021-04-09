namespace ExRam.Gremlinq.Core
{
    public interface IGremlinQueryEnvironmentTransformation
    {
        IGremlinQueryEnvironment Transform(IGremlinQueryEnvironment environment);
    }
}
