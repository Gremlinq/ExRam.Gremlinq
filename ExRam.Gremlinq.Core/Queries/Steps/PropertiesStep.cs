namespace ExRam.Gremlinq.Core
{
    public sealed class PropertiesStep : Step
    {
        public PropertiesStep(string[] keys)
        {
            Keys = keys;
        }

        public string[] Keys { get; }
    }
}
