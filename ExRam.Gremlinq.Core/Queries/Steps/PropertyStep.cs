using System;
using Gremlin.Net.Process.Traversal;
using NullGuard;

namespace ExRam.Gremlinq.Core
{
    public sealed class PropertyStep : Step
    {
        public PropertyStep(object key, object value, Cardinality? cardinality = default) : this(key, value, Array.Empty<object>(), cardinality)
        {

        }

        public PropertyStep(object key, object value, object[] metaProperties, Cardinality? cardinality = default)
        {
            Key = key;
            Value = value;
            Cardinality = cardinality;
            MetaProperties = metaProperties;
        }

        public object Key { get; }
        public object Value { get; }
        public object[] MetaProperties { get; }
        [AllowNull] public Cardinality? Cardinality { get; }
    }
}
