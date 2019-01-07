namespace ExRam.Gremlinq.Core
{
    public abstract class AddElementStep : Step
    {
        protected AddElementStep(string label)
        {
            Label = label;
        }

        public string Label { get; }
    }
}
