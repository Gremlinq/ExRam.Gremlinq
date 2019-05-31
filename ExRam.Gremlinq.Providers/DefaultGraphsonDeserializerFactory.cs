using System.Runtime.CompilerServices;
using ExRam.Gremlinq.Core;
using Newtonsoft.Json;

namespace ExRam.Gremlinq.Providers
{
    public sealed class DefaultGraphsonDeserializerFactory : IGraphsonDeserializerFactory
    {
        private readonly ConditionalWeakTable<IGraphModel, GraphsonDeserializer> _serializers = new ConditionalWeakTable<IGraphModel, GraphsonDeserializer>();

        public JsonSerializer Get(IGraphModel model)
        {
            return _serializers.GetValue(
                model,
                closureModel => new GraphsonDeserializer(closureModel));
        }
    }
}
