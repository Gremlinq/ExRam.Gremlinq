namespace ExRam.Gremlinq
{
    public abstract class AddElementStep : Step
    {
        protected AddElementStep(IGraphModel model, object value)
        {
            var type = value.GetType();

            Label = model
                .TryGetLabel(type)
                .IfNone(type.Name);
        }

        public string Label { get; }
    }
}
