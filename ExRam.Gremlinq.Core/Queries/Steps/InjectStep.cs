namespace ExRam.Gremlinq.Core
{
    public sealed class InjectStep : Step
    {
        public object[] Elements { get; }

        public InjectStep(object[] elements)
        {
            Elements = elements;
        }
    }
}
