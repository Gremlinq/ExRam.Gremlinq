namespace ExRam.Gremlinq.Core
{
    public readonly struct PropertyMetadata
    {
        public PropertyMetadata(string name, SerializationBehaviour serializationBehaviour = SerializationBehaviour.Default)
        {
            Name = name;
            SerializationBehaviour = serializationBehaviour;
        }

        public string Name { get; }
        public SerializationBehaviour SerializationBehaviour { get; }
    }
}
