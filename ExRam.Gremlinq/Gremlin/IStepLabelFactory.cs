namespace ExRam.Gremlinq
{
    public interface IStepLabelFactory
    {
        StepLabel<T> Create<T>();
    }
}