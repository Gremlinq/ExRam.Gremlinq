using System.Collections.Generic;

namespace ExRam.Gremlinq.Core.GraphElements
{
    public interface IMeta
    {
        object Value { get; }
        IDictionary<string, object> Properties { get; }
    }

    public sealed class Meta<TElement> : IMeta
    {
        private readonly Dictionary<string, object> _properties = new Dictionary<string, object>();

        public Meta(TElement value)
        {
            Value = value;
        }

        private Meta()
        {

        }

        public IDictionary<string, object> Properties => _properties;

        public static implicit operator TElement(Meta<TElement> meta)
        {
            return meta.Value;
        }

        public static implicit operator Meta<TElement>(TElement value)
        {
            return new Meta<TElement>(value);
        }

        public TElement Value { get; set; }

        object IMeta.Value => Value;
    }
}
