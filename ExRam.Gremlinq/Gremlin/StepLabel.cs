using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace ExRam.Gremlinq
{
    public struct StepLabel<T> : IGremlinSerializable
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

        public (string queryString, IDictionary<string, object> parameters) Serialize(IGraphModel graphModel, IParameterCache parameterCache, bool inlineParameters)
        {
            if (inlineParameters)
                return ("'" + this.Label + "'", ImmutableDictionary<string, object>.Empty);

            var parameterName = parameterCache.Cache(this.Label);
            return (parameterName, ImmutableDictionary<string, object>.Empty.Add(parameterName, this.Label));
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
            return !object.ReferenceEquals(null, obj) && obj is StepLabel<T> && Equals((StepLabel<T>) obj);
        }

        public override int GetHashCode()
        {
            return this.Label != null ? this.Label.GetHashCode() : 0;
        }
    }
}
