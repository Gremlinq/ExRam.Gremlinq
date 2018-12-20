using System;

namespace ExRam.Gremlinq.Core
{
    public abstract class DerivedLabelNamesStep : Step
    {
        protected DerivedLabelNamesStep(string[] labels)
        {
            if (labels.Length == 0)
                throw new ArgumentException($"{nameof(labels)} may not be empty.");

            Labels = labels;
        }

        public string[] Labels { get; }
    }
}
