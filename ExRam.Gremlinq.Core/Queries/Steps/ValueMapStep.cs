namespace ExRam.Gremlinq.Core
{
    public sealed class ValueMapStep : Step
    {
        public ValueMapStep(string[] keys)
        {
            Keys = keys;
        }

        public string[] Keys { get; }
    }
}
