using System.Collections.Generic;

namespace ExRam.Gremlinq
{
    public abstract class AddElementGremlinStep : NonTerminalGremlinStep
    {
        private readonly object _value;
        private readonly string _stepName;

        protected AddElementGremlinStep(string stepName, object value)
        {
            _value = value;
            _stepName = stepName;
        }

        public override IEnumerable<TerminalGremlinStep> Resolve(IGraphModel model)
        {
            var type = _value.GetType();
            
            yield return new MethodGremlinStep(
                _stepName,
                model
                    .TryGetLabelOfType(type)
                    .IfNone(type.Name));
        }
    }
}