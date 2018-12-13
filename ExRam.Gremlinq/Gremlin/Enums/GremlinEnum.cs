using ExRam.Gremlinq.Serialization;

namespace ExRam.Gremlinq
{
    public abstract class GremlinEnum<TEnum> : IGremlinQueryElement where TEnum : GremlinEnum<TEnum>
    {
        protected GremlinEnum(string name)
        {
            Name = name;
        }

        public void Accept(IGremlinQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override bool Equals(object obj)
        {
            return obj is TEnum variable && Name == variable.Name;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public string Name { get; }
    }
}
