namespace ExRam.Gremlinq
{
    public interface IIdentifierFactory
    {
        StepLabel<T> CreateStepLabel<T>();
        string CreateIndexName();
    }
}