using System.Collections.Generic;
using LanguageExt;

namespace ExRam.Gremlinq
{
    public sealed class HasStep : NonTerminalGremlinStep
    {
        private const string Name = "has";

        private readonly string _key;
        private readonly Option<object> _value;

        public HasStep(string key, Option<object> value = default)
        {
            this._key = key;
            this._value = value;
        }

        public override IEnumerable<TerminalGremlinStep> Resolve(IGraphModel model)
        {
            var key = model.GetIdentifier(this._key);

            yield return this._value.Match(
                value => new MethodGremlinStep(Name, key, value),
                () => new MethodGremlinStep(Name, key));
        }
    }
}