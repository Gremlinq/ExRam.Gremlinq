using System.Diagnostics.CodeAnalysis;

namespace ExRam.Gremlinq.Core.Transformation
{
    internal sealed class IdentityConverterFactory : IConverterFactory
    {
        private sealed class IdentityConverter<TSource, TRequested> : IConverter<TSource, TRequested>
        {
            public bool TryConvert(TSource serialized, IGremlinQueryEnvironment environment, ITransformer recurse, [NotNullWhen(true)] out TRequested? value)
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

        public IConverter<TSource, TRequested> TryCreate<TSource, TRequested>() => new IdentityConverter<TSource, TRequested>();
    }
}
