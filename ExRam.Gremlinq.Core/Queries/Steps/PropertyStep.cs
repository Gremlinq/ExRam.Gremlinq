using System;
using ExRam.Gremlinq.Core.Serialization;
using LanguageExt;

namespace ExRam.Gremlinq.Core
{
    public sealed class PropertyStep : Step
    {
        public PropertyStep(object key, object value, Option<Cardinality> cardinality = default) : this(key, value, Array.Empty<object>(), cardinality)
        {

        }

        public PropertyStep(object key, object value, object[] metaProperties, Option<Cardinality> cardinality = default)
        {
            Key = key;
            Value = value;
            Cardinality = cardinality;
            MetaProperties = metaProperties;
        }

        public object Key { get; }
        public object Value { get; }
        public object[] MetaProperties { get; }
        public Option<Cardinality> Cardinality { get; }
    }
}
