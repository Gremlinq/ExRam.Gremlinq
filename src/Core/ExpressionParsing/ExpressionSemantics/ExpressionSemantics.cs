namespace ExRam.Gremlinq.Core.ExpressionParsing
{
    /// <summary>Base class for expression semantics that define how LINQ expressions translate to Gremlin predicates.</summary>
    public abstract class ExpressionSemantics
    {
        /// <summary>Returns the flipped (operand-swapped) semantics.</summary>
        public abstract ExpressionSemantics Flip();
    }
}
