using System.Diagnostics.CodeAnalysis;

namespace ExRam.Gremlinq.Core.Deserialization
{
    public static class ConverterFactory
    {
        private sealed class IdentityConverterFactory : IConverterFactory
        {
            private sealed class IdentityConverter<TSerialized, TRequested> : IConverter<TSerialized, TRequested>
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

            public IConverter<TSerialized, TRequested>? TryCreate<TSerialized, TRequested>() => new IdentityConverter<TSerialized, TRequested>();
        }

        public static readonly IConverterFactory Identity = new IdentityConverterFactory();
    }
}
