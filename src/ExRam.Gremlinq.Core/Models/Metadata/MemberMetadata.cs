using System;

namespace ExRam.Gremlinq.Core
{
    public readonly struct MemberMetadata
    {
        private readonly Key? _key;

        public MemberMetadata(Key key, SerializationBehaviour serializationBehaviour = SerializationBehaviour.Default)
        {
            _key = key;
            SerializationBehaviour = serializationBehaviour;
        }

        public Key Key
        {
            get
            {
                if (_key == null)
                    throw new InvalidOperationException($"Cannot retrieve the {nameof(Key)} property of an uninitialized {nameof(MemberMetadata)} struct.");

                return _key.Value;
            }
        }

        public SerializationBehaviour SerializationBehaviour { get; }
    }
}
