namespace ExRam.Gremlinq
{
    public struct Lambda : IGremlinSerializable
    {
        private readonly string _lambda;

        public Lambda(string lambda)
        {
            this._lambda = lambda;
        }

        public GroovyExpressionBuilder Serialize(GroovyExpressionBuilder builder, IParameterCache parameterCache)
        {
            return builder.AppendLambda(this._lambda);
        }
    }
}