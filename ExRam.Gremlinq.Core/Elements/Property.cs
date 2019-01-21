using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using NullGuard;

namespace ExRam.Gremlinq.Core.GraphElements
{
    public abstract class Property
    {
        [AllowNull] public string Key { get; set; }

        internal abstract object GetValue();
        internal abstract IDictionary<string, object> GetMetaProperties();
    }

    public class Property<TValue> : Property
    {
        internal override object GetValue() => Value;

        internal override IDictionary<string, object> GetMetaProperties() => ImmutableDictionary<string, object>.Empty;

        public static implicit operator Property<TValue>(TValue value) => new VertexProperty<TValue>(value);
        public static implicit operator Property<TValue>(TValue[] value) => throw new NotSupportedException();

        [AllowNull] public TValue Value { get; set; }
    }
}
