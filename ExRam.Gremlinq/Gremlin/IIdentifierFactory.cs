namespace ExRam.Gremlinq
{
    public interface IIdentifierFactory
    {
        StepLabel<TElement> CreateStepLabel<TElement>();
        string CreateIndexName();
    }
}