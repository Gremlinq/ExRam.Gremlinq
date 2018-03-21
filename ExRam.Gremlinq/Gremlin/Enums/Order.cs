using System.Text;

namespace ExRam.Gremlinq
{
    public sealed class Order : IGroovySerializable
    {
        public static readonly Order Increasing = new Order("incr");
        public static readonly Order Decreasing = new Order("decr");

        private readonly string _name;

        private Order(string name)
        {
            this._name = name;
        }

        public GroovyExpressionState Serialize(StringBuilder stringBuilder, GroovyExpressionState state)
        {
            return state
                .AppendIdentifier(stringBuilder, nameof(Order))
                .AppendField(stringBuilder, this._name);
        }
    }
}