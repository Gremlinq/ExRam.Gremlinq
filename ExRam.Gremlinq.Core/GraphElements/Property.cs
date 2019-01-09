using System.Collections.Generic;
using System.Collections.Immutable;
using NullGuard;

namespace ExRam.Gremlinq.Core.GraphElements
{
    public class Property<TValue> : PropertyBase
    {
        internal override object GetValue() => Value;

        internal override IDictionary<string, object> GetMetaProperties() => ImmutableDictionary<string, object>.Empty;

        [AllowNull] public string Key { get; set; }
        [AllowNull] public TValue Value { get; set; }
    }
}
