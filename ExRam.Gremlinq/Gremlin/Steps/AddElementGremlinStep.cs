using System.Collections.Generic;

namespace ExRam.Gremlinq
{
    public abstract class AddElementGremlinStep : NonTerminalGremlinStep
    {
        private readonly object _value;
        private readonly string _stepName;

        protected AddElementGremlinStep(string stepName, object value)
        {
            this._value = value;
            this._stepName = stepName;
        }

        public override IEnumerable<TerminalGremlinStep> Resolve(IGraphModel model)
        {
            yield return new TerminalGremlinStep(
                this._stepName,
                model
                    .TryGetLabelOfType(this._value.GetType()).IfNone(this._value.GetType().Name));
        }
    }
}