using ExRam.Gremlinq.Core.Serialization;

namespace ExRam.Gremlinq.Core
{
    public abstract class TextP : P
    {
        public sealed class StartingWith : TextP
        {
            public StartingWith(string value) : base(value)
            {
            }

            public override void Accept(IGremlinQueryElementVisitor visitor)
            {
                visitor.Visit(this);
            }

            internal override P WorkaroundLimitations(Options options)
            {
                if ((options.DisabledTextPredicates & DisabledTextPredicates.StartingWith) == 0)
                    return base.WorkaroundLimitations(options);

                var upperBound = Value;

                if (Value[Value.Length - 1] == char.MaxValue)
                    upperBound = Value + char.MinValue;
                else
                {
                    var upperBoundChars = Value.ToCharArray();

                    upperBoundChars[upperBoundChars.Length - 1]++;
                    upperBound = new string(upperBoundChars);
                }

                return new P.Between(Value, upperBound);
            }
        }

        public sealed class EndingWith : TextP
        {
            public EndingWith(string value) : base(value)
            {
            }

            public override void Accept(IGremlinQueryElementVisitor visitor)
            {
                visitor.Visit(this);
            }

            internal override P WorkaroundLimitations(Options options)
            {
                if ((options.DisabledTextPredicates & DisabledTextPredicates.EndingWith) != 0)
                    throw new ExpressionNotSupportedException();

                return base.WorkaroundLimitations(options);
            }
        }

        protected TextP(string value)
        {
            Value = value;
        }

        internal override bool ContainsSingleStepLabel()
        {
            return false;
        }

        public string Value { get; }
    }
}
