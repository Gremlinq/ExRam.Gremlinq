using ExRam.Gremlinq.Core.Transformation;

namespace ExRam.Gremlinq.Core
{
    public interface IGremlinqConfigurator<out TConfigurator> : IGremlinQuerySourceTransformation
        where TConfigurator : IGremlinqConfigurator<TConfigurator>
    {
        TConfigurator ConfigureDeserialization(Func<ITransformer, ITransformer> deserializerTransformation);
    }
}
