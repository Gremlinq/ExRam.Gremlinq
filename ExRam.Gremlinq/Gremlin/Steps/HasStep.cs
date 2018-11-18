using System.Collections.Generic;
using LanguageExt;

namespace ExRam.Gremlinq
{
    public sealed class HasStep : NonTerminalStep
    {
        private const string Name = "has";

        private readonly string _key;

        public HasStep(string key, Option<object> value = default)
        {
            _key = key;
            Value = value;
        }

        public override IEnumerable<Step> Resolve(IGraphModel model)
        {
            var key = model.GetIdentifier(_key);

            yield return Value
                .Bind<object>(v =>
                {
                    if (v == P.False)
                        return (object)GremlinQuery.Anonymous.Not(_ => _.Identity());

                    if (v == P.True)
                        return default;

                    return v;
                })
                .Match(
                    value => new MethodStep(Name, key, value),
                    () => new MethodStep(Name, key));
        }

        internal Option<object> Value { get; }
    }
}
