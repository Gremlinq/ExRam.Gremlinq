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

        public (string queryString, IDictionary<string, object> parameters) Serialize(IParameterCache parameterCache, bool inlineParameters)
        {
            return (this._value, ImmutableDictionary<string, object>.Empty);
        }
    }
}