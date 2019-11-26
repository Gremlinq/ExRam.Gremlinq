namespace ExRam.Gremlinq.Core
{
    public abstract class TextP : P
    {
        public sealed class StartingWith : TextP
        {
            public StartingWith(string value) : base(value)
            {
            }

            internal override P WorkaroundLimitations(GremlinqOptions gremlinqOptions)
            {
                if ((gremlinqOptions.GetValue(GremlinqOption.DisabledTextPredicates) & DisabledTextPredicates.StartingWith) == 0)
                    return base.WorkaroundLimitations(gremlinqOptions);

                string upperBound;

                if (Value[Value.Length - 1] == char.MaxValue)
                    upperBound = Value + char.MinValue;
                else
                {
                    var upperBoundChars = Value.ToCharArray();

                    upperBoundChars[upperBoundChars.Length - 1]++;
                    upperBound = new string(upperBoundChars);
                }

                return new Between(Value, upperBound);
            }
        }

        public sealed class EndingWith : TextP
        {
            public EndingWith(string value) : base(value)
            {
            }

            internal override P WorkaroundLimitations(GremlinqOptions gremlinqOptions)
            {
                if ((gremlinqOptions.GetValue(GremlinqOption.DisabledTextPredicates) & DisabledTextPredicates.EndingWith) != 0)
                    throw new ExpressionNotSupportedException();

                return base.WorkaroundLimitations(gremlinqOptions);
            }
        }

        public sealed class Containing : TextP
        {
            public Containing(string value) : base(value)
            {
            }

            internal override P WorkaroundLimitations(GremlinqOptions gremlinqOptions)
            {
                if ((gremlinqOptions.GetValue(GremlinqOption.DisabledTextPredicates) & DisabledTextPredicates.Containing) != 0)
                    throw new ExpressionNotSupportedException();

                return base.WorkaroundLimitations(gremlinqOptions);
            }
        }

        protected TextP(string value)
        {
            Value = value;
        }

        internal override bool ContainsOnlyStepLabels()
        {
            return false;
        }

        public string Value { get; }
    }
}
