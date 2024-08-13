﻿using System.Collections.Immutable;
using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core.Steps
{
    public abstract class PropertyStep : Step
    {
        public sealed class ByKeyStep : PropertyStep
        {
            public ByKeyStep(Key key, object value, Cardinality? cardinality = default) : this(key, value, ImmutableArray<KeyValuePair<string, object>>.Empty, cardinality)
            {
            }

            public ByKeyStep(Key key, object value, ImmutableArray<KeyValuePair<string, object>> metaProperties, Cardinality? cardinality = default) : base(value, metaProperties, cardinality)
            {
                Key = key;
            }

            public Key Key { get; }
        }

        public sealed class ByTraversalStep : PropertyStep
        {
            public ByTraversalStep(Traversal traversal, object value, Cardinality? cardinality = default) : this(traversal, value, ImmutableArray<KeyValuePair<string, object>>.Empty, cardinality)
            {
            }

            public ByTraversalStep(Traversal traversal, object value, ImmutableArray<KeyValuePair<string, object>> metaProperties, Cardinality? cardinality = default) : base(value, metaProperties, cardinality)
            {
                Traversal = traversal;
            }

            public Traversal Traversal { get; }
        }

        protected PropertyStep(object value, Cardinality? cardinality = default) : this(value, ImmutableArray<KeyValuePair<string, object>>.Empty, cardinality)
        {

        }

        protected PropertyStep(object value, ImmutableArray<KeyValuePair<string, object>> metaProperties, Cardinality? cardinality = default) : base(SideEffectSemanticsChange.Write)
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
