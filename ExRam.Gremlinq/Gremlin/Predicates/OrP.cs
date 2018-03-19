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

        public GroovyExpressionBuilder Serialize(StringBuilder stringBuilder, GroovyExpressionBuilder builder)
        {
            for (var i = 0; i < this._predicates.Length; i++)
            {
                builder = i != 0 
                    ? builder.AppendMethod(stringBuilder, "or", new object[] { this._predicates[i] }) 
                    : this._predicates[i].Serialize(stringBuilder, builder);
            }

            return builder;
        }
    }
}