namespace ExRam.Gremlinq
{
    public struct Lambda : IQueryElement
    {
        public string LambdaString { get; }

        public Lambda(string lambdaString)
        {
            LambdaString = lambdaString;
        }

        public void Accept(IQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
