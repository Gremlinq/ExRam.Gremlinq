using System.Text;

namespace ExRam.Gremlinq
{
    internal struct OrP : IGremlinSerializable
    {
        private readonly P[] _predicates;

        public OrP(params P[] predicates)
        {
            this._predicates = predicates;
        }

        public MethodStringBuilder Serialize(MethodStringBuilder builder, IParameterCache parameterCache)
        {
            for (var i = 0; i < this._predicates.Length; i++)
            {
                builder = i != 0 
                    ? builder.AppendMethod("or", new object[] { this._predicates[i] }, parameterCache) 
                    : this._predicates[i].Serialize(builder, parameterCache);
            }

            return builder;
        }
    }
}