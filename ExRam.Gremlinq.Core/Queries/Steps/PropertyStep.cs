using System.Collections.Immutable;
using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core
{
    public sealed class PropertyStep : Step
    {
        public PropertyStep(Key key, object value, Cardinality? cardinality = default) : this(key, value, ImmutableArray<object>.Empty, cardinality)
        {

        }

        public PropertyStep(Key key, object value, ImmutableArray<object> metaProperties, Cardinality? cardinality = default)
        {
            Key = key;
            Value = value;
            Cardinality = cardinality;
            MetaProperties = metaProperties;
        }

        public Key Key { get; }
        public object Value { get; }
        public Cardinality? Cardinality { get; }
        public ImmutableArray<object> MetaProperties { get; }
    }
}
