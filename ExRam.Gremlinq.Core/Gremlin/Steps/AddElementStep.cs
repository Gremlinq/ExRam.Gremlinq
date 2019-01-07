using System.Linq;

namespace ExRam.Gremlinq.Core
{
    public abstract class AddElementStep : Step
    {
        protected AddElementStep(IGraphModel model, object value)
        {
            var type = value.GetType();

            Label = model
                .TryGetConstructiveLabel(type)
                .IfNone(type.Name); //TODO: We don't want to work outside the model!
        }

        public string Label { get; }
    }
}
