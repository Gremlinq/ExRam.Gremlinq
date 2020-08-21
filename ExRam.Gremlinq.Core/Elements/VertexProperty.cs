using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace ExRam.Gremlinq.Core.GraphElements
{
    public class VertexProperty<TValue, TMeta> : Property<TValue>, IVertexProperty
        where TMeta : class
    {
        public VertexProperty(TValue value) : base(value)
        {
            if (value is IVertexProperty)
                throw new InvalidOperationException($"Cannot assign a value of type {value.GetType().Name} to a property of type {nameof(VertexProperty<TValue, TMeta>)}.");
        }

        public static implicit operator VertexProperty<TValue, TMeta>(TValue value) => new VertexProperty<TValue, TMeta>(value);
        public static implicit operator VertexProperty<TValue, TMeta>(TValue[] value) => throw new NotSupportedException("This conversion is only intended to be used in expressions. It can't be executed reasonably.");
        public static implicit operator VertexProperty<TValue, TMeta>(VertexProperty<TValue, TMeta>[] value) => throw new NotSupportedException("This conversion is only intended to be used in expressions. It can't be executed reasonably.");

        public override string ToString()
        {
            return $"vp[{Label}->{GetValue()}]";
        }

        protected virtual IDictionary<string, object> GetProperties(IGremlinQueryEnvironment environment)
        {
            return Properties?
                .Serialize(environment, SerializationBehaviour.Default)
                .Where(x => x.key.RawKey is string)
                .ToDictionary(x => (string)x.key.RawKey, x => x.value) ?? (IDictionary<string, object>)ImmutableDictionary<string, object>.Empty;
        }

        IDictionary<string, object> IVertexProperty.GetProperties(IGremlinQueryEnvironment environment) => GetProperties(environment);

        public object? Id { get; set; }
        public string? Label { get; set; }
        public TMeta? Properties { get; set; }

        object? IElement.Id { get => Id; set => throw new InvalidOperationException($"Can't set the {nameof(Id)}-property of a {nameof(VertexProperty<TValue, TMeta>)}."); }
    }

    public class VertexProperty<TValue> : VertexProperty<TValue, IDictionary<string, object>>
    {
        public VertexProperty(TValue value) : base(value)
        {
            Properties = new Dictionary<string, object>();
        }

        public static implicit operator VertexProperty<TValue>(TValue value) => new VertexProperty<TValue>(value);
        public static implicit operator VertexProperty<TValue>(TValue[] value) => throw new NotSupportedException("This conversion is only intended to be used in expressions. It can't be executed reasonably.");
        public static implicit operator VertexProperty<TValue>(VertexProperty<TValue>[] value) => throw new NotSupportedException("This conversion is only intended to be used in expressions. It can't be executed reasonably.");

        protected override IDictionary<string, object> GetProperties(IGremlinQueryEnvironment environment) => Properties!;
    }
}
