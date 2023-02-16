using System.Diagnostics.CodeAnalysis;

namespace ExRam.Gremlinq.Core.Deserialization
{
    public static class DeserializationTransformationFactory
    {
        private sealed class IdentityDeserializationTransformationFactory : IDeserializationTransformationFactory
        {
            private sealed class IdentityDeserializationTransformation<TSerialized, TRequested> : IDeserializationTransformation<TSerialized, TRequested>
            {
                public bool Transform(TSerialized serialized, IGremlinQueryEnvironment environment, IGremlinQueryFragmentDeserializer recurse, [NotNullWhen(true)] out TRequested? value)
                {
                    if (typeof(TRequested).IsInstanceOfType(serialized))
                    {
                        value = (TRequested)(object)serialized!;

                        return true;
                    }

                    value = default;
                    return false;
                }
            }

            public IDeserializationTransformation<TSerialized, TRequested>? TryCreate<TSerialized, TRequested>() => new IdentityDeserializationTransformation<TSerialized, TRequested>();
        }

        public static readonly IDeserializationTransformationFactory Identity = new IdentityDeserializationTransformationFactory();
    }
}
