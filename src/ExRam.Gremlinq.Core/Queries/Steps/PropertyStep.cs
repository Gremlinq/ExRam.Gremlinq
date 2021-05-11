using System.Collections.Generic;
using System.Collections.Immutable;
using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core
{
    public abstract class PropertyStep : Step
    {
        public sealed class ByKeyStep : PropertyStep
        {
            public ByKeyStep(Key key, object value, Cardinality? cardinality = default, QuerySemantics? semantics = default) : base(value, cardinality, semantics)
            {
                Key = key;
            }

            public ByKeyStep(Key key, object value, ImmutableArray<KeyValuePair<string, object>> metaProperties, Cardinality? cardinality = default, QuerySemantics? semantics = default) : base(value, metaProperties, cardinality, semantics)
            {
                Key = key;
            }

            public Key Key { get; }
        }

        public sealed class ByTraversalStep : PropertyStep
        {
            public ByTraversalStep(Traversal traversal, object value, Cardinality? cardinality = default, QuerySemantics? semantics = default) : base(value, cardinality, semantics)
            {
                Traversal = traversal;
            }

            public ByTraversalStep(Traversal traversal, object value, ImmutableArray<KeyValuePair<string, object>> metaProperties, Cardinality? cardinality = default, QuerySemantics? semantics = default) : base(value, metaProperties, cardinality, semantics)
            {
                Traversal = traversal;
            }

            public Traversal Traversal { get; }
        }

        protected PropertyStep(object value, Cardinality? cardinality = default, QuerySemantics? semantics = default) : this(value, ImmutableArray<KeyValuePair<string, object>>.Empty, cardinality, semantics)
        {

        }

        protected PropertyStep(object value, ImmutableArray<KeyValuePair<string, object>> metaProperties, Cardinality? cardinality = default, QuerySemantics? semantics = default) : base(semantics)
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
