using System;

namespace ExRam.Gremlinq
{
    public struct StepLabel<T>
    {
        public StepLabel(string label)
        {
            this.Label = label;
        }

        public static StepLabel<T> CreateNew()
        {
            return new StepLabel<T>(Guid.NewGuid().ToString("N"));
        }

        public string Label { get; }
    }
}
