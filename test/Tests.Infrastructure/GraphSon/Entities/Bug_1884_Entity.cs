namespace ExRam.Gremlinq.Tests.Infrastructure.GraphSon.Entities
{
    public class Bug_1884_Entity
    {
        public Bug_1884_Entity()
        {
            MyEnum1 = Bug_1884_Enum.ValueB;
        }

        // this default value is overwritten during deserialization (as expected)
        public DateTime CreatedAt { get; set; } = TimeProvider.System.GetUtcNow().DateTime.Date;

        // after deserialization, the value of this property is always ValueB (bug)
        public Bug_1884_Enum MyEnum1 { get; set; }

        // after deserialization the value of this property is always false (bug)
        public bool IsDeleted { get; set; } = false;
    }
}
