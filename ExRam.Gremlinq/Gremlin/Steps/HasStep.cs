using System.Collections.Generic;
using LanguageExt;

namespace ExRam.Gremlinq
{
    public sealed class HasStep : NonTerminalGremlinStep
    {
        private readonly string _key;
        private readonly Option<object> _value;

        public HasStep(string key, Option<object> value = default(Option<object>))
        {
            this._key = key;
            this._value = value;
        }

        public override IEnumerable<TerminalGremlinStep> Resolve(IGraphModel model)
        {
            var key = this._key == model.IdPropertyName
                ? new SpecialGremlinString("T.id")
                : (object)this._key;

            yield return this._value.Match(
                value => new TerminalGremlinStep("has", key, value),
                () => new TerminalGremlinStep("has", key));
        }
    }
}