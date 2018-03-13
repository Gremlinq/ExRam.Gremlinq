using System;
using System.Text;

namespace ExRam.Gremlinq
{
    public abstract class StepLabel : IGremlinSerializable
    {
        protected StepLabel(string label)
        {
            this.Label = label;
        }
        
        public abstract void Serialize(StringBuilder builder, IParameterCache parameterCache);

        public string Label { get; }
    }

    public class StepLabel<TElement> : StepLabel
    {
        public StepLabel(string label) : base(label)
        {
        }

        public static StepLabel<TElement> CreateNew()
        {
            return new StepLabel<TElement>(Guid.NewGuid().ToString("N"));
        }

        public override void Serialize(StringBuilder builder, IParameterCache parameterCache)
        {
            builder.Append(parameterCache.Cache(this.Label));
        }

        public static bool operator ==(TElement a, StepLabel<TElement> b)
        {
            throw new NotImplementedException("Only for expressions.");
        }

        public static bool operator !=(TElement a, StepLabel<TElement> b)
        {
            throw new NotImplementedException("Only for expressions.");
        }

        public bool Equals(StepLabel<TElement> other)
        {
            return string.Equals(this.Label, other.Label);
        }

        public override bool Equals(object obj)
        {
            return obj is StepLabel<TElement> label && this.Equals(label);
        }

        public override int GetHashCode()
        {
            return this.Label != null ? this.Label.GetHashCode() : 0;
        }
    }
}
