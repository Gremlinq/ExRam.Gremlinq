using System;
using System.Collections.Generic;

using ExRam.Gremlinq.Core.Models;

namespace ExRam.Gremlinq.Core.GraphElements
{
    public class VertexProperty<TValue, TMeta> : Property<TValue>, IVertexProperty
    {
        public VertexProperty(TValue value) : base(value)
        {
            if (value is IVertexProperty)
                throw new InvalidOperationException($"Cannot assign a value of type {value.GetType().Name} to a property of type {nameof(VertexProperty<TValue, TMeta>)}.");
        }

        public static implicit operator VertexProperty<TValue, TMeta>(TValue value) => new(value);
        public static implicit operator VertexProperty<TValue, TMeta>(TValue[] value) => throw new NotSupportedException("This conversion is only intended to be used in expressions. It can't be executed reasonably.");
        public static implicit operator VertexProperty<TValue, TMeta>(VertexProperty<TValue, TMeta>[] value) => throw new NotSupportedException("This conversion is only intended to be used in expressions. It can't be executed reasonably.");

        public override string ToString()
        {
            return $"vp[{Label}->{GetValue()}]";
        }

        protected virtual IEnumerable<KeyValuePair<string, object>> GetProperties(IGremlinQueryEnvironment environment)
        {
            if (Properties is { } properties)
            {
                foreach (var (key, value) in properties.Serialize(environment, SerializationBehaviour.Default))
                {
                    if (key.RawKey is string str)
                        yield return new KeyValuePair<string, object>(str, value);
                }
            }
        }

        IEnumerable<KeyValuePair<string, object>> IVertexProperty.GetProperties(IGremlinQueryEnvironment environment) => GetProperties(environment);

        public object? Id { get; private set; }
        public string? Label { get; private set; }
        public TMeta? Properties { get; set; }
    }

    public class VertexProperty<TValue> : VertexProperty<TValue, IDictionary<string, object>>
    {
        public VertexProperty(TValue value) : base(value)
        {
            Properties = new Dictionary<string, object>();
        }

        public static implicit operator VertexProperty<TValue>(TValue value) => new(value);
        public static implicit operator VertexProperty<TValue>(TValue[] value) => throw new NotSupportedException("This conversion is only intended to be used in expressions. It can't be executed reasonably.");
        public static implicit operator VertexProperty<TValue>(VertexProperty<TValue>[] value) => throw new NotSupportedException("This conversion is only intended to be used in expressions. It can't be executed reasonably.");

        protected override IEnumerable<KeyValuePair<string, object>> GetProperties(IGremlinQueryEnvironment environment) => Properties!;
    }
}
