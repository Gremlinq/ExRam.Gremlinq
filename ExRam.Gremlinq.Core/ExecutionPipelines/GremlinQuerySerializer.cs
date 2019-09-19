using System;

namespace ExRam.Gremlinq.Core
{
    public static class GremlinQuerySerializer
    {
        private sealed class InvalidGremlinQuerySerializer : IGremlinQuerySerializer
        {
            public object Serialize(IGremlinQuery query)
            {
                throw new InvalidOperationException($"{nameof(Serialize)} must not be called on {nameof(GremlinQuerySerializer)}.{nameof(Invalid)}. If you are getting this exception while executing a query, configure a proper {nameof(IGremlinQuerySerializer)} on your {nameof(GremlinQuerySource)}.");
            }
        }

        public static readonly GremlinqOption<bool> WorkaroundTinkerpop2112 = new GremlinqOption<bool>(false);

        public static readonly IGremlinQuerySerializer Invalid = new InvalidGremlinQuerySerializer();

        public static readonly IGremlinQuerySerializer Groovy = GremlinQuerySerializerBuilder
            .Invalid
            .AddGremlinSteps()
            .AddGroovy()
            .Build();
    }
}
