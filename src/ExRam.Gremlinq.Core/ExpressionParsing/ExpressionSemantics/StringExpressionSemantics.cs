namespace ExRam.Gremlinq.Core
{
    internal sealed class HasInfixExpressionSemantics : StringExpressionSemantics
    {
        public static readonly HasInfixExpressionSemantics Instance = new();

        private HasInfixExpressionSemantics()
        {

        }

        public override ExpressionSemantics Flip() => IsInfixOfExpressionSemantics.Instance;
    }

    internal sealed class StartsWithExpressionSemantics : StringExpressionSemantics
    {
        public static readonly StartsWithExpressionSemantics Instance = new();

        private StartsWithExpressionSemantics()
        {

        }

        public override ExpressionSemantics Flip() => IsPrefixOfExpressionSemantics.Instance;
    }

    internal sealed class EndsWithExpressionSemantics : StringExpressionSemantics
    {
        public static readonly EndsWithExpressionSemantics Instance = new();

        private EndsWithExpressionSemantics()
        {

        }

        public override ExpressionSemantics Flip() => IsSuffixOfExpressionSemantics.Instance;

    }

    internal sealed class IsInfixOfExpressionSemantics : StringExpressionSemantics
    {
        public static readonly IsInfixOfExpressionSemantics Instance = new();

        private IsInfixOfExpressionSemantics()
        {

        }

        public override ExpressionSemantics Flip() => HasInfixExpressionSemantics.Instance;

    }

    internal sealed class IsPrefixOfExpressionSemantics : StringExpressionSemantics
    {
        public static readonly IsPrefixOfExpressionSemantics Instance = new();

        private IsPrefixOfExpressionSemantics()
        {

        }

        public override ExpressionSemantics Flip() => StartsWithExpressionSemantics.Instance;

    }

    internal sealed class IsSuffixOfExpressionSemantics : StringExpressionSemantics
    {
        public static readonly IsSuffixOfExpressionSemantics Instance = new();

        private IsSuffixOfExpressionSemantics()
        {

        }

        public override ExpressionSemantics Flip() => EndsWithExpressionSemantics.Instance;
    }


    internal abstract class StringExpressionSemantics : ExpressionSemantics
    {
    }
}
