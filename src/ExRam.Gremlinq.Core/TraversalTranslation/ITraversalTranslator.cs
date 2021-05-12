namespace ExRam.Gremlinq.Core
{
    public interface ITraversalTranslator
    {
        Traversal Translate(StepStack steps, QueryFlags queryFlags, IGremlinQueryEnvironment environment);
    }
}
