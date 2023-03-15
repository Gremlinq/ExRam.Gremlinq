using ExRam.Gremlinq.Core.Transformation;

namespace ExRam.Gremlinq.Core
{
    public sealed class GremlinqConfigurator : IGremlinqConfigurator<GremlinqConfigurator>
    {
        private readonly Func<ITransformer, ITransformer> _deserializerTransformation;

        public GremlinqConfigurator() : this(_ => _)
        {
        }

        public GremlinqConfigurator(Func<ITransformer, ITransformer> deserializerTransformation)
        {
            _deserializerTransformation = deserializerTransformation;
        }

        public GremlinqConfigurator ConfigureDeserialization(Func<ITransformer, ITransformer> deserializerTransformation) => new (deserializerTransformation);

        public IGremlinQuerySource Transform(IGremlinQuerySource source) => source
            .ConfigureEnvironment(environment => environment
                .ConfigureDeserializer(deserializer => _deserializerTransformation(deserializer)));
    }
}
