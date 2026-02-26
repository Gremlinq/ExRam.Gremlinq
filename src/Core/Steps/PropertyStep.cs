using System.Collections.Immutable;
using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core.Steps
{
    public abstract class PropertyStep : Step
    {
        public sealed class ByKeyStep : PropertyStep
        {
            public ByKeyStep(Key key, object value, Cardinality? cardinality = null) : this(key, value, ImmutableArray<KeyValuePair<string, object>>.Empty, cardinality)
            {
                ArgumentNullException.ThrowIfNull(value);

            }

            public ByKeyStep(Key key, object value, ImmutableArray<KeyValuePair<string, object>> metaProperties, Cardinality? cardinality = null) : base(value, metaProperties, cardinality)
            {
                ArgumentNullException.ThrowIfNull(value);

                Key = key;
            }

            public Key Key { get; }
        }

        public sealed class ByTraversalStep : PropertyStep
        {
            public ByTraversalStep(Traversal traversal, object value, Cardinality? cardinality = null) : this(traversal, value, ImmutableArray<KeyValuePair<string, object>>.Empty, cardinality)
            {
                ArgumentNullException.ThrowIfNull(value);

            }

            public ByTraversalStep(Traversal traversal, object value, ImmutableArray<KeyValuePair<string, object>> metaProperties, Cardinality? cardinality = null) : base(value, metaProperties, cardinality)
            {
                ArgumentNullException.ThrowIfNull(value);

                Traversal = traversal;
            }

            public Traversal Traversal { get; }
        }

        protected PropertyStep(object value, Cardinality? cardinality = null) : this(value, ImmutableArray<KeyValuePair<string, object>>.Empty, cardinality)
        {

        }

        protected PropertyStep(object value, ImmutableArray<KeyValuePair<string, object>> metaProperties, Cardinality? cardinality = null) : base(SideEffectSemanticsChange.Write)
        {
            Value = value;
            Cardinality = cardinality;
            MetaProperties = metaProperties;
        }

        public object Value { get; }
        public Cardinality? Cardinality { get; }
        public ImmutableArray<KeyValuePair<string, object>> MetaProperties { get; }
    }
}
