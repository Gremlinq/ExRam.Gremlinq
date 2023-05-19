using ExRam.Gremlinq.Core.Transformation;

namespace ExRam.Gremlinq.Core.Tests.Fixtures
{
    public abstract class SerializationFixture<TSerialized> : GremlinqTestFixture
    {
        protected SerializationFixture(IGremlinQuerySource source) : base(source)
        {
        }

        public override Task Verify<TElement>(IGremlinQueryBase<TElement> query)
        {
            var env = query.AsAdmin().Environment;

            return GremlinqTestBase.Current.Verify(env
                .Serializer
                .TransformTo<TSerialized>()
                .From(query, env));
        }
    }
}
