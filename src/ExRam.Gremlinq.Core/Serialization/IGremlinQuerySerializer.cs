using System;

namespace ExRam.Gremlinq.Core.Serialization
{
    public interface IGremlinQuerySerializer
    {
        ISerializedQuery Serialize(IGremlinQueryBase query);

        IGremlinQuerySerializer ConfigureFragmentSerializer(Func<IGremlinQueryFragmentSerializer, IGremlinQueryFragmentSerializer> transformation);
    }
}
