namespace ExRam.Gremlinq
{
    public sealed class AddEGremlinStep : AddElementGremlinStep
    {
        public AddEGremlinStep(object value) : base((string) "addE", value)
        {
        }
    }
}