using System;

namespace ExRam.Gremlinq.Core
{
    public readonly struct MemberMetadata
    {
        private readonly Key? _key;

        public MemberMetadata(Key name, SerializationBehaviour serializationBehaviour = SerializationBehaviour.Default)
        {
            _key = name;
            SerializationBehaviour = serializationBehaviour;
        }

        public Key Key
        {
            get
            {
                if (_key == null)
                    throw new InvalidOperationException();

                return _key.Value;
            }
        }

        public SerializationBehaviour SerializationBehaviour { get; }
    }
}
