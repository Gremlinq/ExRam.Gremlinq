namespace ExRam.Gremlinq.Tests.Infrastructure.GraphSon.Entities
{
    public class ClassWithInitializedMember
    {
        public string? Name { get; set; }

        public int Age { get; set; }

        public int Initialized { get; set; } = 42;
    }
}
