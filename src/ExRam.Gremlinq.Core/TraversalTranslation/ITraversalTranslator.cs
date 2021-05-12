namespace ExRam.Gremlinq.Core
{
    public interface ITraversalTranslator
    {
        Traversal Translate(StepStack steps, bool allowElementProjection, IGremlinQueryEnvironment environment);
    }
}
