namespace ExRam.Gremlinq
{
    public abstract class DerivedLabelNamesStep : Step
    {
        protected DerivedLabelNamesStep(string stepName)
        {
            StepName = stepName;
        }

        public string StepName { get; }
    }

    public sealed class DerivedLabelNamesStep<TElement> : DerivedLabelNamesStep
    {
        public static DerivedLabelNamesStep<TElement> HasLabel = new DerivedLabelNamesStep<TElement>("hasLabel");
        public static DerivedLabelNamesStep<TElement> Out = new DerivedLabelNamesStep<TElement>("out");
        public static DerivedLabelNamesStep<TElement> In = new DerivedLabelNamesStep<TElement>("in");
        public static DerivedLabelNamesStep<TElement> Both = new DerivedLabelNamesStep<TElement>("both");
        public static DerivedLabelNamesStep<TElement> OutE = new DerivedLabelNamesStep<TElement>("outE");
        public static DerivedLabelNamesStep<TElement> InE = new DerivedLabelNamesStep<TElement>("inE");
        public static DerivedLabelNamesStep<TElement> BothE = new DerivedLabelNamesStep<TElement>("bothE");

        private DerivedLabelNamesStep(string stepName) : base(stepName)
        {
        }

        public override void Accept(IQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
