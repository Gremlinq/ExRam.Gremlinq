namespace ExRam.Gremlinq.Core.AspNet
{
    public interface IGremlinQueryEnvironmentTransformation
    {
        IGremlinQueryEnvironment Transform(IGremlinQueryEnvironment environment);
    }
}