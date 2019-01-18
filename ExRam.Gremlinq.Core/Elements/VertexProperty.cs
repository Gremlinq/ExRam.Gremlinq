using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using LanguageExt;
using NullGuard;

namespace ExRam.Gremlinq.Core.GraphElements
{
    public class VertexProperty<TValue, TMeta> : Property<TValue>, IVertexProperty
    {
        public VertexProperty(TValue value)
        {
            Value = value;
        }

        protected VertexProperty()
        {

        }

        public static implicit operator TValue(VertexProperty<TValue, TMeta> meta) => meta.Value;

        public static implicit operator VertexProperty<TValue, TMeta>(TValue value) => new VertexProperty<TValue, TMeta>(value);

        internal override IDictionary<string, object> GetMetaProperties() => Properties?.Serialize().ToDictionary(x => x.Item1.Name, x => x.Item2) ?? (IDictionary<string, object>)ImmutableDictionary<string, object>.Empty;

        [AllowNull] public object Id { get; set; }
        [AllowNull] public string Label { get; set; }
        [AllowNull] public TMeta Properties { get; set; }
    }

    public class VertexProperty<TValue> : VertexProperty<TValue, IDictionary<string, object>>
    {
        protected VertexProperty()
        {
            Properties = new Dictionary<string, object>();
        }

        public VertexProperty(TValue value) : this()
        {
            Value = value;
        }

        public static implicit operator TValue(VertexProperty<TValue> meta) => meta.Value;

        public static implicit operator VertexProperty<TValue>(TValue value) => new VertexProperty<TValue>(value);

        internal override IDictionary<string, object> GetMetaProperties() => Properties;
    }
}
