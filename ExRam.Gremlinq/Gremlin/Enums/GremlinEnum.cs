using System.Text;

namespace ExRam.Gremlinq
{
    public abstract class GremlinEnum<TEnum> : IGroovySerializable where TEnum : GremlinEnum<TEnum>
    {
        protected GremlinEnum(string name)
        {
            Name = name;
        }

        public GroovyExpressionState Serialize(StringBuilder stringBuilder, GroovyExpressionState state)
        {
            return state
                .AppendIdentifier(stringBuilder, GetType().Name)
                .AppendField(stringBuilder, Name);
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
