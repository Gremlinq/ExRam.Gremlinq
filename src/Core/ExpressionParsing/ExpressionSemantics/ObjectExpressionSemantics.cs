namespace ExRam.Gremlinq.Core.ExpressionParsing
{
    public sealed class EqualsExpressionSemantics : ObjectExpressionSemantics
    {
        public static readonly EqualsExpressionSemantics Instance = new ();

        private EqualsExpressionSemantics()
        {

        }

        public override ExpressionSemantics Flip() => this;
    }

    public sealed class NotEqualsExpressionSemantics : ObjectExpressionSemantics
    {
        public static readonly NotEqualsExpressionSemantics Instance = new();

        private NotEqualsExpressionSemantics()
        {

        }

        public override ExpressionSemantics Flip() => this;
    }

    public abstract class ObjectExpressionSemantics : ExpressionSemantics
    {
        public ExpressionSemantics TransformCompareTo(int comparison)
        {
            return this switch
            {
                LowerThanExpressionSemantics => comparison switch
                {
                    0 => LowerThanExpressionSemantics.Instance,
                    1 => LowerThanOrEqualExpressionSemantics.Instance,
                    > 1 => TrueExpressionSemantics.Instance,
                    _ => FalseExpressionSemantics.Instance
                },
                LowerThanOrEqualExpressionSemantics => comparison switch
                {
                    -1 => LowerThanExpressionSemantics.Instance,
                    0 => LowerThanOrEqualExpressionSemantics.Instance,
                    > 0 => TrueExpressionSemantics.Instance,
                    _ => FalseExpressionSemantics.Instance
                },
                EqualsExpressionSemantics => comparison switch
                {
                    -1 => LowerThanExpressionSemantics.Instance,
                    0 => EqualsExpressionSemantics.Instance,
                    1 => GreaterThanExpressionSemantics.Instance,
                    _ => FalseExpressionSemantics.Instance
                },
                GreaterThanOrEqualExpressionSemantics => comparison switch
                {
                    <= -1 => TrueExpressionSemantics.Instance,
                    0 => GreaterThanOrEqualExpressionSemantics.Instance,
                    1 => GreaterThanExpressionSemantics.Instance,
                    _ => FalseExpressionSemantics.Instance
                },
                GreaterThanExpressionSemantics => comparison switch
                {
                    < -1 => TrueExpressionSemantics.Instance,
                    -1 => GreaterThanOrEqualExpressionSemantics.Instance,
                    0 => GreaterThanExpressionSemantics.Instance,
                    _ => FalseExpressionSemantics.Instance
                },
                NotEqualsExpressionSemantics => comparison switch
                {
                    -1 => GreaterThanOrEqualExpressionSemantics.Instance,
                    0 => NotEqualsExpressionSemantics.Instance,
                    1 => LowerThanOrEqualExpressionSemantics.Instance,
                    _ => TrueExpressionSemantics.Instance
                },
                _ => throw new ExpressionNotSupportedException()
            };
        }
    }
}
