using System.Text;

namespace ExRam.Gremlinq
{
    public struct Lambda : IGremlinSerializable
    {
        private readonly string _lambda;

        public Lambda(string lambda)
        {
            this._lambda = lambda;
        }

        public GroovyExpressionBuilder Serialize(StringBuilder stringBuilder, GroovyExpressionBuilder builder)
        {
            return builder.AppendLambda(stringBuilder, this._lambda);
        }
    }
}