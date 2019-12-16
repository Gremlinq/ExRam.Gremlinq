using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using NullGuard;

namespace ExRam.Gremlinq.Core.GraphElements
{
    public class VertexProperty<TValue, TMeta> : Property<TValue>, IVertexProperty
        where TMeta : class
    {
        public VertexProperty(TValue value) : base(value)
        {
            Value = value;
        }

        public static implicit operator VertexProperty<TValue, TMeta>(TValue value) => new VertexProperty<TValue, TMeta>(value);
        public static implicit operator VertexProperty<TValue, TMeta>(TValue[] value) => throw new NotSupportedException();
        public static implicit operator VertexProperty<TValue, TMeta>(VertexProperty<TValue, TMeta>[] value) => throw new NotSupportedException();

        public override string ToString()
        {
            return $"vp[{Label}->{GetValue()}]";
        }

        //TODO: Honor Mask.
        internal override IDictionary<string, object> GetMetaProperties(IGraphElementPropertyModel model) => Properties?
            .Serialize(model, SerializationBehaviour.Default)
            .Where(x => x.identifier is string)
            .ToDictionary(x => (string)x.identifier, x => x.value) ?? (IDictionary<string, object>)ImmutableDictionary<string, object>.Empty;

        [AllowNull] public object? Id { get; set; }
        [AllowNull] public string? Label { get; set; }
        [AllowNull] public TMeta? Properties { get; set; }

        object? IElement.Id { get => Id; set => throw new NotSupportedException(); }
    }

    public class VertexProperty<TValue> : VertexProperty<TValue, IDictionary<string, object>>
    {
        public VertexProperty(TValue value) : base(value)
        {
            Value = value;
            Properties = new Dictionary<string, object>();
        }

        public static implicit operator VertexProperty<TValue>(TValue value) => new VertexProperty<TValue>(value);
        public static implicit operator VertexProperty<TValue>(TValue[] value) => throw new NotSupportedException();
        public static implicit operator VertexProperty<TValue>(VertexProperty<TValue>[] value) => throw new NotSupportedException();

        internal override IDictionary<string, object> GetMetaProperties(IGraphElementPropertyModel model) => Properties;
    }
}
