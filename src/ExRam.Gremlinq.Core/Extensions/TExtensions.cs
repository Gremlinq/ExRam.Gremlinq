using ExRam.Gremlinq.Core.Steps;
using _Step = ExRam.Gremlinq.Core.Steps.Step;

namespace Gremlin.Net.Process.Traversal
{
    internal static class TExtensions
    {
        public static _Step? TryToStep(this T t)
        {
            return t.EnumValue switch
            {
                "id" => IdStep.Instance,
                "label" => LabelStep.Instance,
                "key" => KeyStep.Instance,
                "value" => ValueStep.Instance,
                _ => null
            };
        }
    }
}
