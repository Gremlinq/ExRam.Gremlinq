using System.Collections.Generic;

namespace ExRam.Gremlinq
{
    public abstract class AddElementStep : NonTerminalStep
    {
        private readonly object _value;
        private readonly string _stepName;

        protected AddElementStep(string stepName, object value)
        {
            _value = value;
            _stepName = stepName;
        }

        public override IEnumerable<Step> Resolve(IGraphModel model)
        {
            var type = _value.GetType();
            
            yield return new MethodStep.MethodStep1(
                _stepName,
                model
                    .TryGetLabelOfType(type)
                    .IfNone(type.Name));
        }
    }
}
