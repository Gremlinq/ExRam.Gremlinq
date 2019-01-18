namespace ExRam.Gremlinq.Core
{
    public sealed class Order : GremlinEnum<Order>
    {
        public static readonly Order Increasing = new Order("incr");
        public static readonly Order Decreasing = new Order("decr");

        private Order(string name) : base(name)
        {
        }
    }
}
