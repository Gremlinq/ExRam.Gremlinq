using System.Collections.Generic;

namespace ExRam.Gremlinq
{
    public sealed class ReplaceElementPropertyStep : NonTerminalGremlinStep
    {
        private readonly string _key;
        private readonly object _value;
        private readonly AddElementPropertiesStep _baseStep;

        public ReplaceElementPropertyStep(AddElementPropertiesStep baseStep, string key, object value) : base()
        {
            this._baseStep = baseStep;
            this._key = key;
            this._value = value;
        }

        public override IEnumerable<TerminalGremlinStep> Resolve(IGraphModel model)
        {
            foreach (var step in this._baseStep.Resolve(model))
            {
                if (step.Name == "property" && step.Parameters.Count > 0 && step.Parameters[0] as string == this._key)
                    yield return new TerminalGremlinStep("property", this._key, this._value);
                else
                    yield return step;
            }
        }
    }
}