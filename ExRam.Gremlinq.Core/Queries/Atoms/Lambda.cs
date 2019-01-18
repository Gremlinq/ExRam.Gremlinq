using ExRam.Gremlinq.Core.Serialization;

namespace ExRam.Gremlinq.Core
{
    public struct Lambda : IGremlinQueryAtom
    {
        public string LambdaString { get; }

        public Lambda(string lambdaString)
        {
            LambdaString = lambdaString;
        }

        public void Accept(IGremlinQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
