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

    public class StepLabel<T> : StepLabel
    {
        public StepLabel(string label) : base(label)
        {
        }

        public static StepLabel<T> CreateNew()
        {
            return new StepLabel<T>(Guid.NewGuid().ToString("N"));
        }

        public override void Serialize(StringBuilder builder, IParameterCache parameterCache)
        {
            builder.Append(parameterCache.Cache(this.Label));
        }

        public static bool operator ==(T a, StepLabel<T> b)
        {
            throw new NotImplementedException("Only for expressions.");
        }

        public static bool operator !=(T a, StepLabel<T> b)
        {
            throw new NotImplementedException("Only for expressions.");
        }

        public bool Equals(StepLabel<T> other)
        {
            return string.Equals(this.Label, other.Label);
        }

        public override bool Equals(object obj)
        {
            return obj is StepLabel<T> label && this.Equals(label);
        }

        public override int GetHashCode()
        {
            return this.Label != null ? this.Label.GetHashCode() : 0;
        }
    }
}
