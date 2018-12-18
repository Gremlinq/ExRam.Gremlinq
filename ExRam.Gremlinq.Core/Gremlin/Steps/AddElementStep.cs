using System.Linq;

namespace ExRam.Gremlinq.Core
{
    public abstract class AddElementStep : Step
    {
        protected AddElementStep(IGraphModel model, object value)
        {
            var type = value.GetType();

            Label = model
                .GetLabels(type)
                .FirstOrDefault() ?? type.Name;
        }

        public string Label { get; }
    }
}
