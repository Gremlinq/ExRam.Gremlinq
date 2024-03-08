#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

using NSubstitute;

namespace ExRam.Gremlinq.Providers.Core.Tests
{
    public class ClientFactoryTests
    {
        [Fact]
        public void Configured_base_factory_doesnt_have_to_be_same_type()
        {
            var baseFactory = Substitute
                .For<IWebSocketGremlinqClientFactory>();

            var poolClient = baseFactory
                .Pool()
                .ConfigureBaseFactory(factory => factory
                    .ConfigureClient(client => client));
        }
    }
}
