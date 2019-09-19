namespace ExRam.Gremlinq.Core
{
    public sealed class ValuesStep : Step
    {
        public ValuesStep(string[] keys)
        {
            Keys = keys;
        }

        public string[] Keys { get; }
    }
}
