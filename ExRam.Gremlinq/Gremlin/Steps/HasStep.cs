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
            _key = key;
            _value = value;
        }

        public override IEnumerable<TerminalGremlinStep> Resolve(IGraphModel model)
        {
            var key = model.GetIdentifier(_key);

            yield return _value.Match(
                value => new MethodGremlinStep(Name, key, value),
                () => new MethodGremlinStep(Name, key));
        }
    }
}