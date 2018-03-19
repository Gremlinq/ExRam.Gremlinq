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

        public GroovyExpressionBuilder Serialize(GroovyExpressionBuilder builder)
        {
            for (var i = 0; i < this._predicates.Length; i++)
            {
                builder = i != 0 
                    ? builder.AppendMethod("or", new object[] { this._predicates[i] }) 
                    : this._predicates[i].Serialize(builder);
            }

            return builder;
        }
    }
}