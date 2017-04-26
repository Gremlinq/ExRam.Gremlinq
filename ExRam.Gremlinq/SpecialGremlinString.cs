using System.Collections.Generic;
using System.Collections.Immutable;

namespace ExRam.Gremlinq
{
    internal struct SpecialGremlinString : IGremlinSerializable
    {
        private readonly string _value;

        public SpecialGremlinString(string value)
        {
            this._value = value;
        }

        public (string queryString, IDictionary<string, object> parameters) Serialize(IParameterNameProvider parameterNameProvider, bool inlineParameters)
        {
            return (_value, ImmutableDictionary<string, object>.Empty);
        }
    }
}