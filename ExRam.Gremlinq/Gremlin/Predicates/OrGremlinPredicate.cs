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

        public string Serialize(IParameterCache parameterCache)
        {
            var builder = new StringBuilder();

            for (var i = 0; i < this._predicates.Length; i++)
            {
                if (i != 0)
                    builder.Append(".or(");

                var subQuery = this._predicates[i].Serialize(parameterCache);

                builder.Append(subQuery);

                if (i != 0)
                    builder.Append(")");
            }

            return builder.ToString();
        }
    }
}