namespace ExRam.Gremlinq
{
    public sealed class AddVGremlinStep : AddElementGremlinStep
    {
        public AddVGremlinStep(object value) : base((string) "addV", value)
        {
        }
    }
}