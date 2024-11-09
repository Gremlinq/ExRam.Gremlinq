namespace ExRam.Gremlinq.Tests.Infrastructure
{
    public interface ISourceFileNameProvider<T>
    {
        static abstract string GetSourceFileName();
    }
}
