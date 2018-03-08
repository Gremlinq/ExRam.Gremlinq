using System.Collections.Generic;
using System.Text;

namespace ExRam.Gremlinq
{
    internal struct OrGremlinPredicate : IGremlinSerializable
    {
        private readonly GremlinPredicate[] _predicates;

        public OrGremlinPredicate(params GremlinPredicate[] predicates)
        {
            this._predicates = predicates;
        }

        public (string queryString, IDictionary<string, object> parameters) Serialize(IParameterCache parameterCache)
        {
            var builder = new StringBuilder();
            var dict = new Dictionary<string, object>();

            for (var i = 0; i < this._predicates.Length; i++)
            {
                if (i != 0)
                    builder.Append(".or(");

                var (subQuery, subDict) = this._predicates[i].Serialize(parameterCache);

                foreach(var kvp in subDict)
                {
                    dict.Add(kvp.Key, kvp.Value);
                }

                builder.Append(subQuery);

                if (i != 0)
                    builder.Append(")");
            }

            return (builder.ToString(), dict);
        }
    }
}