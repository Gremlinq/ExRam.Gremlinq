using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using NullGuard;

namespace ExRam.Gremlinq.Core.GraphElements
{
    public abstract class Property
    {
        public override string ToString()
        {
            return $"p[{Key}->{GetValue()}]";
        }

        internal abstract object GetValue();
        internal abstract IDictionary<string, object> GetMetaProperties(IGraphElementPropertyModel model);

        [AllowNull] public string Key { get; set; }
    }

    public class Property<TValue> : Property
    {
        public Property(TValue value)
        {
            Value = value;
        }

        protected Property()
        {
        }

        internal override object GetValue() => Value;

        internal override IDictionary<string, object> GetMetaProperties(IGraphElementPropertyModel model) => ImmutableDictionary<string, object>.Empty;

        public static implicit operator Property<TValue>(TValue value) => new Property<TValue>(value);
        public static implicit operator Property<TValue>(TValue[] value) => throw new NotSupportedException();
        public static implicit operator Property<TValue>(Property<TValue>[] value) => throw new NotSupportedException();

        [AllowNull] public TValue Value { get; set; }
    }
}
