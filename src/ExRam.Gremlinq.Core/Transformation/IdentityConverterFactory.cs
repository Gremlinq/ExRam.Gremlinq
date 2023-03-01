using System.Diagnostics.CodeAnalysis;
using ExRam.Gremlinq.Core.Deserialization;

namespace ExRam.Gremlinq.Core.Transformation
{
    internal sealed class IdentityConverterFactory : IConverterFactory
    {
        private sealed class IdentityConverter<TSerialized, TRequested> : IConverter<TSerialized, TRequested>
        {
            public bool TryConvert(TSerialized serialized, IGremlinQueryEnvironment environment, IDeserializer recurse, [NotNullWhen(true)] out TRequested? value)
            {
                if (serialized is TRequested requested)
                {
                    value = requested;
                    return true;
                }

                value = default;
                return false;
            }
        }

        public IConverter<TSerialized, TRequested> TryCreate<TSerialized, TRequested>() => new IdentityConverter<TSerialized, TRequested>();
    }
}
