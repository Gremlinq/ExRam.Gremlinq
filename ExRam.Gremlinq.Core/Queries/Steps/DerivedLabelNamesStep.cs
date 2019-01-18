namespace ExRam.Gremlinq.Core
{
    public abstract class DerivedLabelNamesStep : Step
    {
        protected DerivedLabelNamesStep(string[] labels)
        {
            Labels = labels;
        }

        public string[] Labels { get; }
    }
}
