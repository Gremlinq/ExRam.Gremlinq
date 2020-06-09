namespace ExRam.Gremlinq.Core
{
    public readonly struct MemberMetadata
    {
        public MemberMetadata(string name, SerializationBehaviour serializationBehaviour = SerializationBehaviour.Default)
        {
            Name = name;
            SerializationBehaviour = serializationBehaviour;
        }

        public string Name { get; }
        public SerializationBehaviour SerializationBehaviour { get; }
    }
}
