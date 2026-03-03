namespace ExRam.Gremlinq.Core.ExpressionParsing
{
    /// <summary>Represents semantics that always evaluate to <see langword="true"/>.</summary>
    public sealed class TrueExpressionSemantics : ConstantExpressionSemantics
    {
        /// <summary>Gets the singleton instance.</summary>
        public static readonly TrueExpressionSemantics Instance = new();

        private TrueExpressionSemantics()
        {

        }
    }

    /// <summary>Represents semantics that always evaluate to <see langword="false"/>.</summary>
    public sealed class FalseExpressionSemantics : ConstantExpressionSemantics
    {
        /// <summary>Gets the singleton instance.</summary>
        public static readonly FalseExpressionSemantics Instance = new();

        private FalseExpressionSemantics()
        {

        }
    }

    /// <summary>Base class for expression semantics that represent constant boolean results.</summary>
    public abstract class ConstantExpressionSemantics : ExpressionSemantics
    {
        /// <inheritdoc />
        public override ExpressionSemantics Flip() => this;
    }
}
