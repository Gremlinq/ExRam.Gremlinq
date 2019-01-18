namespace ExRam.Gremlinq.Core
{
    public abstract class AddElementStep : Step
    {
        protected AddElementStep(IGraphElementModel elementModel, object value)
        {
            Label = elementModel.TryGetConstructiveLabel(value.GetType()).IfNone(value.GetType().Name);
        }

        public string Label { get; }
    }
}
